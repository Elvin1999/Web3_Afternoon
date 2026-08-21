using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web3_Afternoon.Dtos;
using Web3_Afternoon.Entities;
using Web3_Afternoon.Services.Abstract;

namespace Web3_Afternoon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;

        public AuthController(UserManager<ApplicationUser> userManager,
            IConfiguration configuration, 
            IAuthService authService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            var user = new ApplicationUser
            {
                Fullname = model.Fullname,
                Email = model.Email,
                UserName = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new
            {
                message = "User created successfully"
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var user=await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }

            var passwordValid=await _userManager.CheckPasswordAsync(user,model.Password);
            if (!passwordValid)
            {
                return Unauthorized("Invalid email or password");
            }

            var accessToken=_authService.GenerateToken(user);

            return Ok(new
            {
                accessToken
            });
        }

    }
}
