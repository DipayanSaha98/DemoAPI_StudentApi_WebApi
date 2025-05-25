using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


builder.Services.AddDistributedMemoryCache();                     // since we are using cookie based authenticate user so this service can be useful for catching caching the data across multiple requests within a single application instance .
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.Cookie.HttpOnly = true; // Make the session cookie HTTP-only
    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);  // Set session timeout to 10 minutes
    options.LoginPath = "/AuthUser/Login"; // Redirect to login page if authenticated
    options.AccessDeniedPath = "/AuthUser/AccessDenied"; // Redirect to access denied page if user is not authorized
    options.SlidingExpiration = true; // Enable sliding expiration
});


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);  // Set session timeout to 10 minutes
    options.Cookie.HttpOnly = true; // Make the session cookie HTTP-only
    options.Cookie.IsEssential = true; // Make the session cookie essential
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
// Enable session state
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
