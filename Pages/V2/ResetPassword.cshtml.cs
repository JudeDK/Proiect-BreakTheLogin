using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProiectWeb.Data;
using BCrypt.Net;

namespace ProiectWeb.Pages.V2;

public class ResetPasswordModel : PageModel
{
    private readonly AppDbContext _context;
    public ResetPasswordModel(AppDbContext context) { _context = context; }

    [BindProperty] public string Token { get; set; } = string.Empty;
    [BindProperty] public string NewPassword { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    public void OnGet(string token) { Token = token; }

    public void OnPost()
    {
        // SECURIZAT: Verifica atat Token-ul cat si Expirarea
        var user = _context.Users.FirstOrDefault(u => u.ResetToken == Token && u.ResetTokenExpires > DateTime.UtcNow);
        if (user != null) {
            user.Password = BCrypt.Net.BCrypt.HashPassword(NewPassword); // Hashing BCrypt
            user.ResetToken = null;
            user.ResetTokenExpires = null;
            _context.SaveChanges();
            Message = "Parola a fost resetata securizat (V2).";
        } else {
            Message = "Link-ul este invalid sau a expirat.";
        }
    }
}