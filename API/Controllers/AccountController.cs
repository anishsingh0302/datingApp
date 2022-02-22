using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _dataContext;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext dataContext, ITokenService tokenService)
        {
            _dataContext = dataContext;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> register(RegisterDto registerDto)
        {
           if(await UserExists(registerDto.Username)) return BadRequest("UserName is taken");

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDto.Username, 
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                passwordSalt = hmac.Key
            };
            _dataContext.AppUsers.Add(user);

            await _dataContext.SaveChangesAsync();

            return new UserDto
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
            
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
           var Users = await _dataContext.AppUsers.SingleOrDefaultAsync(x => x.UserName == loginDto.UserName );

           if(Users ==  null)
           {
               return Unauthorized("Invalid UserName");
           }

           using var hmac = new HMACSHA512(Users.passwordSalt);

           var ComputeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

           for(int i=0; i < ComputeHash.Length; i++)
           {
               if(ComputeHash[i] != Users.passwordHash[i] )
                   return Unauthorized("InValid Password");
           }

           return new UserDto
            {
                UserName = Users.UserName,
                Token = _tokenService.CreateToken(Users)
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await _dataContext.AppUsers.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}