using PxlFund.Mvc.Data;
using PxlFund.Shared.Services;
using PxlFund.Shared.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//Dbcontext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext")));

//Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";
    options.User.RequireUniqueEmail = true;
});

//External Login - Facebook
builder.Services.AddAuthentication()
.AddFacebook(fbOpts =>
{
    fbOpts.AppId = "911973619631699";
    fbOpts.AppSecret = "2d81d76565bc538b1ad3ac562a0281e1";
});

//External Login - Google
builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId =
    "840228473399-gencr6or0udsqckg55a1ras8ndf4bbqm.apps.googleusercontent.com";
    options.ClientSecret = "R9MI9BP_yYorCI-xEB4zMivB";
    options.SignInScheme = IdentityConstants.ExternalScheme;
});

//External Login - Duende
builder.Services.AddAuthentication()
.AddCookie("Cookies")
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = "https://localhost:5001";
    options.ClientId = "mvc";
    options.ClientSecret = "secret";
    options.ResponseType = "code";
    options.SaveTokens = true;
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");
    options.GetClaimsFromUserInfoEndpoint = true;
});


//PxlFund.Shared
builder.Services.AddScoped<ISeedDataRepository, SeedDataRepository>();
builder.Services.AddScoped<IUserLoginRepository, UserLoginRepository>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();