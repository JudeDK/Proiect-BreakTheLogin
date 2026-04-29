using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProiectWeb.Data;
using System.Security.Claims;

namespace ProiectWeb.Pages.V1;

public class LoginModel : PageModel
{
    private readonly AppDbContext _context;
    public LoginModel(AppDbContext context) { _context = context; }

    [BindProperty] public string Email { get; set; } = string.Empty;
    [BindProperty] public string Password { get; set; } = string.Empty;

    public async Task<IActionResult> OnPostAsync()
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == Email);
        
        // VULNERABILITATE: Enumerarea Utilizatorilor
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Eroare: Utilizatorul NU exista!");
            return Page();
        }

        // VULNERABILITATE: Comparare in CLAR
        if (user.Password != Password)
        {
            ModelState.AddModelError(string.Empty, "Eroare: Parola gresita pentru acest user!");
            return Page();
        }

        // Sign in logic
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Email) };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

        TempData["Message"] = "V-ati logat cu succes pe varianta V1 (Vulnerabila)!";
        return RedirectToPage("/Index");
    }
}