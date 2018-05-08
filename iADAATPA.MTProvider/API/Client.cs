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

            Dictionary<string, string> segments =
                sources
                .Select((value, index) => new { value, index })
                .ToDictionary(x => "s" + x.index.ToString(), x => x.value);

            var res = await this.PostAsJsonAsync("translate",
                new TranslationRequestItem {
                    Token = _authToken,
                    Source = source,
                    Target = target,
                    Segments = segments }).ConfigureAwait(false);
            await res.EnsureSuccessStatusCodeAsync();

            var respItem = await res.Content.ReadAsAsync<TranslationResponseItem>().ConfigureAwait(false);
            var translations = respItem.Data.Segments.ToList()
                .OrderBy(x => int.Parse(x.Key.Substring(1)))
                .Select(x => x.Value.Translation)
                .ToList();
            return translations;
        }

        public async Task<bool> ValidateToken(string authToken)
        {
            // describesuppliers is a low-cost method we use instead of a dedicated one
            var res = await this.GetAsync("describesuppliers/" + Uri.EscapeDataString(authToken),
                HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            string respMessage = null;
            if (res.IsSuccessStatusCode)
            {
                return true;
            }
            //else if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized) // Currently this doesn't work
            else if (res.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                respMessage = await res.Content.ReadAsStringAsync();
                if (respMessage.Contains("Invalid <token>"))
                {
                    return false;
                }
            }
            else
            {
                respMessage = await res.Content.ReadAsStringAsync();
            }
            throw new SimpleHttpResponseException(res.StatusCode, res.ReasonPhrase, respMessage);
        }
    }
}
