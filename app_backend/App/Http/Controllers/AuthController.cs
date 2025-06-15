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
using System.Security.Claims;

namespace app_backend.App.Http.Controllers
{
    [ApiController]
    [Route("api/Auth")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly UserService _userService;

        public AuthController(TokenService tokenService, UserService userService)
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
            var organization = _userService.GetUserOwnedOrganization(createdUser);
            var organizationUser = GetOrganizationUser(createdUser, organization.Id);

            return Ok(new AuthResponse
            {
                Token = _tokenService.GenerateToken(createdUser, Role.Owner, organization.Id, organization.Name),
                User = new UserResponse
                {
                    Id = createdUser.Id,
                    Name = createdUser.Name,
                    Email = createdUser.Email,
                    Organization = new OrganizationResponse
                    {
                        Id = organization.Id,
                        Name = organization.Name,
                        Role = (Role) organizationUser.Role
                    }
                }
            });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            var user = await _userService.ValidateUserCredentialsAsync(request.Email, request.Password, request.OrganizationId);
            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }

            var organizationUser = GetOrganizationUser(user, request.OrganizationId);
            var token = _tokenService.GenerateToken(user, (Role) organizationUser.Role, organizationUser.OrganizationId, organizationUser.Organization.Name);

            return Ok(new AuthResponse
            {
                Token = token,
                User = new UserResponse
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Organization = new OrganizationResponse
                    {
                        Id = organizationUser.OrganizationId,
                        Name = organizationUser.Organization.Name,
                        Role = (Role) organizationUser.Role
                    }
                }
            });
        }

        [HttpPost]
        [Route("EnterOrganization")]
        [Authorize]
        public async Task<ActionResult<AuthResponse>> EnterOrganization([FromBody] EnterOrganizationRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _userService.ValidateUserCredentialsAsync(userId, request.OrganizationId);

            if (user == null)
            {
                return Unauthorized("Invalid user");
            }

            var organizationUser = GetOrganizationUser(user, request.OrganizationId);
            var token = _tokenService.GenerateToken(user, (Role) organizationUser.Role, organizationUser.OrganizationId, organizationUser.Organization.Name);

            return Ok(new AuthResponse
            {
                Token = token,
                User = new UserResponse
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Organization = new OrganizationResponse
                    {
                        Id = organizationUser.OrganizationId,
                        Name = organizationUser.Organization.Name,
                        Role = (Role) organizationUser.Role
                    }
                }
            });
        }

        private OrganizationUser? GetOrganizationUser(User user, int? organizationId)
        {
            if (organizationId == null)
            {
                return user.OrganizationUsers.FirstOrDefault(ou => ou.Role == (int) Role.Owner);
            }

            return user.OrganizationUsers.FirstOrDefault(ou => ou.OrganizationId == organizationId);
        }
    }
}