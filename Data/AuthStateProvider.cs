using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace ServerApp.Data
{
    public class AppAuthStateProvider : AuthenticationStateProvider
    {
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "Johndoe@gmail.com"),
            }, "apiauth_ype");
            var user = new ClaimsPrincipal(identity);

            return Task.FromResult(new AuthenticationState(user));
        }
    }
}