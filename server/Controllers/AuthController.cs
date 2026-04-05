using IMDB.Models.Request;
using IMDB.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace IMDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("signin")]
        public IActionResult SignIn([FromBody] SignInRequest userDetails)
        {
            try
            {
                var result = _authService.SignIn(userDetails);
                if (result)
                {
                    return Ok(new { isRegistered = result });
                }
                return BadRequest(new { isRegistered = result, message = "Registration failed" });
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }

        [HttpPost("login")]
        public IActionResult SignUp([FromBody] LogInRequest userDetail)
        {
            try
            {
                var result = _authService.LogIn(userDetail);
                if (result == null)
                {
                    return Unauthorized(new { message = "Invalid Credentials" });
                }
                return Ok(new { token = result });
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

    }
}
