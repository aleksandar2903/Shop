using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShop.Models
{
    public class AuthResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
