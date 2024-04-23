//using Net.Gwiasda;

using Net.Gwiasda.Local.Repository;
using Net.Gwiasda.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<ILoggingRepository, FileSystemLoggingRepository>();
builder.Services.AddSingleton<ILoggingManager, LoggingManager>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
