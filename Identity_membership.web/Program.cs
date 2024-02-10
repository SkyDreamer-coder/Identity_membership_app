using Identity_membership.web.ClaimProviders;
using Identity_membership.web.Extensions;
using Identity_membership.web.Models;
using Identity_membership.web.OptionsModels;
using Identity_membership.web.Requirements;
using Identity_membership.web.Seeds;
using Identity_membership.web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"));
});


builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.FromMinutes(30);
});

builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));    

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddIdentityExtensions();

builder.Services.AddScoped<IEmailService, EmailService>();

//Claim provider implementation
builder.Services.AddScoped<IClaimsTransformation, UserClaimProvider>();

//ExchangeExpireRequirementHandler and !!!(IAuthorizationHandler)!!! reference relate
builder.Services.AddScoped<IAuthorizationHandler, ExchangeExpireRequirementHandler>();

builder.Services.AddScoped<IAuthorizationHandler, ViolenceRequirementHandler>();

//claim policy based authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AfyonPolicy", policy =>
    {
        policy.RequireClaim("city", "Afyonkarahisar");
    });

    //exchange expire date elimination
    options.AddPolicy("ExchangePolicy", policy =>
    {
        policy.AddRequirements(new ExchangeExpireRequirement());
    });

    //birth date policy authorization
    options.AddPolicy("ViolencePolicy", policy =>
    {
        policy.AddRequirements(new ViolenceRequirement() { ThresholdAge = 18 });
    });
});

builder.Services.ConfigureApplicationCookie(opt =>
{
    // cookie builder instance
    var cookieBuilder = new CookieBuilder();
    cookieBuilder.Name = "UserAppCookie";

    // The redirect path if user not logged in the page
    opt.LoginPath = new PathString("/Home/SignIn");
    // Logout redirection query
    opt.LogoutPath = new PathString("/Member/logout");

    //Authorization access denided path
    opt.AccessDeniedPath = new PathString("/Member/AccessDenied");
    // cookie options
    opt.Cookie = cookieBuilder;
    opt.ExpireTimeSpan = TimeSpan.FromDays(60);
    opt.SlidingExpiration = true;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<UserRoleApp>>();

    await PermissionSeed.Seed(roleManager);
}

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
