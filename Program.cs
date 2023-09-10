using Auth0.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using NoteItEasyApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure services for the DataContext using SQLite database provider
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure and add Auth0 authentication services to the application's service collection
builder.Services.AddAuth0WebAppAuthentication(options =>
{
    // Set the Auth0 domain by retrieving it from the app's configuration settings
    options.Domain = builder.Configuration["Auth0:Domain"];

    // Set the Auth0 client ID by retrieving it from the app's configuration settings
    options.ClientId = builder.Configuration["Auth0:ClientId"];
});

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

app.MapGet("/", async (HttpContext context) =>
{
    // Redirect to the "Profile" action after authentication
    context.Response.Redirect("/Account/Profile");
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

