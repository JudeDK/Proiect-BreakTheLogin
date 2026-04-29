using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProiectWeb.Data;

namespace ProiectWeb.Pages.V1;

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
        // VULNERABILITATE: Nu verifica expirarea token-ului
        var user = _context.Users.FirstOrDefault(u => u.ResetToken == Token);
        if (user != null) {
            user.Password = NewPassword; // Salvare in CLAR
            user.ResetToken = null;
            _context.SaveChanges();
            Message = "Parola a fost resetata (V1)!";
        } else {
            Message = "Token invalid!";
        }
    }
}