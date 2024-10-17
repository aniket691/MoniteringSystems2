using MoniteringFrontend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Register VehicleService and configure HttpClient for it
builder.Services.AddHttpClient<VehicleService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5011/api/"); // Set your API base URL
});

// Register other services (like OcrService, if needed)
builder.Services.AddHttpClient<OcrService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5011/api/"); // Set base URL for OCR API if it's a separate service
});

// Add Swagger for API documentation (only for the backend)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger only in the Development environment
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Set up Blazor and fallback for routing
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Run the application
app.Run();
