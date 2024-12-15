using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using Serilog;
using VFXFinancial.WebApi.Data;
using VFXFinancial.WebApi.Infrastructure.External;
using VFXFinancial.WebApi.Infrastructure.Messaging;
using VFXFinancial.WebApi.Infrastructure.ThirdParty;

var builder = WebApplication.CreateBuilder(args);

// Configure EF Core with SQL Server
builder.Services.AddDbContext<VFXFinancialDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VFXFinancialDB")));

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

// Add the publisher service
builder.Services.AddSingleton<IConnection>(sp =>
{
    var factory = new ConnectionFactory
    {
        HostName = "localhost", // RabbitMQ hostname
        Port = 5672,            // RabbitMQ port
        UserName = "guest",     // RabbitMQ username
        Password = "guest"      // RabbitMQ password
    };

    return factory.CreateConnection(); // Create RabbitMQ connection
});

builder.Services.AddSingleton<IModel>(sp =>
{
    var connection = sp.GetRequiredService<IConnection>();
    return connection.CreateModel(); // Create a RabbitMQ channel
});

// Add the publisher service
builder.Services.AddSingleton<RabbitMQPublisher>();

// Register AlphaVantage API client
builder.Services.AddHttpClient<IAlphaVantageClient>(client =>
{
    client.BaseAddress = new Uri("https://www.alphavantage.co");
});

// Add controllers
builder.Services.AddControllers();

// Configuração do Swagger para documentação da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add logging
// Configure Serilog using the settings from appsettings.json
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // Read configuration
    .CreateLogger();

builder.Host.UseSerilog(); // Use Serilog for logging

var app = builder.Build();

// Apply migrations automatically in development
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<VFXFinancialDbContext>();
    dbContext.Database.Migrate(); // Applies any pending migrations
}

// Configuração do pipeline de middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseRouting();

app.MapControllers();

app.Run();