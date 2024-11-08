using Microsoft.OpenApi.Models;
using RealEstateAPI.Models;
using RealEstateAPI.Repositories;
using RealEstateAPI.Services;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using CloudinaryDotNet;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Configure Swagger for API documentation
builder.Services.AddEndpointsApiExplorer(); // Enables API explorer for Swagger
builder.Services.AddSwaggerGen(c =>
{
    // Add basic Swagger documentation
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Real Estate API",
        Version = "v1",
        Description = "An API for managing real estate clients, sellers, and properties.",
        Contact = new OpenApiContact
        {
            Name = "Your Name",
            Email = "your.email@example.com",
            Url = new Uri("https://yourwebsite.com")
        }
    });

    // You can add more customization here, e.g., XML comments for methods, authentication, etc.
});

// TODO Move this to a seperate file
// Load environment variables from the .env file
Env.Load();
// Configure Cloudinary using environment variables
var cloudinarySection = builder.Configuration.GetSection("CloudinarySettings");

builder.Services.AddSingleton(cloudinary =>
{
    var cloudName = Environment.GetEnvironmentVariable(cloudinarySection["CloudName"]);
    var apiKey = Environment.GetEnvironmentVariable(cloudinarySection["ApiKey"]);
    var apiSecret = Environment.GetEnvironmentVariable(cloudinarySection["ApiSecret"]);

    return new Cloudinary(new Account(cloudName, apiKey, apiSecret));
});

// Get connection string from environment variables
var connectionString = Environment.GetEnvironmentVariable("REMOTE_DATABASE_CONNECTION");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Remote database connection string is not set in environment variables.");
}

builder.Services.AddDbContext<RealEstateContext>(options =>
    options.UseNpgsql(connectionString));


// Register repositories and services
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<ISellerRepository, SellerRepository>();
builder.Services.AddScoped<ISellerService, SellerService>();

builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IImageService, ImageService>();

var app = builder.Build();

// Enable Swagger and Swagger UI middleware
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Real Estate API v1");
    });
//}

// app.UseHttpsRedirection();
app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseAuthorization();
app.MapControllers();
app.Run();
