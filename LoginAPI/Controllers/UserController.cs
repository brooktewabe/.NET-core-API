using LoginAPI.EnumTypes;
using LoginAPI.Models;
using LoginAPI.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace LoginAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public UserController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("Create")]
        public IActionResult Createuser(User user)
        {
            var res = _userService.CreateUser(user);
            return res.Status == (byte)Status.Success ? Ok(res) : BadRequest(res);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult LoginUser(Login credentials)
        {
            var res = _userService.loginUser(credentials);
            return res.Status == (byte)Status.Success ? Ok(res) : BadRequest(res);
        }

        [AllowAnonymous]
        [HttpPost("ForgetPassword")]
        public IActionResult ForgetPassword(ForgetPassword forgetPassword)
        {
            var res = _userService.ForgetPassword(forgetPassword);
            return res.Status == (byte)Status.Success ? Ok(res) : BadRequest(res);
        }

        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword(ResetPassword resetPassword)
        {
            var res = _userService.ResetPassword(resetPassword);
            return res.Status == (byte)Status.Success ? Ok(res) : BadRequest(res);
        }
    }
}
