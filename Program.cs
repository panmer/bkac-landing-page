using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using gcsharpRPC.Data;
using gcsharpRPC.Helpers;
using gcsharpRPC.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
.AddRazorPages();
// .AddRazorPagesOptions(options =>
// {
//     options.Conventions.AuthorizeFolder("/");
//     options.Conventions.AllowAnonymousToPage("/Index");
//     options.Conventions.AllowAnonymousToPage("/Identity/Login");
//     options.Conventions.AllowAnonymousToPage("/Identity/ExternalLogin");
//     options.Conventions.AllowAnonymousToPage("/Privacy");
//     options.Conventions.AllowAnonymousToPage("/Identity/AccessDenied");
//     options.Conventions.AllowAnonymousToPage("/Error");
//     options.Conventions.AllowAnonymousToPage("/Events/List");
//     options.Conventions.AllowAnonymousToPage("/Events/Create");
// });

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddDbContext<TrungContext>();

/**
*   Authentication config
**/
// builder.Services
// .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
// .AddEntityFrameworkStores<TrungContext>();

// builder.Services.Configure<IdentityOptions>(options =>
// {
//     // Password settings.
//     options.Password.RequireDigit = true;
//     options.Password.RequireLowercase = false;
//     options.Password.RequireNonAlphanumeric = false;
//     options.Password.RequireUppercase = false;
//     options.Password.RequiredLength = 5;
//     options.Password.RequiredUniqueChars = 1;

//     // Lockout settings.
//     options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
//     options.Lockout.MaxFailedAccessAttempts = 5;
//     options.Lockout.AllowedForNewUsers = true;

//     // User settings.
//     options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
//     options.User.RequireUniqueEmail = false;
// });

// builder.Services.AddAuthentication()
// .AddGoogle(googleOptions =>
// {
//     IConfigurationSection googleSection = builder.Configuration.GetSection("Authentication:Google");

//     googleOptions.ClientId = googleSection["ClientId"];
//     googleOptions.ClientSecret = googleSection["ClientSecret"];
//     googleOptions.CallbackPath = "/Index";
// });

// builder.Services.ConfigureApplicationCookie(options =>
// {
//     // Cookie settings
//     // options.Cookie.HttpOnly = true;
//     // options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

//     options.LoginPath = "/Identity/Login";
//     options.AccessDeniedPath = "/Identity/AccessDenied";
//     options.SlidingExpiration = true;
// });
// builder.Services.AddAuthentication(options =>
// {
//     // Password settings.
//     options.Password.RequireDigit = true;
//     options.Password.RequireLowercase = true;
//     options.Password.RequireNonAlphanumeric = true;
//     options.Password.RequireUppercase = true;
//     options.Password.RequiredLength = 6;
//     options.Password.RequiredUniqueChars = 1;

//     // Lockout settings.
//     options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
//     options.Lockout.MaxFailedAccessAttempts = 5;
//     options.Lockout.AllowedForNewUsers = true;

//     // User settings.
//     options.User.AllowedUserNameCharacters =
//     "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
//     options.User.RequireUniqueEmail = false;
// }).AddCookie(options =>
// {
//     options.Cookie.Name = builder.Configuration["IdentityProvider:CookieName"];
//     options.Cookie.HttpOnly = true;
//     // options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
//     // options.Cookie.SameSite = SameSiteMode.Strict;

//     // Console.WriteLine(options.Cookie);

//     options.LoginPath = new PathString("/Identity/Login");  
//     options.ExpireTimeSpan = TimeSpan.FromMinutes(30.0);  
//     // options.Events.OnSigningOut = async e => await e.HttpContext.RevokeUserRefreshTokenAsync();
// });

/**
* Add Service
**/
builder.Services.AddScoped<PollService>();

// builder.Services.AddDatabaseDeveloperPageExceptionFilter();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<TrungContext>();
    context.Database.EnsureCreated();
    DbInitializer.Initialize(context);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

// app.UseAuthentication();
// app.UseAuthorization();

app.MapRazorPages();

app.Run();
