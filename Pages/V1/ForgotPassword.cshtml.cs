using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProiectWeb.Data;

namespace ProiectWeb.Pages.V1;

public class ForgotPasswordModel : PageModel
{
    private readonly AppDbContext _context;
    public ForgotPasswordModel(AppDbContext context) { _context = context; }
    [BindProperty] public string Email { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    public void OnPost()
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == Email);
        if (user == null) {
            Message = "Eroare: Acest email nu exista in baza de date!"; // ENUMERARE
        } else {
            user.ResetToken = "1234"; // PREDICTIBIL
            _context.SaveChanges();
            Message = "Token trimis: 1234";
        }
    }
}