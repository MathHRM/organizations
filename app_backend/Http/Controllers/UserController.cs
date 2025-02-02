// using Microsoft.AspNetCore.Mvc;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using App.Model;
// using App.Service;

// namespace App.Http.Controllers;

// [ApiController]
// [Route("api/[controller]")]
// public class UserController : ControllerBase
// {
//     private readonly IUserService _userService;

//     public UserController(IUserService userService)
//     {
//         _userService = userService;
//     }

//     [HttpGet]
//     public async Task<ActionResult<IEnumerable<User>>> GetUsers()
//     {
//         var users = await _userService.GetUsersAsync();
//         return Ok(users);
//     }

//     [HttpGet("{id}")]
//     public async Task<ActionResult<User>> GetUser(int id)
//     {
//         var user = await _userService.GetUserByIdAsync(id);
//         if (user == null)
//         {
//             return NotFound();
//         }
//         return Ok(user);
//     }

//     [HttpPost]
//     public async Task<ActionResult<User>> CreateUser(User user)
//     {
//         var createdUser = await _userService.CreateUserAsync(user);
//         return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
//     }

//     [HttpPut("{id}")]
//     public async Task<IActionResult> UpdateUser(int id, User user)
//     {
//         if (id != user.Id)
//         {
//             return BadRequest();
//         }

//         var result = await _userService.UpdateUserAsync(user);
//         if (!result)
//         {
//             return NotFound();
//         }

//         return NoContent();
//     }

//     [HttpDelete("{id}")]
//     public async Task<IActionResult> DeleteUser(int id)
//     {
//         var result = await _userService.DeleteUserAsync(id);
//         if (!result)
//         {
//             return NotFound();
//         }

//         return NoContent();
//     }
// }
