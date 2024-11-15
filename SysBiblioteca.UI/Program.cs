using Microsoft.Extensions.Options;
using SysBiblioteca.UI.Management;
using SysBiblioteca.UI.Middlewares;
using SysBiblioteca.UI.RDIF_Module;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<API_Configs>(builder.Configuration.GetSection("API_Configs"));

builder.Services.Configure<SerialPortConfig>(builder.Configuration.GetSection("SerialPortConfig"));
builder.Services.AddSingleton(serviceProvider =>
{
    var config = serviceProvider.GetRequiredService<IOptions<SerialPortConfig>>().Value;
    return new SerialPortListener(config.PortName, config.BaudRate);
});

builder.Services.AddHostedService<SerialPortHostedService>();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".SysBiblioteca.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseCors(options => options
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod());

app.UseSession();
app.UseAuthenticationMiddleware();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();