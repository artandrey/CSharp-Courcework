using DTO;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[Controller]
[Route("api/set-me-cookie")]
public class AuthController : Controller
{
    [HttpPost]
    public async Task<IActionResult> SetMeCookie([FromBody] TokenDTO tokenDTO)
    {
        string cookieName = "Authorisation";
        string cookieValue = tokenDTO.Token;

        await Task.Run(() =>
        {
            CookieOptions options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1),
                HttpOnly = true
            };

            Response.Cookies.Append(cookieName, cookieValue, options);
        });
        return Ok("cool");
    }
}