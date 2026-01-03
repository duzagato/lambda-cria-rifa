using System.Text.Json;
using LambdaCriaRifa.Application.Models;
using LambdaCriaRifa.Domain.Models;
using LambdaCriaRifa.Domain.Services;
using Microsoft.Extensions.Logging;

namespace LambdaCriaRifa.Application.Handlers;

public class CriaRifaHandler
{
    private readonly RifaService _rifaService;
    private readonly ILogger<CriaRifaHandler> _logger;

    public CriaRifaHandler(RifaService rifaService, ILogger<CriaRifaHandler> logger)
    {
        _rifaService = rifaService;
        _logger = logger;
    }

    public async Task<bool> HandleAsync(string messageBody)
    {
        try
        {
            _logger.LogInformation("Processando mensagem para criar rifa");

            var request = JsonSerializer.Deserialize<CriaRifaRequest>(messageBody);
            
            if (request == null)
            {
                _logger.LogError("Falha ao deserializar mensagem");
                return false;
            }

            var rifa = new Rifa
            {
                Titulo = request.Titulo,
                Descricao = request.Descricao,
                ValorBilhete = request.ValorBilhete,
                QuantidadeBilhetes = request.QuantidadeBilhetes,
                DataSorteio = request.DataSorteio,
                CriadoPor = request.CriadoPor
            };

            var rifaCriada = await _rifaService.CriarRifaAsync(rifa);

            _logger.LogInformation(
                "Rifa criada com sucesso. ID: {RifaId}, Título: {Titulo}", 
                rifaCriada.Id, 
                rifaCriada.Titulo
            );

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar mensagem para criar rifa");
            return false;
        }
    }
}
