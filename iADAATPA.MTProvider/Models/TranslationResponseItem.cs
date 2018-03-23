using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iADAATPA.MTProvider.Models
{
    class TranslationResponseItem
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error")]
        public object Error { get; set; }

        [JsonProperty("data")]
        public SegmentContainer Data { get; set; }
    }

    public class SegmentContainer
    {
        [JsonProperty("segments")]
        public Dictionary<string, TranslationItem> Segments { get; set; }
    }
}
