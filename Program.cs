using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


var frontendOrigin = Environment.GetEnvironmentVariable("FRONTEND_ORIGIN");
builder.Services.AddCors(o => o.AddPolicy("front", p =>
{
    p.WithOrigins(
        "http://localhost:5173",
        "http://localhost:3000"
    )
    .AllowAnyHeader()
    .AllowAnyMethod();

    if (!string.IsNullOrWhiteSpace(frontendOrigin))
        p.WithOrigins(frontendOrigin).AllowAnyHeader().AllowAnyMethod();
}));


var conn =
    Environment.GetEnvironmentVariable("DATABASE_URL")
    ?? builder.Configuration.GetConnectionString("postgres")
    ?? Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");

if (string.IsNullOrWhiteSpace(conn))
    throw new InvalidOperationException(
        "No se encontró cadena de conexión a PostgreSQL en POSTGRES_CONNECTION_STRING / DATABASE_URL / appsettings."
    );

// ---- EF Core (Npgsql) ----
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(conn)
);

// ---- MVC + Swagger ----
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("front");

// ---- Migraciones automáticas al arrancar ----
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Atajo: redirige "/" a swagger
app.MapGet("/", () => Results.Redirect("/swagger"));

app.MapControllers();
app.Run();
