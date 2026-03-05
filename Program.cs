using DAL;
using BLL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// DI - Postgres Helper
builder.Services.AddSingleton<PostgresHelper>();

// DI - DAL
builder.Services.AddScoped<ClienteDAL>();
builder.Services.AddScoped<ProductoDAL>();
builder.Services.AddScoped<PedidoDAL>();

// DI - BLL
builder.Services.AddScoped<ClienteBLL>();
builder.Services.AddScoped<ProductoBLL>();
builder.Services.AddScoped<PedidoBLL>();

// Auth
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "EstaEsUnaLlaveSuperSecretaDe32Caracteres!");
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
