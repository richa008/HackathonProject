using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MirysList.Models
{
    public class FbRequestPayload
    {
        [JsonProperty(PropertyName = "algorithm")]
        public string Algorithm { get; set; }

        [JsonProperty(PropertyName = "code")]
        public string AuthCode { get; set; }

        [JsonProperty(PropertyName = "issued_at")]
        public string IssuedAt { get; set; }

        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }
    }
}
