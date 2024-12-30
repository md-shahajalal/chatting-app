using AutoMapper;
using chat_app.Data;
using chat_app.DTOs;
using chat_app.Entities;
using chat_app.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace chat_app.Controllers
{
    public class UsersController(DataContext context, IUserRepository userRepository, IMapper mapper) : BaseApiController
    {
        [AllowAnonymous]
        [HttpGet] //api/users
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers() {
            var users = await userRepository.GetMembersAsync();
            return Ok(users);
        }

        [Authorize]
        [HttpGet("{username}")] //api/users/2
        public async Task<ActionResult<AppUser>> GetUser(string username)
        {
            var user = await userRepository.GetMemberAsync(username);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (username == null) return BadRequest("No username found in token");
            var user = await userRepository.GetUserByUsernameAsync(username);
            if (user == null) return BadRequest("Could not find user");
            mapper.Map(memberUpdateDto, user);
            if (await userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to update the user");
        }
    }
}
