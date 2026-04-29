using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProiectWeb.Data;
using System.Security.Cryptography;

namespace ProiectWeb.Pages.V2;

public class ForgotPasswordModel : PageModel
{
    private readonly AppDbContext _context;
    public ForgotPasswordModel(AppDbContext context) { _context = context; }
    [BindProperty] public string Email { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    public void OnPost()
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == Email);
        if (user != null) {
            user.ResetToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(16)); // RANDOM
            user.ResetTokenExpires = DateTime.UtcNow.AddMinutes(15);
            _context.SaveChanges();
        }
        Message = "Daca emailul exista, vei primi un link de resetare."; // GENERIC
    }
}