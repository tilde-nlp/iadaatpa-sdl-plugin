using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using iADAATPA.MTProvider.Model;

namespace iADAATPA.MTProvider.API
{
    public class Client : HttpClient
    {
        private string _authToken;
        public Client(string baseAddress, string authToken)
        {
            BaseAddress = new Uri(baseAddress);
            _authToken = authToken;
        }
        public async Task<List<string>> Translate(List<string> sources, String source, String target)
        {
            Dictionary<string, string> segments =
                sources
                .Select((value, index) => new { value, index })
                .ToDictionary(x => x.index.ToString(), x => x.value);

            var res = await this.PostAsJsonAsync("translate",
                new TranslationRequestItem {
                    Token = _authToken,
                    Source = source,
                    Target = target,
                    Segments = segments }).ConfigureAwait(false);
            res.EnsureSuccessStatusCode();

            var respItem = await res.Content.ReadAsAsync<TranslationResponseItem>().ConfigureAwait(false);
            var translations = respItem.Data.Segments.ToList()
                .OrderBy(x => int.Parse(x.Key))
                .Select(x => x.Value.Translation)
                .ToList();
            return translations;
        }
    }
}
