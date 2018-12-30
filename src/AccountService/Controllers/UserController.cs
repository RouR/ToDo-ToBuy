using AccountService.Interfaces;
using DTO.Internal.Account;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;
using Shared;
using Utils;

namespace AccountService.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [GlobalValidator]
    public class UserController : Controller
    {
        private readonly ITracer _tracer;
        private readonly IUserService _userService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tracer"></param>
        /// <param name="userService"></param>
        public UserController(ITracer tracer,
            IUserService userService)
        {
            _tracer = tracer;
            _userService = userService;
        }


        /// <summary>
        /// Register new User
        /// UserId must be pregenerated
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Errorable Guid UserId</returns>
        [HttpPost]
        public CreateUserResponse Register([FromBody] CreateUserRequest model)
        {
            return _userService.Register(model);
        }

        /// <summary>
        /// Try Login, with ban check
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public TryLoginResponse TryLogin([FromBody] TryLoginRequest model)
        {
            return _userService.Login(model);
        }
    }
}