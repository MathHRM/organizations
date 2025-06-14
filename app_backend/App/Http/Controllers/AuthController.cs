using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using app_backend.App.Repositories;
using app_backend.App.Models;
using Microsoft.AspNetCore.Authorization;
using app_backend.App.Repositories.IRepositories;
using app_backend.App.Services;
using app_backend.App.Services.IServices;
using app_backend.App.Http.Requests;
using app_backend.App.Http.Responses;

namespace app_backend.App.Http.Controllers
{
    [ApiController]
    [Route("api/Auth")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly IUserService _userService;

        public AuthController(TokenService tokenService, IUserService userService)
        {
            _tokenService = tokenService;
            _userService = userService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest request)
        {
            // Check if user already exists
            if (await _userService.UserExistsAsync(request.Email))
            {
                return BadRequest("User with this email already exists");
            }

            var user = new User
            {
                Email = request.Email,
                Password = request.Password, // UserService will handle password hashing
                Name = request.Name
            };

            var createdUser = await _userService.CreateUserAsync(user);

            return Ok(new RegisterResponse
            {
                Token = _tokenService.GenerateToken(createdUser.Id, createdUser.Email),
            });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            var user = await _userService.ValidateUserCredentialsAsync(request.Email, request.Password);
            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }

            var token = _tokenService.GenerateToken(user.Id, user.Email);

            return Ok(new LoginResponse
            {
                Token = token,
                User = new UserResponse
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email
                }
            });
        }
    }
}