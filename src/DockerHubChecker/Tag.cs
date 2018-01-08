using System;
using Newtonsoft.Json;

namespace DockerHubChecker
{
    public sealed class Tag
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("full_size")]
        public long FullSize { get; set; }

        [JsonProperty("images")]
        public TagImage[] TagImages { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("repository")]
        public long Repository { get; set; }

        [JsonProperty("creator")]
        public long Creator { get; set; }

        [JsonProperty("last_updater")]
        public long LastUpdater { get; set; }

        [JsonProperty("last_updated")]
        public DateTime LastUpdated { get; set; }

        [JsonProperty("image_id")]
        public object ImageId { get; set; }

        [JsonProperty("v2")]
        public bool V2 { get; set; }
    }
}