using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
       // private readonly DataContext _datacontext;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _Mapper;
       

        // public UsersController(DataContext datacontext)
        // {
        //     _datacontext = datacontext;

        // }
        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _Mapper = mapper;
        }
        [HttpGet]
        // [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await _userRepository.GetMembersAsync();
            return Ok(users);
           // return await _datacontext.AppUsers.ToListAsync();
        //    var users = await _userRepository.GetUserAsync();
        //    var userToReturn = _Mapper.Map<IEnumerable<MemberDto>>(users);
        //    return Ok(userToReturn);
        }
        // [Authorize]
        [HttpGet("{userName}")]
        public async Task<ActionResult<MemberDto>> GetUser(string userName)
        {
            //return await _datacontext.AppUsers.FindAsync(id);
            return await _userRepository.GetMemberAsync(userName);
            // var user = await _userRepository.GetUserByUsernameAsync(userName);
            // return _Mapper.Map<MemberDto>(user);
        }


        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepository.GetUserByUsernameAsync(username);

            _Mapper.Map(memberUpdateDto, user);

            _userRepository.Update(user);

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update user");

            // var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            // _mapper.Map(memberUpdateDto, user);

            // _unitOfWork.UserRepository.Update(user);

            // if (await _unitOfWork.Complete()) return NoContent();

            // return BadRequest("Failed to update user");
        }
    }
}