using LambdaCriaRifa.Domain.Interfaces;
using LambdaCriaRifa.Domain.Models;
using Microsoft.Extensions.Logging;

namespace LambdaCriaRifa.Domain.Services;

public class RifaService
{
    private readonly IRifaRepository _rifaRepository;
    private readonly ILogger<RifaService> _logger;

    public RifaService(IRifaRepository rifaRepository, ILogger<RifaService> logger)
    {
        _rifaRepository = rifaRepository;
        _logger = logger;
    }

    public async Task<Rifa> CriarRifaAsync(Rifa rifa)
    {
        _logger.LogInformation("Criando nova rifa: {Titulo}", rifa.Titulo);

        // Validações de negócio
        if (string.IsNullOrWhiteSpace(rifa.Titulo))
        {
            throw new ArgumentException("Título da rifa é obrigatório");
        }

        if (rifa.ValorBilhete <= 0)
        {
            throw new ArgumentException("Valor do bilhete deve ser maior que zero");
        }

        if (rifa.QuantidadeBilhetes <= 0)
        {
            throw new ArgumentException("Quantidade de bilhetes deve ser maior que zero");
        }

        if (rifa.DataSorteio <= DateTime.UtcNow)
        {
            throw new ArgumentException("Data do sorteio deve ser futura");
        }

        // Definir valores padrão
        rifa.Id = Guid.NewGuid();
        rifa.DataCriacao = DateTime.UtcNow;
        rifa.Status = "Ativa";

        var rifaCriada = await _rifaRepository.CreateAsync(rifa);
        
        _logger.LogInformation("Rifa criada com sucesso. ID: {RifaId}", rifaCriada.Id);
        
        return rifaCriada;
    }

    public async Task<Rifa?> ObterRifaPorIdAsync(Guid id)
    {
        _logger.LogInformation("Obtendo rifa por ID: {RifaId}", id);
        return await _rifaRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Rifa>> ListarRifasAsync()
    {
        _logger.LogInformation("Listando todas as rifas");
        return await _rifaRepository.GetAllAsync();
    }
}
