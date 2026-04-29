using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ProiectWeb.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => { 
        options.LoginPath = "/V1/Login"; 
        options.LogoutPath = "/Logout";
    });

var app = builder.Build();

Console.WriteLine("==========================================");
Console.WriteLine("Proiect Securitate Web - Realizat de: RARES");
Console.WriteLine("==========================================");

if (!app.Environment.IsDevelopment()) { app.UseExceptionHandler("/Error"); app.UseHsts(); }
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.Run();
