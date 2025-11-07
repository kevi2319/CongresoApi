using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ====== CONFIGURACIÓN DE CORS ======
var allowedOrigins = new[]
{
    "http://localhost:5173",
    "http://localhost:3000",
    "https://congresapifront.netlify.app"
};

var frontendOrigin = Environment.GetEnvironmentVariable("FRONTEND_ORIGIN");
if (!string.IsNullOrWhiteSpace(frontendOrigin))
{
    allowedOrigins = allowedOrigins.Append(frontendOrigin).ToArray();
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("front", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ====== CONEXIÓN A POSTGRES ======
var conn =
    Environment.GetEnvironmentVariable("DATABASE_URL") ??
    builder.Configuration.GetConnectionString("postgres") ??
    Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");

if (string.IsNullOrWhiteSpace(conn))
    throw new InvalidOperationException(
        "❌ No se encontró cadena de conexión a PostgreSQL"
    );

// ====== EF CORE (Npgsql) ======
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(conn));

// ====== MVC + SWAGGER ======
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ====== SWAGGER ======
app.UseSwagger();
app.UseSwaggerUI();

// ====== CORS ======
app.UseCors("front");

// ====== MIGRACIONES AUTOMÁTICAS ======
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// ====== ENDPOINTS ======
app.MapGet("/", () => Results.Redirect("/swagger"));
app.MapControllers();

// ====== EJECUCIÓN ======
app.Run();