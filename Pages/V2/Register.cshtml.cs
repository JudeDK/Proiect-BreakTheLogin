using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProiectWeb.Data;
using ProiectWeb.Models;
using BCrypt.Net;
using System.ComponentModel.DataAnnotations;

namespace ProiectWeb.Pages.V2;

public class RegisterModel : PageModel
{
    private readonly AppDbContext _context;
    public RegisterModel(AppDbContext context) { _context = context; }

    [BindProperty, Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [BindProperty, Required, MinLength(8)] public string Password { get; set; } = string.Empty;

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        if (_context.Users.Any(u => u.Email == Email)) {
             ModelState.AddModelError("Email", "Email deja ocupat");
             return Page();
        }

        // SECURIZAT: Hashing BCrypt
        var user = new User { 
            Email = Email, 
            Password = BCrypt.Net.BCrypt.HashPassword(Password)  ,
            Version = "V2" ,
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return RedirectToPage("/V2/Login");
    }
}