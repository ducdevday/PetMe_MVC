using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using PetMe.Business.Services;
using PetMe.Core.Entities;
using PetMe.Data;
using PetMe.Data.Repositories;
using PetMe.DataAccess.Repositories;
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
builder.Services.AddHttpClient();
builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();

builder.Services.AddDbContext<PetMeDbContext>(options =>
    options.UseNpgsql(setting.GetConnectionString()));

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IAdoptionService, AdoptionService>();
builder.Services.AddTransient<IAdoptionRepository, AdoptionRepository>();
builder.Services.AddTransient<IPetService, PetService>();
builder.Services.AddTransient<IPetRepository, PetRepository>();
builder.Services.AddTransient<IAdoptionService, AdoptionService>();
builder.Services.AddTransient<IPetOwnerRepository, PetOwnerRepository>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IPetOwnerService, PetOwnerService>();
builder.Services.AddTransient<IAdoptionRequestRepository, AdoptionRequestRepository>();
builder.Services.AddTransient<IAdoptionRequestService, AdoptionRequestService>();
builder.Services.AddTransient<ILostPetAdRepository, LostPetAdRepository>();
builder.Services.AddTransient<ILostPetAdService, LostPetAdService>();
builder.Services.AddTransient<IHelpRequestRepository, HelpRequestRepository>();
builder.Services.AddTransient<IHelpRequestService, HelpRequestService>();
builder.Services.AddTransient<IVeterinarianService, VeterinarianService>();
builder.Services.AddTransient<IVeterinarianRepository, VeterinarianRepository>();
builder.Services.AddTransient<IAdminService, AdminService>();
builder.Services.AddTransient<IAdminRepository, AdminRepository>();
builder.Services.AddTransient<ICommentService, CommentService>();
builder.Services.AddTransient<ICommentRepository, CommentRepository>();
builder.Services.AddTransient<IVnAddressService, VnAddressService>();

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
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
