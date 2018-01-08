using System;
using Newtonsoft.Json;

namespace DockerHubChecker
{
    public sealed class Repository
    {
        [JsonProperty("user")]
        public string User { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("repository_type")]
        public string RepositoryType { get; set; }

        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("is_private")]
        public bool IsPrivate { get; set; }

        [JsonProperty("is_automated")]
        public bool IsAutomated { get; set; }

        [JsonProperty("can_edit")]
        public bool CanEdit { get; set; }

        [JsonProperty("star_count")]
        public long StarCount { get; set; }

        [JsonProperty("pull_count")]
        public long PullCount { get; set; }

        [JsonProperty("last_updated")]
        public DateTime LastUpdated { get; set; }
    }
}