using System.Text;
using Microsoft.OpenApi.Models;
using SysBiblioteca.API.dbContext;
using Microsoft.IdentityModel.Tokens;
using SysBiblioteca.API.Services.ADM.RolesService;
using SysBiblioteca.API.Services.ADM.UsuariosService;
using SysBiblioteca.API.Services.ADM.DatosPersonalesService;
using SysBiblioteca.API.Services.CTL.EstadosService;
using SysBiblioteca.API.Services.ADM.MenusService;
using SysBiblioteca.API.Services.ADM.LinkRolMenuService;

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

//Administración
builder.Services.AddScoped<iRolesService, RolesService>();
builder.Services.AddScoped<iDatosPersonalesService, DatosPersonalesService>();
builder.Services.AddScoped<iUsuariosService, UsuariosService>();
builder.Services.AddScoped<iMenusService, MenusService>();
builder.Services.AddScoped<iLinkRolMenuService, LinkRolMenuService>();

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