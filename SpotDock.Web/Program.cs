using MassTransit;
using SpotDock.Modules.Billing.Infrastructure.DI;
using SpotDock.Modules.Market.Infrastructure.DI;
using SpotDock.Modules.Compute.Infrastructure.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

// Add DI for modules
builder.Services.AddMarketModule(builder.Configuration);
builder.Services.AddComputeModule(builder.Configuration);
builder.Services.AddBillingModule(builder.Configuration);

// MassTransit + RabbitMQ
builder.Services.AddMassTransit(cfg =>
{
    // TODO: register consumers/sagas from modules when they appear

    cfg.UsingRabbitMq((context, busCfg) =>
    {
        var rabbitSection = builder.Configuration.GetSection("RabbitMq");
        var host = rabbitSection.GetValue<string>("Host") ?? "localhost";
        var virtualHost = rabbitSection.GetValue<string>("VirtualHost") ?? "/";
        var username = rabbitSection.GetValue<string>("Username") ?? "guest";
        var password = rabbitSection.GetValue<string>("Password") ?? "guest";

        busCfg.Host(host, virtualHost, h =>
        {
            h.Username(username);
            h.Password(password);
        });
    });
});

var app = builder.Build();

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