﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace DockerHubChecker
{
    public class DockerHubClient
    {
        //https://registry.hub.docker.com/v2/roured/tdtb-web/tags/list
        private const string HttpsRegistryHubDockerComV2 = "https://hub.docker.com/v2/";
      
        public IEnumerable<Repository> GetRepos(string username)
        {
            var client = new RestClient(HttpsRegistryHubDockerComV2);

            var request = new RestRequest($"/repositories/{username}/", Method.GET);
            //request.AddHeader("Authorization", $"JWT {token}");

            var response = client.Execute(request);
            var content = response.Content;

            return JsonConvert.DeserializeObject<RepoListResponse>(content).Results;
        }

        public async Task<IEnumerable<Tag>> GetTags(string username, string repoName)
        {
            Console.WriteLine($"docker start load for {username}/{repoName}");
            var client = new RestClient(HttpsRegistryHubDockerComV2);
            var cancellationTokenSource = new CancellationTokenSource();
            
            var request = new RestRequest($"/repositories/{username}/{repoName}/tags/", Method.GET);
            //request.AddHeader("Authorization",$"JWT {token}");

            var response = await client.ExecuteTaskAsync(request, cancellationTokenSource.Token);
            var content = response.Content;

            Console.WriteLine($"docker finish load for {username}/{repoName} = {content}");
            return JsonConvert.DeserializeObject<TagListResponse>(content).Results;
        }

        private sealed class AuthResponse
        {
            [JsonProperty("token")]
            public string Token { get; set; }
        }

        private sealed class RepoListResponse
        {
            [JsonProperty("count")]
            public long Count { get; set; }

            [JsonProperty("next")]
            public object Next { get; set; }

            [JsonProperty("previous")]
            public object Previous { get; set; }

            [JsonProperty("results")]
            public Repository[] Results { get; set; }
        }


        private sealed class TagListResponse
        {
            [JsonProperty("count")]
            public long Count { get; set; }

            [JsonProperty("next")]
            public object Next { get; set; }

            [JsonProperty("previous")]
            public object Previous { get; set; }

            [JsonProperty("results")]
            public Tag[] Results { get; set; }
        }


    }
}
