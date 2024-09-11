using FinalDern_Support.Models.Dto.RequestDtos;
using FinalDern_Support.Models;
using FinalDern_Support.Repositories.Interfaces;
using FinalDern_Support.Repositories.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalDern_Support.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _user;
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        private readonly ICustomer _customer;
        private readonly Jwt_TokenServices _jwtTokenService;

        public UserController(IUser user, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ICustomer customer, Jwt_TokenServices jwtTokenService)
        {
            _user = user;
            _signInManager = signInManager;
            _userManager = userManager;
            _customer = customer;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _user.Register(registerDto, ModelState);

            if (user == null)
            {
                return BadRequest(new { Message = "Registration failed. Please check your details and try again." });
            }

            return CreatedAtAction(nameof(Register), new { id = user.Id }, user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _user.Login(loginDto.UserName, loginDto.Password);

            if (user == null)
            {
                return Unauthorized(new { Message = "Invalid username or password." });
            }

            return Ok(user);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _user.Logout();
            return Ok(new { Message = "Logged out successfully" });
        }

    }
}
