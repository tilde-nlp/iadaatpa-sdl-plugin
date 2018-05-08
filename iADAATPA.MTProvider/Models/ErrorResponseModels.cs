using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iADAATPA.MTProvider.Models
{
    public class ResponseError
    {
        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("timestamp")]
        public int Timestamp { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class GenericResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error")]
        public ResponseError Error { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }
    }
}
