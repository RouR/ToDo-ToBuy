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
            _client = new HttpClient
            {
                BaseAddress = new Uri(ServiceClients.Url(Service.Account))
            };
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> TestCall(string data)
        {
            var response = await _client.PostAsJsonAsync("/Test1", data);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<string>();
            return result;
        }

        public async Task<string> TestDelay(string data)
        {
            var response = await _client.PostAsJsonAsync("/TestDelay", data);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<string>();
            return result;
        }

    }
}