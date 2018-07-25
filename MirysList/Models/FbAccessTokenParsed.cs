using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MirysList.Models
{
    public class FbAccessTokenParsed
    {
        [JsonProperty(PropertyName = "app_id")]
        public string AppId { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "application")]
        public string AppName { get; set; }

        [JsonProperty(PropertyName = "expires_at")]
        public string ExpiresAt { get; set; }

        [JsonProperty(PropertyName = "is_valid")]
        public bool IsValid { get; set; }

        [JsonProperty(PropertyName = "scopes")]
        public List<string> Scopes { get; set; } = new List<string>();

        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }
    }
}
