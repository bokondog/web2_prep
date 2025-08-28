using Template.Mvc.Services.Interfaces;
using Template.Mvc.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Template.Mvc.Data;
using Template.Mvc.Data.DefaultData;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region services
//DB: connection string
var templateDbConnection = builder.Configuration.GetConnectionString("TemplateDbContext");
//DB: context
builder.Services.AddDbContext<TemplateDbContext>(options => options.UseSqlServer(templateDbConnection));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;

})
.AddEntityFrameworkStores<TemplateDbContext>();

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<IExternalAuthService, ExternalAuthService>();

//blazor
builder.Services.AddServerSideBlazor();

//Duende
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

#endregion

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

app.MapBlazorHub();

//DB: seeding
await SeedData.EnsurePopulatedAsync(app);

app.Run();
