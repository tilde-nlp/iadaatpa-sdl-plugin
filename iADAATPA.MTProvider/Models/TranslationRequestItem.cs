using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace iADAATPA.MTProvider.Models
{
    class TranslationRequestItem
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("target")]
        public string Target { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("segments")]
        public List<string> Segments { get; set; }
    }
}
