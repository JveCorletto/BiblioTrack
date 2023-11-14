using System.Text;
using Microsoft.OpenApi.Models;
using SysBiblioteca.API.dbContext;
using Microsoft.IdentityModel.Tokens;
using SysBiblioteca.API.Services.ADM.RolesService;
using SysBiblioteca.API.Services.ADM.MenusService;
using SysBiblioteca.API.Services.ADM.CargosService;
using SysBiblioteca.API.Services.CTL.GenerosService;
using SysBiblioteca.API.Services.CTL.EstadosService;
using SysBiblioteca.API.Services.ADM.UsuariosService;
using SysBiblioteca.API.Services.ADM.LinkRolMenuService;
using SysBiblioteca.API.Services.CTL.EstadosMultasService;
using SysBiblioteca.API.Services.ADM.DatosPersonalesService;
using SysBiblioteca.API.Services.INV.AutoresService;
using SysBiblioteca.API.Services.INV.GenerosLiterariosService;
using SysBiblioteca.API.Models.INV;
using SysBiblioteca.API.Services.INV.EditorialesService;
using SysBiblioteca.API.Services.INV.LibrosService;
using SysBiblioteca.API.Services.INV.AutoresLibrosService;
using SysBiblioteca.API.Services.INV.GenerosLibrosService;
using SysBiblioteca.API.Services.INV.SeccionesService;
using SysBiblioteca.API.Services.INV.EstanteriasService;
using SysBiblioteca.API.Services.INV.NivelesService;
using SysBiblioteca.API.Services.INV.UbicacionesService;
using SysBiblioteca.API.Services.PRS.PrestamosService;
using SysBiblioteca.API.Services.PRS.MultasService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    //Título del Swagger
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SysBiblioteca", Version = "v1" });

    //Botón de Authorize
    c.AddSecurityDefinition("Authorization", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Authorization"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Authorization"
                }
            },
            new String[] {}
        }
    });
});

builder.Services.AddCors(option =>
{
    option.AddPolicy("SysBiblioteca_Policy", builder =>
    {
        builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
    });
});

//Se agrega el contexto de la base de datos
builder.Services.AddDbContext<DataContext>();

//Se agrega la inyección de dependencias de los servicios
//Catálogos
builder.Services.AddScoped<iEstadosService, EstadosService>();
builder.Services.AddScoped<iGenerosService, GenerosService>();
builder.Services.AddScoped<iEstadosMultasService, EstadosMultasService>();

//Administración
builder.Services.AddScoped<iRolesService, RolesService>();
builder.Services.AddScoped<iMenusService, MenusService>();
builder.Services.AddScoped<iCargosService, CargosService>();
builder.Services.AddScoped<iUsuariosService, UsuariosService>();
builder.Services.AddScoped<iLinkRolMenuService, LinkRolMenuService>();
builder.Services.AddScoped<iDatosPersonalesService, DatosPersonalesService>();

//Inventario
builder.Services.AddScoped<iAutoresService, AutoresService>();
builder.Services.AddScoped<iGenerosLiterariosService, GenerosLiterariosService>();
builder.Services.AddScoped<iEditorialesService, EditorialesService>();
builder.Services.AddScoped<iLibrosService, LibrosService>();
builder.Services.AddScoped<iAutoresLibrosService, AutoresLibrosService>();
builder.Services.AddScoped<iGenerosLibrosService, GenerosLibrosService>();
builder.Services.AddScoped<iSeccionesService, SeccionesService>();
builder.Services.AddScoped<iEstanteriasService, EstanteriasService>();
builder.Services.AddScoped<iNivelesService, NivelesService>();
builder.Services.AddScoped<iUbicacionesService, UbicacionesService>();

//Procesos
builder.Services.AddScoped<iPrestamosService, PrestamosService>();
builder.Services.AddScoped<iMultasService, MultasService>();

//Configuración de JWT
builder.Services.AddAuthentication()
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["JWT:JWT_ISSUER_TOKEN"],
        ValidAudience = builder.Configuration["JWT:JWT_AUDIENCE_TOKEM"],

        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:JWT_SECRET_KEY"]))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Usuario", policy => policy.RequireRole("Usuario"));
    options.AddPolicy("Empleado", policy => policy.RequireRole("Empleado"));
    options.AddPolicy("Administrador", policy => policy.RequireRole("Administrador"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("SysBiblioteca_Policy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();