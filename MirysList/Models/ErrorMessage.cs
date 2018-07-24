using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MirysList.Models
{
    public class ErrorMessage
    {
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        public ErrorMessage(string message)
        {
            this.Message = message;
        }
    }
}
