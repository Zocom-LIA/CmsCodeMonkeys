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

//builder.Logging.ClearProviders();
builder.Logging.AddConsole();
// TODO: Add logging configuration that includes database storage

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAutoMapper(typeof(EntityProfiles).Assembly);
builder.Services.AddHttpContextAccessor();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddScoped<ContentItemRepository>();
builder.Services.AddScoped<IContentItemService, ContentItemService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies();


Action<DbContextOptionsBuilder> dbConfigFunction;
if (builder.Configuration["database"] == "inMemory")
{
    string databaseName = builder.Configuration["database_name"] ?? "Something";
    dbConfigFunction = (options) => options.UseInMemoryDatabase(databaseName);
}
else
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    dbConfigFunction = (options) => options.UseSqlServer(connectionString);
}
builder.Services.AddDbContextFactory<ApplicationDbContext>(dbConfigFunction);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddRoleManager<RoleManager<Role>>() // Corrected this line
    .AddDefaultTokenProviders();

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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