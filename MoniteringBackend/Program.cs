using Microsoft.EntityFrameworkCore;
using MoniteringBackend.Data;
using MoniteringBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure EF Core and DbContext
builder.Services.AddDbContext<MonitoringContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MonitoringDatabaseConnectionString")));

// Add HttpClient for OCR service
builder.Services.AddHttpClient();

// Add OCR service with API key from configuration
builder.Services.AddScoped<OcrService>(provider =>
{
    var httpClient = provider.GetRequiredService<HttpClient>();
    var apiKey = builder.Configuration["ChatGptApiKey"]; // Ensure you have this key in appsettings.json
    if (string.IsNullOrWhiteSpace(apiKey))
    {
        throw new InvalidOperationException("ChatGptApiKey is not configured in appsettings.json");
    }
    return new OcrService(httpClient, apiKey);
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS policy
app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();
app.UseAuthorization();

// Map controllers
app.MapControllers();

// Run the application
app.Run();
