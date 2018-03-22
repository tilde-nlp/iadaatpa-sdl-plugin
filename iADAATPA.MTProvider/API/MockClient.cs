using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iADAATPA.MTProvider.API
{
    public class MockClient : IClient
    {
        public async Task<List<string>> Translate(List<string> sources, string source, string target)
            => sources.Select(x => (new Random().NextDouble() > 0.5 ? "YOLO " : "LOL ") + x.ToUpperInvariant()).ToList();
    }
}
