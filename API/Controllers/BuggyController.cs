using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    
    public class BuggyController : BaseApiController
    {
        private readonly DataContext _Context;
        public BuggyController(DataContext context)
        {
            _Context = context;
        }
        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecrets()
        {
            return "Secret Text";
        }

        [HttpGet("not-found")]
        public ActionResult<string> GetNotFound()
        {
            var thing = _Context.AppUsers.Find(-1);
            if(thing == null)
            {
                return NotFound();
            }
            return Ok(thing);
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            var thingToReturn = _Context.AppUsers.Find(-1);
            return thingToReturn.ToString();
        }
        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
             return BadRequest("This is not good Request");
        }
        



    }
}