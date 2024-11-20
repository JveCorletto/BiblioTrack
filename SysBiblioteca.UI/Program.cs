using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using SysBiblioteca.UI.Management;
using SysBiblioteca.UI.Middlewares;
using SysBiblioteca.UI.RDIF_Module;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<API_Configs>(builder.Configuration.GetSection("API_Configs"));

builder.Services.Configure<SerialPortConfig>(builder.Configuration.GetSection("SerialPortConfig"));
builder.Services.AddHttpContextAccessor();
builder.Services.AddSignalR();

// Registro del servicio SerialPortListener como Singleton
builder.Services.AddSingleton<SerialPortListener>(serviceProvider =>
{
    // Obtener la configuración del puerto serial
    var config = serviceProvider.GetRequiredService<IOptions<SerialPortConfig>>().Value;

    // Obtener el HttpContextAccessor
    var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();

    // Obtener el hub context para SignalR
    var hubContext = serviceProvider.GetRequiredService<IHubContext<RfidHub>>();

    // Crear una instancia del SerialPortListener con los parámetros requeridos
    var listener = new SerialPortListener(config.PortName, config.BaudRate, httpContextAccessor);

    // Asignar el evento para leer el tag y notificar a los clientes conectados
    listener.OnTagRead += async (tag) =>
    {
        await hubContext.Clients.All.SendAsync("ReceiveTag", tag);
    };

    return listener;
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

app.MapHub<RfidHub>("/rfidHub");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();