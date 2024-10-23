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
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CodeMonkeys.CMS.Public.Tests")]

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog console logging and enrichers

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAutoMapper(typeof(EntityProfiles).Assembly);
builder.Services.AddHttpContextAccessor();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<ContentItemService>();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
.AddIdentityCookies();

Action<DbContextOptionsBuilder> dbConfigFunction;

// Konfigurera databasanslutning baserat på miljön
if (builder.Environment.IsDevelopment())
{
    // Använd Docker-anslutningssträngen under utveckling
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Connection string not found for development. Please set the DefaultConnection in your appsettings.json.");
    }

    dbConfigFunction = (options) => options.UseSqlServer(connectionString);
}
else if (builder.Environment.IsEnvironment("Testing"))
{
    // Använd in-memory databas för testning
    string databaseName = builder.Configuration["database_name"] ?? "TestDatabase";
    dbConfigFunction = (options) => options.UseInMemoryDatabase(databaseName);
}
else
{
    // Använd DB_CONNECTION_STRING i produktion
    var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Connection string not found. Please set the DB_CONNECTION_STRING environment variable.");
    }
    
    dbConfigFunction = (options) => options.UseSqlServer(connectionString);
}

// Registrera DbContext
builder.Services.AddDbContextFactory<ApplicationDbContext>(dbConfigFunction);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddRoleManager<RoleManager<Role>>() // Corrected this line
    .AddDefaultTokenProviders();

// Registrera repositorier
builder.Services.AddScoped<IContentItemRepository, ContentItemRepository>();
builder.Services.AddScoped<ContentItemRepository>();
builder.Services.AddScoped<IContentItemService, ContentItemService>();
builder.Services.AddScoped<IContentRepository, ContentRepository>();
builder.Services.AddScoped<IContentService, ContentService>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IPageStatsRepository, PageStatsRepository>();
builder.Services.AddScoped<IPageStatsService, PageStatsService>();
builder.Services.AddScoped<ISectionRepository, SectionRepository>();
builder.Services.AddScoped<ISectionService, SectionService>();
builder.Services.AddScoped<ISiteRepository, SiteRepository>();
builder.Services.AddScoped<ISiteService, SiteService>();
builder.Services.AddScoped<IWebPageRepository, WebPageRepository>();
builder.Services.AddScoped<IWebPageService, WebPageService>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<MenuConfigurationService>();
builder.Services.AddSingleton<IEmailSender<User>, IdentityNoOpEmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
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

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
