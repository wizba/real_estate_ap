using Microsoft.OpenApi.Models;
using RealEstateAPI.Models;
using RealEstateAPI.Repositories;
using RealEstateAPI.Services;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using CloudinaryDotNet;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Amazon.Extensions.NETCore.Setup;

var builder = WebApplication.CreateBuilder(args);

//Config AWS
builder.Services.AddDefaultAWSOptions(new AWSOptions
{
    Region = Amazon.RegionEndpoint.USEast1
});

builder.Services.AddAWSService<IAmazonSimpleSystemsManagement>();

// Add services to the container
builder.Services.AddControllers();

// Function to get SSM parameter
async Task<string> GetSsmParameter(IAmazonSimpleSystemsManagement ssmClient, string paramName)
{
    try
    {
        var response = await ssmClient.GetParameterAsync(new GetParameterRequest
        {
            Name = paramName,
            WithDecryption = true
        });
        return response.Parameter.Value;
    }
    catch (Exception ex)
    {
        throw new InvalidOperationException($"Failed to get parameter {paramName}: {ex.Message}");
    }
}

// Get SSM client
var ssmClient = builder.Services.BuildServiceProvider()
    .GetRequiredService<IAmazonSimpleSystemsManagement>();

// Fetch Cloudinary configuration from SSM
var cloudName = await GetSsmParameter(ssmClient, "/RealEstate/Cloudinary/CloudName");
var apiKey = await GetSsmParameter(ssmClient, "/RealEstate/Cloudinary/ApiKey");
var apiSecret = await GetSsmParameter(ssmClient, "/RealEstate/Cloudinary/ApiSecret");

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
//var cloudinarySection = builder.Configuration.GetSection("CloudinarySettings");

//builder.Services.AddSingleton(cloudinary =>
//{
//    var cloudName = Environment.GetEnvironmentVariable(cloudinarySection["CloudName"]);
//    var apiKey = Environment.GetEnvironmentVariable(cloudinarySection["ApiKey"]);
//    var apiSecret = Environment.GetEnvironmentVariable(cloudinarySection["ApiSecret"]);

//    return new Cloudinary(new Account(cloudName, apiKey, apiSecret));
//});

// Configure Cloudinary
builder.Services.AddSingleton(cloudinary =>
    new Cloudinary(new Account(cloudName, apiKey, apiSecret))
);

// Get database connection from SSM
var connectionString = await GetSsmParameter(ssmClient, "/RealEstate/Database/RemoteConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Database connection string not found in SSM.");
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
