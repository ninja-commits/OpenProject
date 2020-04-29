﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiJwt.TokenAuth;

namespace WebApiJwt.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        [Route("~/api/Values/Post")]
        public void Post([FromBody]string value)
        {

        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        [HttpPost]
        public IHttpActionResult ValidLogin(string userName, string userPassword)
        {
            if (userName == "admin")
            {
                return Ok(TokenManager.GetToken());
            }
            else
                return BadRequest("userName password Invalid");
        }

        [HttpPost]
        public IHttpActionResult GetToken()
        {
            return Ok(TokenManager.GetToken());
        }

        [TokenAuthFilter]
        [HttpPost]
        public IHttpActionResult ValideToken()
        {
            return Ok();
        }
    }
}
