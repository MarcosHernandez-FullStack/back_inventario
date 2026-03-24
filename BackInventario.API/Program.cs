using System.Text;
using System.Threading.RateLimiting;
using BackInventario.API.Hubs;
using BackInventario.Application.Interfaces;
using BackInventario.Application.Services;
using BackInventario.Infrastructure.Data;
using BackInventario.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// DB
var connStr = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddSingleton(new DbConnectionFactory(connStr));

// Repositories
builder.Services.AddScoped<IAuthRepository,     AuthRepository>();
builder.Services.AddScoped<IUsuarioRepository,  UsuarioRepository>();
builder.Services.AddScoped<ICategoriaRepository,CategoriaRepository>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IReporteRepository,  ReporteRepository>();

// Services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<ProductoService>();
builder.Services.AddScoped<ReporteService>();

// JWT
var jwtKey = builder.Configuration["Jwt:Key"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = builder.Configuration["Jwt:Issuer"],
            ValidAudience            = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddSignalR();

// Rate Limiting — máximo 5 intentos de login por minuto por IP
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("login", opt =>
    {
        opt.PermitLimit      = 5;
        opt.Window           = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit       = 0;
    });
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

// CORS
builder.Services.AddCors(options =>
    options.AddPolicy("FrontPolicy", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()));

var app = builder.Build();

// Security headers
app.Use(async (ctx, next) =>
{
    ctx.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    ctx.Response.Headers.Append("X-Frame-Options", "DENY");
    ctx.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
    ctx.Response.Headers.Append("Referrer-Policy", "no-referrer");
    await next();
});

app.UseRateLimiter();
app.UseCors("FrontPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<NotificacionHub>("/hubs/notificaciones");

app.Run();
