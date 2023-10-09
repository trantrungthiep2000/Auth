using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Authen.Cookie.Attributies
{
    public class CookieAuthorizeAttribute : TypeFilterAttribute
    {
        public string RoleName { get; set; }
        public string ActionValue { get; set; }

        public CookieAuthorizeAttribute(string roleName, string actionValue) : base(typeof(CookieAuthorizeFilter))
        {
            RoleName = roleName;
            ActionValue = actionValue;
            Arguments = new object[] { RoleName, ActionValue };
        }
    }

    public class CookieAuthorizeFilter : IAuthorizationFilter
    {
        public string RoleName { get; set; }
        public string ActionValue { get; set; }

        public CookieAuthorizeFilter(string roleName, string actionValue)
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
