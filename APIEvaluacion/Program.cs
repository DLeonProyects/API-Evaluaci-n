// Usings: importamos dependencias del proyecto
using APIEvaluacion.Data;
using APIEvaluacion.DTOs;
using APIEvaluacion.Helpers;
using APIEvaluacion.Models;
using APIEvaluacion.Services;
using APIEvaluacion.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;



// Constructor de la aplicación
var builder = WebApplication.CreateBuilder(args);


// Configuración de EF Core InMemory para usar nuestra base de datos en memoria.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("InMemoryDb"));


// Configuración JWT
// Obtenemos la configuración desde appsettings.json
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);


// Configuramos la autenticación con JWT Bearer
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Para pruebas locales, no requiere HTTPS, obviamente
    options.SaveToken = true; // Guarda el token en el contexto de la solicitud
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true, // Validamos la firma del emisor
        IssuerSigningKey = new SymmetricSecurityKey(key), // Clave secreta para validar la firma
        ValidateIssuer = true, // Validamos que el emisor sea el esperado
        ValidateAudience = true, // Validamos que el público sea el esperado
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        ClockSkew = TimeSpan.Zero // No permitimos desfase horario.
    };
});




// Configuracion de controladores
builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddHttpClient();



// Inyeccion de dependencias
builder.Services.AddScoped<IJwtHelper, JwtHelper>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ExternalApiService>();
builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserValidator>();


//Swagger para realizar pruebas a la API facilmente.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();


// Configuramos Swagger solo para ambientes de desarrollo como en este caso.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
