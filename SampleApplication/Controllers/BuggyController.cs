using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleApplication.Database;
using SampleApplication.Entities;

namespace SampleApplication.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly DatabaseContext context;

        public BuggyController(DatabaseContext context)
        {
            this.context = context;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }

        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var thing = context.Users.Find(-1);

            if (thing != null)
            {
                return Ok(thing);
            }

            return NotFound();
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            var thing = context.Users.Find(-1);

            var thingToReturn = thing.ToString();

            return thingToReturn;
        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("This was not a good request");
        }
    }
}