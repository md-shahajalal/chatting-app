using chat_app.Data;
using chat_app.DTOs;
using chat_app.Entities;
using chat_app.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace chat_app.Controllers
{
    public class UsersController(DataContext context, IUserRepository userRepository): BaseApiController
    {
        [AllowAnonymous]
        [HttpGet] //api/users
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers() {
            var users = await userRepository.GetMembersAsync();
            return Ok(users);
        }

        [Authorize]
        [HttpGet("{username:string}")] //api/users/2
        public async Task<ActionResult<AppUser>> GetUser(string username)
        {
            var user = await userRepository.GetMemberAsync(username);
            if (user == null) return NotFound();
            return Ok(user);
        }
    }
}
