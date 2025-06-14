using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using app_backend.App.Repositories;
using app_backend.App.Models;
using Microsoft.AspNetCore.Authorization;
using app_backend.App.Repositories.IRepositories;
using app_backend.App.Services.IServices;
using System.Security.Claims;

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
            var user = await _userService.GetUserByIdAsync(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
            return Ok(user);
        }
    }
}