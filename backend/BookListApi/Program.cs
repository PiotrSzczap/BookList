using Microsoft.EntityFrameworkCore;
using BookListApi.Data;
using BookListApi.Services;
using Azure.Storage.Blobs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add Cosmos DB
var cosmosConnectionString = builder.Configuration.GetConnectionString("CosmosDb");
if (string.IsNullOrEmpty(cosmosConnectionString))
{
    throw new InvalidOperationException("CosmosDb connection string is not configured.");
}

builder.Services.AddDbContext<BookDbContext>(options =>
    options.UseCosmos(
        connectionString: cosmosConnectionString,
        databaseName: "BookListDB"));

// Add Azure Blob Storage
var blobConnectionString = builder.Configuration.GetConnectionString("BlobStorage");
if (string.IsNullOrEmpty(blobConnectionString))
{
    throw new InvalidOperationException("BlobStorage connection string is not configured.");
}

builder.Services.AddSingleton(x => new BlobServiceClient(blobConnectionString));
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();

// Add health checks
builder.Services.AddHealthChecks();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200", "http://localhost:4201", "http://localhost:80", "http://frontend")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BookDbContext>();
    await DatabaseSeeder.SeedAsync(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowAngularApp");

// Map health checks
app.MapHealthChecks("/health");

// Map controllers
app.MapControllers();

app.Run();
