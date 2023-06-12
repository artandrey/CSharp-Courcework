using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Services;

namespace CustomAttributes
{
    public class ValidateTokenAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var jwtService = context.HttpContext.RequestServices.GetRequiredService<JwtService>();

            if (!context.HttpContext.Request.Cookies.TryGetValue("Authorisation", out var token))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!jwtService.ValidateToken(token!))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var principal = jwtService.GetPrincipalFromToken(token!);

            if (principal == null || !principal.HasClaim(x => x.Type == "userId"))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var userId = principal.Claims.FirstOrDefault(x => x.Type == "userId")!.Value;

            // Add the user ID to the HttpContext
            context.HttpContext.Items["userId"] = userId;
        }
    }
}
