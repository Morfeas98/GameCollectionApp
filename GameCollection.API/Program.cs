using GameCollection.Infrastructure;
using GameCollection.Infrastructure.Data;
using GameCollection.Infrastructure.Repositories;
using GameCollection.Domain.Common;
using GameCollection.Application.Common;
using Microsoft.EntityFrameworkCore;
using GameCollection.Domain.Repositories;
using AutoMapper;
using GameCollection.Application.DTOs;
using GameCollection.Domain.Entities;
using GameCollection.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();


// Add DbContextx with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions=> sqlOptions.MigrationsAssembly("GameCollection.Infrastructure")
        ));

// Add AutoMapper
// 1. Δημιουργία του Configuration
var config = new MapperConfiguration(cfg => 
{
    cfg.AddProfile<MappingProfile>();
});

// 2. Δημιουργία του Mapper
var mapper = config.CreateMapper();

// 3. Εγγραφή στο Dependency Injection
builder.Services.AddSingleton(mapper);

// Register Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IGameRepository, GameRepository>();

// Register Services
builder.Services.AddScoped<IGameService, GameService>();

// Add CORS for FrontEnd
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder =>
        {
            builder.WithOrigins("http://localhost:5113")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// Add JSON Options for Circular References
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Game Collection API",
        Version = "v1",
        Description = "API for managing video game collections",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Game Collection Team",
            Email = "contact@gamecollection.com"
        }
    });

    // Add XML comments
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (System.IO.File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});


var app = builder.Build();

// Seed Data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        // Migrate if needed
        context.Database.Migrate();
        SeedData.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occured while seeding the database.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Game Collection API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
