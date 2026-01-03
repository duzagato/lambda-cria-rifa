namespace LambdaCriaRifa.Domain.Models;

public class Rifa
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal ValorBilhete { get; set; }
    public int QuantidadeBilhetes { get; set; }
    public DateTime DataSorteio { get; set; }
    public DateTime DataCriacao { get; set; }
    public string Status { get; set; } = "Ativa";
    public string CriadoPor { get; set; } = string.Empty;
}
