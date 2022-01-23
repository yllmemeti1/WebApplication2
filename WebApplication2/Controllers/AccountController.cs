using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication2.Common.Entities;
using WebApplication2.Common.Models;
using WebApplication2.Persistence;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    public class AccountController: BaseController
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly DataContext _context;
        private readonly TokenService _tokenService;

        public AccountController(DataContext context,
              UserManager<User> userManager,
              SignInManager<User> signInManager,
              TokenService tokenService)
        {
            this._context = context;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                return ValidationProblem("Passwords must match", statusCode: 401);

            }
            if (await _context.Users.FirstOrDefaultAsync(u => u.Email == registerDto.Email) != null)
            {
                return ValidationProblem("Email must be unique", statusCode: 401);
            }

            if (await _context.Users.FirstOrDefaultAsync(u => u.UserName == registerDto.UserName) != null)
            {
                return ValidationProblem("Username must be unique", statusCode: 401);
            }

            var user = new User
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            var loggedInUser = await _userManager.FindByEmailAsync(user.Email);

            if (result.Succeeded)
            {
                return new UserDto
                {
                    Token = _tokenService.CreateToken(user),
                    Username = loggedInUser.UserName,
                    Id = loggedInUser.Id,
                    Email = loggedInUser.Email,
                };
            }

            return BadRequest();
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized();

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (result.Succeeded)
            {
                return new UserDto
                {
                    Token = _tokenService.CreateToken(user),
                    Username = user.UserName,
                    Id = user.Id,
                    Email = user.Email,
                };
            }

            return Unauthorized();
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Append("sms_token", "novalue");

            return Ok();
        }

        [Authorize]
        [HttpGet("currentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));

            return new UserDto
            {
                Token = _tokenService.CreateToken(user),
                Username = user.UserName,
                Id = user.Id,
                Email = user.Email,
            };
        }

        [Authorize]
        [HttpGet("authorize")]
        public IActionResult Authorize()
        {
            return Ok();
        }
    }
}
