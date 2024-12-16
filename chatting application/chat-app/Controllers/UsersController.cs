using chat_app.Data;
using chat_app.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace chat_app.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(DataContext context): ControllerBase
    {
        [HttpGet] //api/users
        public async Task<ActionResult<IEnumerable<AppUser>>> Getusers() { 
            var users =  await context.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id:int}")] //api/users/2
        public async Task<ActionResult<AppUser>> Getuser(int id)
        {
            var user = await context.Users.FindAsync(id);
            if (user == null) return BadRequest("No user with given id.");
            return Ok(user);
        }
    }
}
