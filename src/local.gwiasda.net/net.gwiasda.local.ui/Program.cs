//using Net.Gwiasda;

using Net.Gwiasda.FiMa;
using Net.Gwiasda.FiMa.Categories;
using Net.Gwiasda.Local.Repository;
using Net.Gwiasda.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<ILoggingRepository, FileSystemLoggingRepository>();
builder.Services.AddSingleton<ILoggingManager, LoggingManager>();
builder.Services.AddSingleton<ICategoryRepository, FiMaFileSystemCategoryRepository>();
builder.Services.AddSingleton<ICategoryValidator, CategoryValidator>();
builder.Services.AddSingleton<ICategoryManager, CategoryManager>();
builder.Services.AddSingleton<ICategorySaveWorkflow, CategorySaveWorkflow>();

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
