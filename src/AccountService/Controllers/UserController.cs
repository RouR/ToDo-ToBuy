using System.Threading.Tasks;
using DTO.Internal.Account;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;

namespace AccountService.Controllers
{
    public class UserController : Controller
    {
        private readonly ITracer _tracer;

        public UserController(ITracer tracer)
        {
            _tracer = tracer;
        }


        [HttpPost]
        public async Task<CreateUserResponse> Register([FromBody] CreateUserRequest model)
        {
            await Task.Delay(0);
            return new CreateUserResponse
            {
                HasError = true,
                Message = "Not Implemented"
            };
        }
    }
}