using ApsCourseFinder.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<CourseCatalog>();
builder.Services.AddSingleton<ApsCalculator>();
builder.Services.AddSingleton<EligibilityService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute("courses", "Courses", new { controller = "Home", action = "Courses" });
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

app.Run();
