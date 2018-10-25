using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Shared
{
    public class AccountServiceClient
    {
        private HttpClient _client;

        public AccountServiceClient()
        {
            var url = ServiceClients.Url(Service.Account);
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
            var response = await _client.PostAsJsonAsync("/Test/Test1", data);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<string>();
            return result;
        }

        public async Task<string> TestDelay(string data)
        {
            var response = await _client.PostAsJsonAsync("/Test/TestDelay", data);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<string>();
            return result;
        }

    }
}