namespace LambdaCriaRifa.Application.Models;

public class CriaRifaRequest
{
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal ValorBilhete { get; set; }
    public int QuantidadeBilhetes { get; set; }
    public DateTime DataSorteio { get; set; }
    public string CriadoPor { get; set; } = string.Empty;
}
