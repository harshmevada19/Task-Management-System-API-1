using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ClaimsAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (user.IsInRole("admin"))
            return;

        var requieredClaimType = context.RouteData.Values["controller"]?.ToString();
        var requieredClaimValue = context.RouteData.Values["action"]?.ToString();
        
        // Check if user is authenticated
        if (!user.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // Check for the required claim
        if (!user.HasClaim(c => c.Type == requieredClaimType && c.Value == requieredClaimValue))
        {
            context.Result = new ForbidResult();
            return;
        }
    }
}
