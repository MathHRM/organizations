using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using app_backend.App.Repositories;
using app_backend.App.Models;
using Microsoft.AspNetCore.Authorization;
using app_backend.App.Repositories.IRepositories;
using app_backend.App.Services.IServices;
using System.Security.Claims;
using app_backend.App.Http.Responses;
using app_backend.App.Enums;

namespace app_backend.App.Http.Controllers
{
    [ApiController]
    [Route("api/Users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        [Route("Me")]
        public async Task<IActionResult> Me()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _userService.GetUserWithOrganizationsAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            Console.WriteLine((User.FindFirst(ClaimTypes.Role)?.Value));

            return Ok(new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Organization = new OrganizationResponse
                {
                    Id = int.Parse(User.FindFirst(JwtClaim.OrganizationId.ToString())?.Value),
                    Name = User.FindFirst(JwtClaim.OrganizationName.ToString())?.Value,
                    Role = (Role) int.Parse(User.FindFirst(ClaimTypes.Role)?.Value)
                }
            });
        }
    }
}