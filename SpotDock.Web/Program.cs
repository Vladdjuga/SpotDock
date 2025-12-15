using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MassTransit;
using SpotDock.Modules.Auth.Infrastructure.DI;
using SpotDock.Modules.Auth.Infrastructure.Security;
using SpotDock.Modules.Billing.Infrastructure.DI;
using SpotDock.Modules.Market.Infrastructure.DI;
using SpotDock.Modules.Compute.Infrastructure.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();

// JWT Authentication
var jwtOptions = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
    ?? throw new InvalidOperationException("JWT configuration is missing");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
        };
    });

builder.Services.AddAuthorization();

// Add DI for modules
builder.Services.AddAuthModule(builder.Configuration);
builder.Services.AddMarketModule(builder.Configuration);
builder.Services.AddComputeModule(builder.Configuration);
builder.Services.AddBillingModule(builder.Configuration);

// MassTransit + RabbitMQ
builder.Services.AddMassTransit(cfg =>
{
    // Saga orchestrating resource provisioning (funds -> capacity -> job)
    cfg.AddSagaStateMachine<SpotDock.Modules.Market.Application.Sagas.ProvisionComputeStateMachine,
        SpotDock.Modules.Market.Application.Sagas.ProvisionComputeSagaState>()
        .InMemoryRepository();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/health", () => Results.Ok("OK"))
    .WithName("HealthCheck")
    .WithTags("HealthCheck");

app.MapControllers();

app.Run();