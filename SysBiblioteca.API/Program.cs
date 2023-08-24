using System.Text;
using SysBiblioteca.API.dbContext;
using Microsoft.IdentityModel.Tokens;
using SysBiblioteca.API.Services.ADM.RolesService;
using SysBiblioteca.API.Services.CTL.EstadosService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SysBiblioteca.API.Services.ADM.UsuariosService;
using SysBiblioteca.API.Services.CTL.CargosService;
using SysBiblioteca.API.Services.ADM.DatosPersonalesService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Se agrega el contexto de la base de datos
builder.Services.AddDbContext<DataContext>();

//Se agrega la inyección de dependencias de los servicios
//Catálogos
builder.Services.AddScoped<iEstadosService, EstadosService>();
builder.Services.AddScoped<iCargosService, CargosService>();

//Administración
builder.Services.AddScoped<iRolesService, RolesService>();
builder.Services.AddScoped<iDatosPersonalesService, DatosPersonalesService>();
builder.Services.AddScoped<iUsuariosService, UsuariosService>();

//Configuración de JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:JWT_SECRET_KEY"])),

        ValidIssuer = builder.Configuration["JWT:JWT_ISSUER_TOKEN"],
        ValidAudience = builder.Configuration["JWT:JWT_AUDIENCE_TOKEM"],
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();