using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Threading.Tasks;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService<User> _authService;

        public AuthenticationController(IAuthService<User> authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Authenticates user.
        /// </summary>
        /// <param name="username">User username.</param>
        /// <returns>Returns Http response result.</returns>
        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> AuthenticateUser(string username)
        {
            var user = await _authService.Authenticate(username);

            if (user == null)
                return Unauthorized("User is not found.");

            return Ok(user);
        }
    }
}