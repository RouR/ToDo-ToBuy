using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DTO;
using Newtonsoft.Json;

namespace Shared
{
    public sealed partial class ToDoServiceClient
    {
        private readonly HttpClient _client;

        public ToDoServiceClient()
        {
            var url = ServiceClients.Url(Service.ToDo);
            Console.WriteLine($"AccountServiceClient Url {url}");
            var uri = new Uri(url);
            Console.WriteLine($"AccountServiceClient Uri {uri}");
            _client = new HttpClient
            {
                BaseAddress = uri
            };
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> TestCall(string data)
        {
            var response = await _client.PostAsJsonAsync("/Test/Test1?request=" + data, new { });
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<string>();
            return result;
        }

        public async Task<string> TestDelay(string data)
        {
            var response = await _client.PostAsJsonAsync("/Test/TestDelay?request=" + data, new { });
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<string>();
            return result;
        }

        public async Task<string> TestFail(string data)
        {
            var response = await _client.PostAsJsonAsync("/Test/TestFail?request=" + data, new { });
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<string>();
            return result;
        }
    }
}