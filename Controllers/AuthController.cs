using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SmartStockAI.Dtos.Account;
using SmartStockAI.Interfaces;
using SmartStockAI.models;

namespace SmartStockAI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        private readonly SignInManager<AppUser> _signInManager;

        public AuthController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(ReqLoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return Unauthorized("Invalid Email or Password");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized("Invalid Email or Password");

            var token = _tokenService.CreateToken(user);

            return Ok(new ResLoginDto
            {
                UserId = user.Id,
                Email = user.Email ?? "Unkown",
                Role = user.Role.ToString(),
                Token = token,
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(ReqRegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var validRoles = new[] { "Admin", "Staff" };
                if (!validRoles.Contains(registerDto.Role))
                    return BadRequest(new { message = "Invalid role. Allowed roles: Admin, Staff" });

                var appUser = new AppUser
                {
                    FullName = registerDto.FullName,
                    Email = registerDto.Email,
                    UserName = registerDto.Email,
                    Role = Enum.Parse<UserRole>(registerDto.Role)
                };

                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

                if (createdUser.Succeeded)
                {
                    return Ok(new ResRegisterDto
                    {
                        Id = appUser.Id,
                        Email = appUser.Email,
                        FullName = appUser.FullName,
                        Role = appUser.Role.ToString(),
                    });
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }

}