using EmployAI.Controllers;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("https://localhost:7217", "http://localhost:5004");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", policy =>
    {
        policy.AllowAnyOrigin() 
              .AllowAnyHeader() 
              .AllowAnyMethod(); 
    });
});
// Add HttpClient to the service container in Program.cs or Startup.cs
builder.Services.AddHttpClient<HomeController>();

var app = builder.Build();
app.UseCors("AllowLocalhost");


app.UseCors("AllowLocalhost");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
