using Microsoft.AspNetCore.Mvc;
using Phorum.Models;
using Phorum.Services;

namespace Phorum.Controllers
{
    [ApiController]
    [Route("api/phorum/account")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("register")]
        public ActionResult Register([FromBody] RegisterUserDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _userService.RegisterUser(model);
            return Ok();
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] UserLoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var login = _userService.Authenticate(model);
            return Ok(login);
        }

        [HttpPost("refresh-token")]
        public ActionResult RefreshToken()
        {
            if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                if (authorizationHeader.ToString().StartsWith("Bearer "))
                {
                    var refreshToken = authorizationHeader.ToString().Substring("Bearer ".Length);

                    var newTokens = _userService.RefreshToken(refreshToken);

                    return Ok(newTokens);
                }
            }
            return BadRequest();
            

        }
    }
}