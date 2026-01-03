using System.Text.Json;
using LambdaCriaRifa.Application.Handlers;
using LambdaCriaRifa.Application.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LambdaCriaRifa.Application.Workers;

public class SqsWorker : BackgroundService
{
    private readonly CriaRifaHandler _criaRifaHandler;
    private readonly ILogger<SqsWorker> _logger;
    private readonly string _mockMessagesPath;

    public SqsWorker(
        CriaRifaHandler criaRifaHandler,
        ILogger<SqsWorker> logger)
    {
        _criaRifaHandler = criaRifaHandler;
        _logger = logger;
        _mockMessagesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MockData", "sqs-messages.json");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("SQS Worker iniciado. Processando mensagens mockadas...");

        // Verificar se o arquivo de mensagens mockadas existe
        if (!File.Exists(_mockMessagesPath))
        {
            _logger.LogError("Arquivo de mensagens mockadas não encontrado: {Path}", _mockMessagesPath);
            return;
        }

        // Ler mensagens mockadas do arquivo JSON
        var jsonContent = await File.ReadAllTextAsync(_mockMessagesPath, stoppingToken);
        var messages = JsonSerializer.Deserialize<List<SqsMessageDto>>(jsonContent);

        if (messages == null || messages.Count == 0)
        {
            _logger.LogWarning("Nenhuma mensagem mockada encontrada");
            return;
        }

        _logger.LogInformation("Encontradas {Count} mensagens para processar", messages.Count);

        // Processar cada mensagem
        foreach (var message in messages)
        {
            if (stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Processamento cancelado");
                break;
            }

            _logger.LogInformation("Processando mensagem ID: {MessageId}", message.MessageId);

            var success = await _criaRifaHandler.HandleAsync(message.Body);

            if (success)
            {
                _logger.LogInformation("Mensagem {MessageId} processada com sucesso", message.MessageId);
            }
            else
            {
                _logger.LogError("Falha ao processar mensagem {MessageId}", message.MessageId);
            }

            // Simular um pequeno delay entre mensagens
            await Task.Delay(1000, stoppingToken);
        }

        _logger.LogInformation("Todas as mensagens foram processadas. Worker finalizado.");
    }
}
