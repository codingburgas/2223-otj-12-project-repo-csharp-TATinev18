using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Identity.Data;
using WebApp.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Default Connection") ?? throw new InvalidOperationException("Connection string 'IdentityContextConnection' not found.");

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("Default Connection")
));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(TokenOptions.DefaultProvider);


builder.Services.AddRazorPages();

var cultureInfo = new CultureInfo("bg-BG");


CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
cultureInfo.NumberFormat.CurrencySymbol = "ыт.";

decimal amount = 1000.50m;
string formattedAmount = string.Format("{0} {1:N2}", cultureInfo.NumberFormat.CurrencySymbol, amount);

Console.WriteLine(formattedAmount);


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); ;
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
