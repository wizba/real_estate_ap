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
var connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");

// Register DbContext with PostgreSQL
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
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();  // Enable the Swagger JSON endpoint
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Real Estate API v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI as the root page
    });
}

// app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
