using System.Reflection;
using Jobvelina.Application.Interfaces;
using Jobvelina.Application.Services;
using Jobvelina.Persistence.Data;
using Jobvelina.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configuration flag to choose between mock and database service
var useMockData = builder.Configuration.GetValue<bool>("UseMockData", true);

if (useMockData)
{
    // Use mock service for development/testing
    builder.Services.AddSingleton<IJobApplicationRepository, MockJobApplicationService>();
}
else
{
    // Use Entity Framework with SQL Server LocalDB
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    
    builder.Services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();
}

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Jobvelina API",
        Version = "v1",
        Description = "A simple API for managing job applications",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Jobvelina Team",
            Email = "contact@jobvelina.com"
        }
    });

    // Include XML comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Jobvelina API v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at apps root
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
