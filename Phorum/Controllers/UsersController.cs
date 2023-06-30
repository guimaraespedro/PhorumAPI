
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phorum.AppConstants;
using Phorum.Entities;
using Phorum.Identity;
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
       
    }
}
