using System;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(o => o.AddPolicy("front",
    p => p.WithOrigins("http://localhost:5173", "http://localhost:3000")
          .AllowAnyHeader().AllowAnyMethod()));


builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("sqlite")));

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

// CORS
app.UseCors("front");

app.MapControllers();
app.Run();
