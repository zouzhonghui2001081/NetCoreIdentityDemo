using IdentityDemo.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using IdentityDemo.Service;

var builder = WebApplication.CreateBuilder(args);

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", false)
    .Build();

builder.Services.Configure<SmtpOptions>(config.GetSection("Smtp"));

var conString = config.GetConnectionString("default");
builder.Services.AddDbContext<AppDbContext>(o => o.UseNpgsql(conString));
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
builder.Services.Configure<IdentityOptions>(o => {
    o.Password.RequiredLength = 6;
    o.Password.RequireDigit = true;
    o.Password.RequireNonAlphanumeric = false;
    o.Lockout.MaxFailedAccessAttempts = 3;
    o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(30);
    o.SignIn.RequireConfirmedEmail = true;
});


builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = "/Identity/Signup";
    options.AccessDeniedPath = "/Identity/AccessDenied";
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IEmailSender, SmtpEmailSender>();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

InitializeIdentityDB(app);
app.Run();


void InitializeIdentityDB(WebApplication app)
{
    using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
    {
        var appDbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
        appDbContext.Database.Migrate();
    }
}
