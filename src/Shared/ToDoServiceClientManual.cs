using System.Net.Http;
using System.Threading.Tasks;
using DTO;

namespace Shared
{
    public sealed partial class ToDoServiceClient : BaseClient
    {

        public ToDoServiceClient(IHttpClientFactory factory) : base(Service.ToDo, factory)
        {
            
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