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
using app_backend.App.Enums;

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
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
        {
            if (await _userService.UserExistsAsync(request.Email))
            {
                return BadRequest("User with this email already exists");
            }

            var user = new User
            {
                Email = request.Email,
                Password = request.Password,
                Name = request.Name
            };

            var createdUser = await _userService.CreateUserAsync(user);
            var organizationId = user.OrganizationUsers.FirstOrDefault(ou => ou.Role == (int) Role.Owner)?.OrganizationId;

            return Ok(new AuthResponse
            {
                Token = _tokenService.GenerateToken(createdUser, Role.Owner, organizationId),
                User = new UserResponse
                {
                    Id = createdUser.Id,
                    Name = createdUser.Name,
                    Email = createdUser.Email
                }
            });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            var user = await _userService.ValidateUserCredentialsAsync(request.Email, request.Password);
            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }

            // var organizationUser = await _userService.GetOrganizationForUserAsync(user.Id);

            // var token = _tokenService.GenerateToken(user, organizationUser.OrganizationId, (Role) organizationUser.Role);

            return Ok(new AuthResponse
            {
                Token = "",
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