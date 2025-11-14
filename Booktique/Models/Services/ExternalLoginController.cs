using Booktique.Models.MainModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[Route("[controller]/[action]")]
public class ExternalLoginController : Controller
{
    [HttpGet]
    public IActionResult SignInWithGoogle(string returnUrl = "/")
    {
        var redirectUrl = Url.Action("GoogleResponse", "ExternalLogin", new { returnUrl });
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet]
    public async Task<IActionResult> GoogleResponse(string returnUrl = "/")
    {
        var result = await HttpContext.AuthenticateAsync();
        if (!result.Succeeded)
        {
            Console.WriteLine("❌ Autentificare eșuată.");
            return Redirect("/login");
        }

        var claims = result.Principal.Claims;
        var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value?.Trim().ToLower();
        var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value?.Trim();

        if (string.IsNullOrEmpty(email))
        {
            Console.WriteLine("❌ Emailul nu a fost returnat de Google.");
            return Redirect("/login");
        }

        using var scope = HttpContext.RequestServices.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<BooktiqueContext>();

        var user = await db.User.FirstOrDefaultAsync(u => u.UserEmail.ToLower() == email);
        if (user == null)
        {
            user = new User
            {
                UserName = name ?? "GoogleUser",
                UserEmail = email,
                Password = null,
                Role = "User"
            };

            db.User.Add(user);
            await db.SaveChangesAsync();

            Console.WriteLine($"✨ Utilizator nou creat: {email}");
        }
        else
        {
            Console.WriteLine($"🔁 Utilizator existent: {email}");
        }

        // 🔐 Creează un nou ClaimsPrincipal cu UserId
        var claimsIdentity = new ClaimsIdentity(new[]
        {
            new Claim("UserId", user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.UserEmail),
            new Claim(ClaimTypes.Role, user.Role ?? "User")
        }, CookieAuthenticationDefaults.AuthenticationScheme);

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

        return LocalRedirect(returnUrl);
    }
}
