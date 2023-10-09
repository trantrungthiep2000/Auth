using Authen.Jwt.Models.RequestModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Authen.Jwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthensController : ControllerBase
    {
        private readonly JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        [HttpGet]
        [Route("GetUnauthorized")]
        public IActionResult GetUnauthorized()
        {
            return Unauthorized();
        }

        [HttpGet]
        [Route("GetUnauthorizedV2")]
        public IActionResult GetUnauthorizedV2()
        {
            return Unauthorized();
        }

        [HttpGet]
        [Route("GetForbidden")]
        public IActionResult GetForbidden()
        {
            return StatusCode(Convert.ToInt32(HttpStatusCode.Forbidden));
        }

        [HttpPost]
        [Route("LoginJwt")]
        public IActionResult LoginJwt([FromBody] User user)
        {
            // check user
            // get role

            string roleName = string.Empty;

            if (user.UserName.EndsWith("Administrator"))
                roleName = "Administrator";
            else if (user.UserName.EndsWith("Admin"))
                roleName = "Admin";
            else
                roleName = "Guest";

            return Ok(GetJwtToken(user, roleName));
        }

        private string GetJwtToken(User user, string roleName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, roleName),
                new Claim("FullName", "Thiep Tran Trung"),
            };

            var identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);

            var token = CreateSecurityToken(identity);
            return WriteToken(token);
        }

        private SecurityToken CreateSecurityToken(ClaimsIdentity identity)
        {
            var tokenDescriptor = this.GetTokenDescriptor(identity);

            return this.tokenHandler.CreateToken(tokenDescriptor);
        }

        private SecurityTokenDescriptor GetTokenDescriptor(ClaimsIdentity identity)
        {
            return new SecurityTokenDescriptor()
            {
                Subject = identity,
                Expires = DateTime.UtcNow.AddDays(1),
                Audience = "SwaggerUI",
                Issuer = "Jwt",
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes("f5422e6cdfde4af3bf631c7dd1f80b97")),
                    SecurityAlgorithms.HmacSha256Signature),
            };
        }

        private string WriteToken(SecurityToken token)
        {
            return this.tokenHandler.WriteToken(token);
        }
    }
}