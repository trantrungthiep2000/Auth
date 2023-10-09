using Authen.Cookie.Models.RequestModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Authen.Cookie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthensController : ControllerBase
    {
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
        [Route("LoginCookie")]
        public async Task<IActionResult> LoginCookie([FromBody] User user)
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

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, roleName),
                new Claim("FullName", "Thiep Tran Trung"),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal =new ClaimsPrincipal(identity);

            var authenProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1),
                IsPersistent = true,
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authenProperties);

            return Ok("Login success with cookies");
        }

        [HttpGet]
        [Route("GetLogout")]
        public async Task<IActionResult> GetLogout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Ok("Logout success with cookies");
        }
    }
}
