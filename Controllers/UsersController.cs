using Microsoft.AspNetCore.Mvc;
using back.Models;
using back.Services;

namespace back.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromForm]AuthenticateRequest model)
        {
            var response = await _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var cookieOption = new CookieOptions { HttpOnly = true, Expires = DateTime.UtcNow.AddDays(7) };
            Response.Cookies.Append("Authorization", response.Token,cookieOption);
            return Ok(response);
        }
        [HttpPost("registration")]
        public async Task<IActionResult> Registration(AuthenticateRequest model)
        {
            var response = await _userService.Registation(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

    }
}