using Newtonsoft.Json;

namespace DockerHubChecker
{
    public sealed class TagImage
    {
        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("architecture")]
        public string Architecture { get; set; }

        [JsonProperty("variant")]
        public object Variant { get; set; }

        [JsonProperty("features")]
        public object Features { get; set; }

        [JsonProperty("os")]
        public string Os { get; set; }

        [JsonProperty("os_version")]
        public object OsVersion { get; set; }

        [JsonProperty("os_features")]
        public object OsFeatures { get; set; }
    }
}