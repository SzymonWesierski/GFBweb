using GameWeb.Data;
using GameWeb.Entities;
using MathWars.StartupInitializers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddIdentity<ApplicationUsers, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = "/Home/IndexGuest";
});

// Initializing basic roles
var roleManager = builder.Services.BuildServiceProvider().GetService<RoleManager<IdentityRole>>();
var configuration = builder.Configuration.GetSection("ApplicationRoles").GetChildren();
foreach (var role in configuration)
{
    RoleInitializer.InitializeRoleAsync(roleManager, role.Value).Wait();
}

// Check if default admin user exist
var userManager = builder.Services.BuildServiceProvider().GetService<UserManager<ApplicationUsers>>();
await AdminInitializer.InitializeUserAsync(userManager, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");
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
