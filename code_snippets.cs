//EXAMEN C SHARP

#region Connectionstring in AppSettings
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "ClientLocationData": "Server=(localdb)\\mssqllocaldb;Database=KiesHierDeNaamVanJeDataBase;MultipleActiveResultSets=true;Trusted_Connection=True;"
  }
}
#endregion

#region packages and migration
//installing packages
Install-Package Microsoft.AspNetCore.Authentication.Facebook -Version 8.0.19
Install-Package Microsoft.AspNetCore.Authentication.Google -Version 8.0.19
Install-Package Microsoft.AspNetCore.Authentication.OpenIdConnect -Version 8.0.19
Install-Package Microsoft.AspNetCore.Identity.EntityFrameworkCore -Version 8.0.19
Install-Package Microsoft.EntityFrameworkCore -Version 8.0.19
Install-Package Microsoft.EntityFrameworkCore.Design -Version 8.0.19
Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 8.0.19
Install-Package Microsoft.EntityFrameworkCore.Tools -Version 8.0.19
Install-Package Microsoft.AspNetCore.Components.WebAssembly.Server -Version 8.0.19

//migration - database
add-migration NameOfMigration
update-database
#endregion

#region Program.cs settings

#region DbContext
//DbContext
builder.Services.AddDbContext<NaamVanUwDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("NaamVanUwConnectionInAppSettings.json"));
});
#endregion

#region Identity
//Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredUniqueChars = 1;
})
.AddEntityFrameworkStores<YourDbContext>()
.AddDefaultTokenProviders();
#endregion

#region External Login
//Google
builder.Services.AddAuthentication().AddGoogle(options =>
{
	options.ClientId =
	"CLIENT_ID_HERE";
	options.ClientSecret = "CLIENT_SECRET_HERE";
	options.SignInScheme = IdentityConstants.ExternalScheme;
	options.CallbackPath = new PathString("/signin-google"); //belangrijk voor google te werken

});

//Facebook
builder.Services.AddAuthentication()
.AddFacebook(fbOpts =>
{
	fbOpts.AppId = "APP_ID_HERE";
	fbOpts.AppSecret = "APP_SECRET_HERE";
});

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

#region DbInitializer - SeedData
//Dit kan op twee manieren gebeuren, ofwel zit de "using scope" in de seeddata zelf, ofwel niet.
//indien dit in de seeddata zelf zit moet je enkel dit toevoegen in program.cs:

app.EnsureDbMigrated(); //naam van de functie in je seedata/initializer ziet er zo uit:
                                                                                            public static void EnsureDbMigrated(this IApplicationBuilder applicationBuilder)

//indien je SeedData een gewone class is zoal SeedDataRepository, dan moet je dit doen in program.cs:
using (var scope = app.Services.CreateScope())
{
	var seedService = scope.ServiceProvider.GetRequiredService<ISeedDataRepository>();
	seedService.Initialise();
}

// IN BEIDE GEVALLEN MOET HET GEDEELTE TOEGEVOEGD WORDEN NET VOOR app.Run();
#endregion

//MOET ALTIJD AANWEZIG ZIJN
app.UseAuthentication();
app.UseAuthorization();
#endregion

#region DbContext Sample
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}

		public DbSet<Bank> Banks { get; set; }
		public DbSet<Fund> Funds { get; set; }
		public DbSet<UserFund> UserFunds { get; set; }

	}
#endregion

#region Blazor Setup

//service toevoegen
builder.Services.AddServerSideBlazor();

//middleware toevoegen
app.MapBlazorHub();

//_layout.cshtml toevoegen
//in de <head>
<base href="~/" />

//in de body (einde)
<script src="_framework/blazor.server.js"></script>

//voeg een bestand toe _imports.razor (doe dit in de project folder)
@using Microsoft.AspNetCore.Components
@using Microsoft.AspNetCore.Components.Forms
@using Microsoft.AspNetCore.Components.Routing
@using Microsoft.AspNetCore.Components.Web
@using Microsoft.JSInterop


//maak een map Blazor (in de project folder)
//maak een Razor component TestComponent.razor

//in je view kan je nu dat razor component laden, er hoeft niets anders te staan in je view
<component type="typeof(PeopleApp.Blazor.TestComponent)" render-mode="Server"/>
#endregion

#region TagHelper Setup


#endregion

#region Attributes Sample (API key)

//Add API key to appsettings
,
"ApiKey": "SuperSecretApiKey"


//Maak Folder Attributes
//Maak Klasse genaamd XXXAttribute in deze folder

[AttributeUsage(validOn: AttributeTargets.Class)]
public class ApiKeyAttribute : Attribute, IAsyncActionFilter
{
    private const string APIKEYNAME = "ApiKey";
    private ContentResult GetContentResult(int statusCode, string content)
    {
        var result=new ContentResult();
        result.StatusCode = statusCode;
        result.Content = content;
        return result;
    }
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {   
        try
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
            {
                context.Result = GetContentResult(401, "Api Key was not provided");
                return;
            }
            var appSettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            if (appSettings == null)
            {
                context.Result = GetContentResult(401, "Appsettings not found");
                return;
            }
            var apiKey = appSettings.GetValue<string>(APIKEYNAME);
            if (apiKey == null)
            {
                context.Result = GetContentResult(401, "Appsettings - ApiKey - not found");
                return;
            }
            if (!apiKey.Equals(extractedApiKey))
            {
                context.Result = GetContentResult(401, "Api Key is not valid");
                return;
            }
            await next();
        }
        catch (Exception ex)
        {
        context.Result = GetContentResult(401, ex.Message);
        return;
        }
    }
}

//you can now use the attribute as [ApiKey] (name of the attribute class minus "Attribute")

#endregion

#region API accessen met HttpClient

//GET
private static readonly HttpClient client = new HttpClient();
var result = await client.GetFromJsonAsync<WeatherDto>("https://localhost:5001/api/weather");

//POST
var newItem = new CreateUserRequest { Name = "John", Age = 30 };
var response = await client.PostAsJsonAsync("https://localhost:5001/api/users", newItem);
if (response.IsSuccessStatusCode)
{
    Console.WriteLine("User created!");
}

//Optional: Reuse HttpClient with IHttpClientFactory (DI-style)

//set API settings
"ApiSettings": {
  "BaseUrl": "https://api.example.com/",
  "ApiKey": "your-secret-key"
}

var apiSettings = builder.Configuration.GetSection("ApiSettings");

builder.Services.AddHttpClient("MyApi", client =>
{
    client.BaseAddress = new Uri(apiSettings["BaseUrl"]);
    client.DefaultRequestHeaders.Add("X-Api-Key", apiSettings["ApiKey"]);
});

//Daarna kan je HttpClient injecteren volgens DI en het zo gebruiken:

private readonly HttpClient _client;

    public MyService(IHttpClientFactory factory)
    {
        _client = factory.CreateClient("MyApi");
    }

    public async Task<WeatherDto> GetWeatherAsync()
    {
        return await _client.GetFromJsonAsync<WeatherDto>("weather");
    }

#endregion

#region Algemene Info
//De container maakt elke keer een nieuw object aan als er een instantie van een service(dependency) gevraagd wordt.
builder.Services.AddTransient<ServiceInterface, Service>();

//Bij een scoped service (dependency) wordt hetzelfde object steeds teruggegeven zolang dit gevraagd wordt binnen dezelfde http request. 
//Als er een nieuwe http request (een nieuwe scope) binnenkomt, dan wordt er weer een nieuwe instantie gemaakt die dan steeds wordt teruggegeven tijdens het verwerken van die http request.
builder.Services.AddScoped<ServiceInterface, Service>();

//De eerste keer dat een singleton service wordt gevraagd, wordt er een nieuw object gemaakt. Alle volgende keren wordt ditzelfde object teruggegeven.
builder.Services.AddSingleton<ServiceInterface, Service>();

#endregion


#region Extra
//Stap 1: Definieer een interface voor de data-operaties die je nodig hebt
// In Shared project
public interface IApplicationDbContext
{
    DbSet<MyModel> MyModels { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

//Stap 2: Implementeer deze interface in het MVC-project
// In MVC project
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<MyModel> MyModels { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
}

//Stap 3: Registreer de interface in Program.cs of Startup.cs
builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();


//Stap 4: Injecteer de interface in je service
public class MyService
{
    private readonly IApplicationDbContext _context;

    public MyService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task DoSomething()
    {
        var data = await _context.MyModels.ToListAsync();
        // etc.
    }
}

#endregion




