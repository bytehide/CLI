using Newtonsoft.Json;

namespace Dotnetsafer.CLI.Models
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
