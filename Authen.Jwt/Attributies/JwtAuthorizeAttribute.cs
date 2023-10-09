using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Authen.Jwt.Attributies
{
    public class JwtAuthorizeAttribute : TypeFilterAttribute
    {
        public string RoleName { get; set; }
        public string ActionValue { get; set; }

        public JwtAuthorizeAttribute(string roleName, string actionValue) : base(typeof(JwtAuthorizeFilter))
        {
            RoleName = roleName;
            ActionValue = actionValue;
            Arguments = new object[] { RoleName, ActionValue };
        }
    }

    public class JwtAuthorizeFilter : IAuthorizationFilter
    {
        public string RoleName { get; set; }
        public string ActionValue { get; set; }

        public JwtAuthorizeFilter(string roleName, string actionValue)
        {
            RoleName = roleName;
            ActionValue = actionValue;  
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!CanAccessToAction(context.HttpContext))
                context.Result = new ForbidResult();
        }

        private bool CanAccessToAction(HttpContext httpContext)
        {
            var roles = httpContext.User.FindFirstValue(ClaimTypes.Role);

            if (roles!.Equals(RoleName))
                return true;

            return false;
        }
    }
}
