using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using iADAATPA.MTProvider.Extensions;
using iADAATPA.MTProvider.Models;

namespace iADAATPA.MTProvider.API
{
    public class Client : HttpClient, IClient
    {
        private string _authToken;
        public Client(string baseAddress, string authToken = null)
        {
            BaseAddress = new Uri(baseAddress);
            _authToken = authToken;
        }
        public async Task<List<string>> Translate(List<string> sources, String source, String target)
        {
            if (_authToken == null)
            {
                throw new InvalidOperationException("authToken is null");
            }

            var res = await this.PostAsJsonAsync("translate",
                new TranslationRequestItem {
                    Token = _authToken,
                    Source = source,
                    Target = target,
                    Segments = sources }).ConfigureAwait(false);
            await res.EnsureSuccessStatusCodeAsync<GenericResponse>();

            var respItem = await res.Content.ReadAsAsync<TranslationResponseItem>().ConfigureAwait(false);
            var translations = respItem.Data.Segments
                .Select(x => x.Translation)
                .ToList();
            return translations;
        }

        public async Task<bool> ValidateToken(string authToken)
        {
            // describesuppliers is a low-cost method we use instead of a dedicated one
            var res = await this.GetAsync("describesuppliers/" + Uri.EscapeDataString(authToken),
                HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            if (res.IsSuccessStatusCode)
            {
                return true;
            }
            else if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return false;
            }
            string respMessage = await res.Content.ReadAsStringAsync();
            throw new SimpleHttpResponseException<string>(res.StatusCode, res.ReasonPhrase, respMessage);
        }
    }
}
