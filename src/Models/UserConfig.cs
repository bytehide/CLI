using Newtonsoft.Json;

namespace ShieldCLI.Models
{
    public class UserConfig
    {
        [JsonProperty]
        public string ApiKey { get; set; }
        [JsonProperty]
        public string Edition { get; set; }
        [JsonProperty]
        public string Username { get; set; }
    }
}
