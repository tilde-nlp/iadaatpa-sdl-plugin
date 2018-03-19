using Newtonsoft.Json;

namespace iADAATPA.MTProvider.Model
{
    public class TranslationItem
    {
        [JsonProperty("segment")]
        public string Segment { get; set; }

        [JsonProperty("translation")]
        public string Translation { get; set; }
    }
}