using AutoMapper;

using CodeMonkeys.CMS.Public.Components;
using CodeMonkeys.CMS.Public.Components.Account;
using CodeMonkeys.CMS.Public.Services;
using CodeMonkeys.CMS.Public.Shared;
using CodeMonkeys.CMS.Public.Shared.Data;
using CodeMonkeys.CMS.Public.Shared.Entities;
using CodeMonkeys.CMS.Public.Shared.Profiles;
using CodeMonkeys.CMS.Public.Shared.Repository;
using CodeMonkeys.CMS.Public.Shared.Services;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System.ComponentModel.Design;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CodeMonkeys.CMS.Public.Tests")]

var builder = WebApplication.CreateBuilder(args);

//Använd vilken flagga du vill använda för databasen, Använd false i samband med push och inkl commiten med [USE_CICD] för att använda CICD pipeline mot cloud
// Definiera en flagga för att välja databasanslutning
bool UseDockerConnection = true; // Sätt till true för Docker eller false för DefaultConnection
bool UseOutSourceDB = false; // Sätt till true för OutSourceDB
bool USEWindowsSql = false;
bool AzConnection = false;

// Konfigurera appsettings och miljöinställningar
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.OutSourceDB.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Hämta anslutningssträngen baserat på flaggorna
string connectionString;
Action<DbContextOptionsBuilder> dbConfigFunction;
if (builder.Configuration["database"] == "inMemory")
{
    string databaseName = builder.Configuration["database_name"] ?? "Something";
    dbConfigFunction = (options) => options.UseInMemoryDatabase(databaseName);
}
else
{
    if (UseOutSourceDB)
    {
        connectionString = builder.Configuration.GetConnectionString("OutSourceDBConnectionString");
        Console.WriteLine($"Using connection string: OutSourceDBConnectionString");
    }
    else if (UseDockerConnection)
    {
        connectionString = builder.Configuration.GetConnectionString("DockerConnectionString");
        Console.WriteLine($"Using connection string: DockerConnectionString");
    }
    else if (USEWindowsSql)
    {
        connectionString = builder.Configuration.GetConnectionString("DefaultConnectioString");
        Console.WriteLine($"Using connection string: DefaultConnectioString");
        Console.WriteLine($"Using connection string:{connectionString}");
    }
    else
    {
        connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        Console.WriteLine($"Using connection string: DefaultConnection");
        Console.WriteLine($"Using connection string:{connectionString}");
    }

    // Hämta anslutningssträngen från miljövariabeln om den inte hittades i konfigurationen
    if (string.IsNullOrEmpty(connectionString))
    {
        connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
        Console.WriteLine($"Using connection string: ENV FILE");
    }

    // Kontrollera om anslutningssträngen är tom
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Ingen giltig anslutningssträng hittades.");
    }

    dbConfigFunction = (options) => options.UseSqlServer(connectionString);
}


// Lägg till tjänster till containern
builder.Services.AddDbContextFactory<ApplicationDbContext>(dbConfigFunction);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddRoleManager<RoleManager<Role>>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
.AddIdentityCookies();

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddAutoMapper(typeof(EntityProfiles).Assembly);
builder.Services.AddHttpContextAccessor();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddScoped<ContentItemRepository>();
builder.Services.AddScoped<IContentItemService, ContentItemService>();

builder.Services.AddSingleton<IEmailSender<User>, IdentityNoOpEmailSender>();
builder.Services.AddScoped<IPageStatsRepository, PageStatsRepository>();
builder.Services.AddScoped<IPageStatsService, PageStatsService>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<ISiteRepository, SiteRepository>();
builder.Services.AddScoped<IWebPageRepository, WebPageRepository>();
builder.Services.AddScoped<IContentRepository, ContentRepository>();
builder.Services.AddScoped<ISiteService, SiteService>();
builder.Services.AddScoped<IWebPageService, WebPageService>();
builder.Services.AddScoped<IContentService, ContentService>();
builder.Services.AddScoped<IContentItemService, ContentItemService>();
builder.Services.AddScoped<CodeMonkeys.CMS.Public.Shared.Repository.IContentItemRepository, CodeMonkeys.CMS.Public.Shared.Repository.ContentItemRepository>();
builder.Services.AddScoped<ISectionService, SectionService>();
builder.Services.AddScoped<ISectionRepository, SectionRepository>();
builder.Services.AddScoped<MenuConfigurationService>();

var app = builder.Build();

// Konfigurera HTTP-request-pipeline
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.MapAdditionalIdentityEndpoints();

app.Run();
