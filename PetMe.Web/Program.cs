using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using PetMe.Core.Entities;
using PetMe.Data;
using PetMe.Setting;

var builder = WebApplication.CreateBuilder(args);

var setting = EnviromentSetting.GetInstance();

// SMTP Settings
var smtpSettings = new SmtpSettings
{
    Host = setting.GetSMTPHost(),
    Port = int.Parse(setting.GetSMTPPort()),
    Password = setting.GetSMTPPassword(),
    FromEmail = setting.GetSMTPFFromEmail(),
};
builder.Services.AddSingleton(smtpSettings);

// Add distributed memory cache and session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Authentication and Authorization
builder.Services.AddAuthentication("Cookies")
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();
// Add services to the container.
builder.Services.AddControllersWithViews();

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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
