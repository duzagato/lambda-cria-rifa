using LambdaCriaRifa.Application.Handlers;
using LambdaCriaRifa.Application.Workers;
using LambdaCriaRifa.Domain.Interfaces;
using LambdaCriaRifa.Domain.Services;
using LambdaCriaRifa.Infra.Data;
using LambdaCriaRifa.Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

// Configuração
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

// Configurar DbContext com PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Registrar repositórios
builder.Services.AddScoped<IRifaRepository, RifaRepository>();

// Registrar serviços de domínio
builder.Services.AddScoped<RifaService>();

// Registrar handlers
builder.Services.AddScoped<CriaRifaHandler>();

// Registrar worker como Hosted Service
builder.Services.AddHostedService<SqsWorker>();

// Configurar logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

var host = builder.Build();

// Criar banco de dados se não existir
using (var scope = host.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        await context.Database.EnsureCreatedAsync();
        Console.WriteLine("Banco de dados verificado/criado com sucesso.");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Erro ao criar/verificar banco de dados");
        Console.WriteLine($"AVISO: Não foi possível conectar ao banco de dados: {ex.Message}");
        Console.WriteLine("A aplicação continuará, mas as operações de banco falharão.");
    }
}

Console.WriteLine("==============================================");
Console.WriteLine("Lambda Cria Rifa - Aplicação em Camadas");
Console.WriteLine("==============================================");
Console.WriteLine("Pressione Ctrl+C para encerrar");
Console.WriteLine();

await host.RunAsync();
