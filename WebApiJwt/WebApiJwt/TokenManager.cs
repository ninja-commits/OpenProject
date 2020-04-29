using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace WebApiJwt
{
    public class TokenManager
    {
        private static string SecretKey = "D4362BD66BFBA629444C5EE891FED312BAF144EB323053C65B9E36B5365B57EF";
        public const string Iss = "Sofia";
        public const string CodeCrgm = "321564";

        public static string GetToken()
        {
            byte[] key = Convert.FromBase64String(SecretKey);

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iss, Iss),   
                new Claim(JwtRegisteredClaimNames.Sub, CodeCrgm),
                new Claim(JwtRegisteredClaimNames.NameId, CodeCrgm)
            };

            var jwt = new JwtSecurityToken(
                    issuer: Iss,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256));

            var handler = new JwtSecurityTokenHandler();
            string result = handler.WriteToken(jwt);
            return result;
        }

        public static ClaimsPrincipal GetSecurityToken(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(token);

                if (jwtToken == null)
                    return null;

                byte[] key = Convert.FromBase64String(SecretKey);

                //it's the validation parameters set the values that you expect
                TokenValidationParameters parameters = new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidIssuer = Iss,
                };
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out SecurityToken securityToken);

                if (DateTime.Now > securityToken.ValidTo)
                    return null;

                return principal;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(token);

                if (jwtToken == null)
                    return null;

                byte[] key = Convert.FromBase64String(SecretKey);

                //it's the validation parameters set the values that you expect
                TokenValidationParameters parameters = new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidIssuer = Iss,
                };
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out SecurityToken securityToken);

                if (DateTime.Now > securityToken.ValidTo)
                    return null;

                return principal;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool CheckClaimPrincipal(string token)
        {
            ClaimsPrincipal principal = GetPrincipal(token);

            if (principal != null)
                return true;
            return false;
        }

        public static SecurityToken ValidateToken(string token)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(token);

            if (jwtToken == null)
                return null;

            byte[] key = Convert.FromBase64String(SecretKey);

            //it's the validation parameters set the values that you expect
            TokenValidationParameters parameters = new TokenValidationParameters
            {
                RequireExpirationTime = true,
                ValidateIssuer = true,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidIssuer = Iss,
            };
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out SecurityToken securityToken);

            if (principal == null)
                return null;

            return securityToken;
        }
    }
}