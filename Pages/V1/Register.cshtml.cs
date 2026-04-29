using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProiectWeb.Data;
using ProiectWeb.Models;

namespace ProiectWeb.Pages.V1;

public class RegisterModel : PageModel
{
    private readonly AppDbContext _context;
    public RegisterModel(AppDbContext context) { _context = context; }

    [BindProperty] public string Email { get; set; } = string.Empty;
    [BindProperty] public string Password { get; set; } = string.Empty;

    public async Task<IActionResult> OnPostAsync()
    {
        // VULNERABILITATE: Fara validare lungime sau complexitate
        // VULNERABILITATE: Stocare in CLAR
        var user = new User { Email = Email, Password = Password, Version = "V1" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return RedirectToPage("/V1/Login");
    }
}