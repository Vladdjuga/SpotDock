using SpotDock.Modules.Auctions.Infrastructure.DI;
using SpotDock.Modules.Compute.Infrastructure.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

// Add DI
builder.Services.AddAuctionsModule(app.Configuration);
builder.Services.AddComputeModule(app.Configuration);
// builder.Services.AddKernelModule(app.Configuration);
// builder.Services.AddBillingModule(app.Configuration);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/health", () => Results.Ok("OK"))
    .WithName("HealthCheck")
    .WithTags("HealthCheck");

app.MapControllers();

app.Run();