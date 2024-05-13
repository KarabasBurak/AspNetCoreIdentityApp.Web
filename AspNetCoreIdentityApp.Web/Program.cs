using AspNetCoreIdentityApp.Web.ClaimProviders;
using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Web.Models;
using AspNetCoreIdentityApp.Web.OptionsModels;
using AspNetCoreIdentityApp.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"));
});

// SecurityStamp kısmını tanımladık
builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.FromMinutes(30);
});

builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));


builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings")); // Herhangi bir class'ın constructorında IOptions<EmailSettings> görürsen dataları GetSection'dan oku
builder.Services.AddIdentityExtension();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IClaimsTransformation, UserClaimProvider>();

// Policy ile Yetkilendirme
builder.Services.AddAuthorization(opts =>
{
    opts.AddPolicy("AnkaraPolicy", policy =>
    {
        policy.RequireClaim("City", "Ankara", "Manisa");
        
    });
});

builder.Services.ConfigureApplicationCookie(opt =>
{
    var cookieBuilder = new CookieBuilder();
    cookieBuilder.Name = "IdentityAppCookie";
    opt.LoginPath = new PathString("/Login/Login");
    opt.AccessDeniedPath = new PathString("/Access/AccessDenied");
    opt.Cookie=cookieBuilder;
    opt.ExpireTimeSpan=TimeSpan.FromDays(60); // Cookie süresi 60 gün olarak belirlendi
    opt.SlidingExpiration=true; // Kullanıcı her giriş yaptığında 60 ekleme için yazıldı. Yani; 30. gün giriş yaptı 90 günlük cookie olur

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
