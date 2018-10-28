using System.Threading.Tasks;
using DTO.Internal.Account;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;

namespace AccountService.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class UserController : Controller
    {
        private readonly ITracer _tracer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tracer"></param>
        public UserController(ITracer tracer)
        {
            _tracer = tracer;
        }


        /// <summary>
        /// Register new User
        /// UserId must be pregenerated
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Erroable Guid UserId</returns>
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