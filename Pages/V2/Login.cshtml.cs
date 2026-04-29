using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProiectWeb.Data;
using BCrypt.Net;
using System.Security.Claims;

namespace ProiectWeb.Pages.V2;

public class LoginModel : PageModel
{
    private readonly AppDbContext _context;
    public LoginModel(AppDbContext context) { _context = context; }

    [BindProperty] public string Email { get; set; } = string.Empty;
    [BindProperty] public string Password { get; set; } = string.Empty;

    public async Task<IActionResult> OnPostAsync()
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == Email);
        
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Email sau parola incorecta.");
            return Page();
        }

        if (user.Locked)
        {
            ModelState.AddModelError(string.Empty, "Contul este blocat din cauza a prea multor incercari esuate. Contactati administratorul.");
            return Page();
        }


        bool isPasswordValid = false;
        try
        {
            isPasswordValid = BCrypt.Net.BCrypt.Verify(Password, user.Password);
        }
        catch (SaltParseException)
        {
            isPasswordValid = false;
        }

        // SECURIZAT: Mesaj Generic si Hashing
        if (!isPasswordValid)

        {
            user.FailedLoginAttempts++;
            if (user.FailedLoginAttempts >= 3)
            {
                user.Locked = true;
            }
            await _context.SaveChangesAsync();

            ModelState.AddModelError(string.Empty, "Email sau parola incorecta.");
            return Page();
        }

        user.FailedLoginAttempts = 0;
        await _context.SaveChangesAsync();

        // Sign in logic
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Email) };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

        TempData["Message"] = "V-ati logat cu succes pe varianta V2 (Securizata)!";
        return RedirectToPage("/Index");
    }
}
