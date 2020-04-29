using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CalculatorService.Controllers
{
    /// <summary>
    /// Main Values Controller
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// Compute the result of a + b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>a + b</returns>
        [HttpGet]
        public IActionResult Calculate(int a, int b)
        {
            return Ok(a + b);
        }
        /// <summary>
        /// multiply a by b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Multipl(int a, int b)
        {
            return Ok(a * b);
        }

        [HttpGet]
        public IActionResult NewMethod()
        {
            return Ok("sdkfmlskf");
        }

        /// <summary>
        /// Get Values
        /// </summary>
        /// <returns></returns>
        // GET: api/Values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Values/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
