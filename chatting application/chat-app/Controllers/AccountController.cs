using chat_app.Data;
using chat_app.DTOs;
using chat_app.Entities;
using chat_app.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace chat_app.Controllers
{
    public class AccountController(DataContext context, ITokenService tokenService) : BaseApiController
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Login(RegisterDto registerDto)
        {
            
            if (await UserExists(registerDto.Username)) return BadRequest("User with this username already exist.");

            var hmac = new HMACSHA512();

            var user = new AppUser
            {
                Username = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            context.Users.Add(user);

            await context.SaveChangesAsync();
            return new UserDto
            {
                Username = registerDto.Username,
                Token = tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await context.Users.FirstOrDefaultAsync(x=>x.Username.ToLower() == loginDto.Username.ToLower());
            if (user == null) return Unauthorized("No user exist with this username.");
            var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for(int i=0;i< user.PasswordHash.Length;i++)
            {
                if (user.PasswordHash[i] != computedHash[i]) return Unauthorized("Password didn't match.");
            }

            return new UserDto
            {
                Username =  user.Username,
                Token = tokenService.CreateToken(user)
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await context.Users.AnyAsync(x => x.Username.ToLower() == username.ToLower()); // Bob != bob
        }
    }
}
