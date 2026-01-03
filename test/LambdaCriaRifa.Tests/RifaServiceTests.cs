using LambdaCriaRifa.Domain.Models;
using LambdaCriaRifa.Domain.Services;
using LambdaCriaRifa.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace LambdaCriaRifa.Tests;

public class RifaServiceTests
{
    private readonly Mock<IRifaRepository> _mockRepository;
    private readonly Mock<ILogger<RifaService>> _mockLogger;
    private readonly RifaService _rifaService;

    public RifaServiceTests()
    {
        _mockRepository = new Mock<IRifaRepository>();
        _mockLogger = new Mock<ILogger<RifaService>>();
        _rifaService = new RifaService(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task CriarRifaAsync_ComDadosValidos_DeveCriarRifa()
    {
        // Arrange
        var rifa = new Rifa
        {
            Titulo = "Rifa Teste",
            Descricao = "Descrição teste",
            ValorBilhete = 10.0m,
            QuantidadeBilhetes = 100,
            DataSorteio = DateTime.UtcNow.AddDays(30),
            CriadoPor = "teste@exemplo.com"
        };

        _mockRepository
            .Setup(r => r.CreateAsync(It.IsAny<Rifa>()))
            .ReturnsAsync((Rifa r) => r);

        // Act
        var resultado = await _rifaService.CriarRifaAsync(rifa);

        // Assert
        Assert.NotNull(resultado);
        Assert.NotEqual(Guid.Empty, resultado.Id);
        Assert.Equal("Rifa Teste", resultado.Titulo);
        Assert.Equal("Ativa", resultado.Status);
        Assert.True(resultado.DataCriacao <= DateTime.UtcNow);
        _mockRepository.Verify(r => r.CreateAsync(It.IsAny<Rifa>()), Times.Once);
    }

    [Fact]
    public async Task CriarRifaAsync_SemTitulo_DeveLancarExcecao()
    {
        // Arrange
        var rifa = new Rifa
        {
            Titulo = "",
            ValorBilhete = 10.0m,
            QuantidadeBilhetes = 100,
            DataSorteio = DateTime.UtcNow.AddDays(30)
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _rifaService.CriarRifaAsync(rifa));
    }

    [Fact]
    public async Task CriarRifaAsync_ComValorBilheteInvalido_DeveLancarExcecao()
    {
        // Arrange
        var rifa = new Rifa
        {
            Titulo = "Rifa Teste",
            ValorBilhete = 0,
            QuantidadeBilhetes = 100,
            DataSorteio = DateTime.UtcNow.AddDays(30)
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _rifaService.CriarRifaAsync(rifa));
    }

    [Fact]
    public async Task CriarRifaAsync_ComQuantidadeBilhetesInvalida_DeveLancarExcecao()
    {
        // Arrange
        var rifa = new Rifa
        {
            Titulo = "Rifa Teste",
            ValorBilhete = 10.0m,
            QuantidadeBilhetes = 0,
            DataSorteio = DateTime.UtcNow.AddDays(30)
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _rifaService.CriarRifaAsync(rifa));
    }

    [Fact]
    public async Task CriarRifaAsync_ComDataSorteioPassada_DeveLancarExcecao()
    {
        // Arrange
        var rifa = new Rifa
        {
            Titulo = "Rifa Teste",
            ValorBilhete = 10.0m,
            QuantidadeBilhetes = 100,
            DataSorteio = DateTime.UtcNow.AddDays(-1)
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _rifaService.CriarRifaAsync(rifa));
    }

    [Fact]
    public async Task ObterRifaPorIdAsync_DeveRetornarRifa()
    {
        // Arrange
        var rifaId = Guid.NewGuid();
        var rifaEsperada = new Rifa
        {
            Id = rifaId,
            Titulo = "Rifa Teste"
        };

        _mockRepository
            .Setup(r => r.GetByIdAsync(rifaId))
            .ReturnsAsync(rifaEsperada);

        // Act
        var resultado = await _rifaService.ObterRifaPorIdAsync(rifaId);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(rifaId, resultado.Id);
        Assert.Equal("Rifa Teste", resultado.Titulo);
    }

    [Fact]
    public async Task ListarRifasAsync_DeveRetornarListaDeRifas()
    {
        // Arrange
        var rifas = new List<Rifa>
        {
            new Rifa { Id = Guid.NewGuid(), Titulo = "Rifa 1" },
            new Rifa { Id = Guid.NewGuid(), Titulo = "Rifa 2" }
        };

        _mockRepository
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(rifas);

        // Act
        var resultado = await _rifaService.ListarRifasAsync();

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count());
    }
}
