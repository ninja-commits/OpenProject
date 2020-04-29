using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CalculatorService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class TokenController : ControllerBase
    {
        public string SecretKey { get; set; } = "98DAD3276CADE1F27204455467F20BEB11F2E98911A91399A5D98A4F9939FC93";
        // GET: api/Token
        [HttpGet]
        public IActionResult Get()
        {
            byte[] key = Convert.FromBase64String(SecretKey);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, "test"),
                    new Claim(ClaimTypes.Email, "test@test.com")

                }),
                Audience = "52b8fa2a-660e-4b7d-a799-b9cea7b3466a",
                Issuer = "788e6e97-6209-4ebc-aaa9-7f01321fe90a",
                Expires = DateTime.UtcNow.AddMinutes(120),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);

            return Ok(handler.WriteToken(token));
        }
    }
}
