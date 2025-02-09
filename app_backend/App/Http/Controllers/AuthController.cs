using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using app_backend.App.Repositories;
using app_backend.App.Models;
using Microsoft.AspNetCore.Authorization;
using app_backend.App.Repositories.IRepositories;
using app_backend.App.Services;
using app_backend.App.Http.Requests;
using app_backend.App.Http.Responses;

namespace app_backend.App.Http.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;

        private readonly IUserRepository _userService;

        public AuthController(TokenService tokenService, IUserRepository userService)
        {
            _tokenService = tokenService;
            _userService = userService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest request)
        {
            var user = new User
            {
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Name = request.Name
            };

            var createdUser = await _userService.AddUserAsync(user);

            _tokenService.GenerateToken(createdUser.Id, createdUser.Email);
            
            return Ok(new RegisterResponse
            {
                Token = _tokenService.GenerateToken(createdUser.Id, createdUser.Email),
            });
        }
    }
}