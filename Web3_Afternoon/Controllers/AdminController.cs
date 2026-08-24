using Microsoft.AspNetCore.Authorization;
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
    public class AdminController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _tokenService;

        public AdminController(UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            IAuthService tokenService,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _tokenService = tokenService;
            _roleManager = roleManager;
        }


        [HttpPost("register-manager")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            var user = new ApplicationUser
            {
                Fullname = model.Fullname,
                UserName = model.Email,
                Email = model.Email,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            var manager = await _userManager.FindByEmailAsync(model.Email);
            if (manager == null)
            {
                return BadRequest("Manager can not created");
            }
            await _userManager.AddToRoleAsync(manager, "Manager");


            return Ok(new
            {
                message = "Manager registered successfully"
            });

        }


    }
}
