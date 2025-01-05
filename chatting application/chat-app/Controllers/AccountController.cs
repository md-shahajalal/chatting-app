using AutoMapper;
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
    public class AccountController(DataContext context, ITokenService tokenService, IMapper mapper) : BaseApiController
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            
            if (await UserExists(registerDto.Username)) return BadRequest("User with this username already exist.");

            using var hmac = new HMACSHA512();

            var user = mapper.Map<AppUser>(registerDto);



            user.Username = registerDto.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;

            context.Users.Add(user);
            await context.SaveChangesAsync();
            return new UserDto
            {
                Username = user.Username,
                Token = tokenService.CreateToken(user),
                Gender = user.Gender,
                KnownAs = user.KnownAs
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await context.Users
            .Include(p => p.Photos)
                .FirstOrDefaultAsync(x =>
                    x.Username == loginDto.Username.ToLower());
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
                KnownAs = user.KnownAs,
                Token = tokenService.CreateToken(user),
                Gender = user.Gender,
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await context.Users.AnyAsync(x => x.Username.ToLower() == username.ToLower()); // Bob != bob
        }
    }
}
