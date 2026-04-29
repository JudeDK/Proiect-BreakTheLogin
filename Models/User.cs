using System;
using System.ComponentModel.DataAnnotations;

namespace ProiectWeb.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Email { get; set; } = string.Empty;
    
    // In V1 we store plain text or MD5 here. In V2 we store BCrypt hash.
    [Required]
    public string Password { get; set; } = string.Empty; 
    
    public bool Locked { get; set; } = false;
    public int FailedLoginAttempts { get; set; } = 0;
    public string Version { get; set; } = string.Empty;

    public string? ResetToken { get; set; }
    public DateTime? ResetTokenExpires { get; set; }
}
