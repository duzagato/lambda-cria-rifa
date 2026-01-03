using System.Text.Json.Serialization;

namespace LambdaCriaRifa.Application.Models;

public class CriaRifaRequest
{
    [JsonPropertyName("titulo")]
    public string Titulo { get; set; } = string.Empty;
    
    [JsonPropertyName("descricao")]
    public string Descricao { get; set; } = string.Empty;
    
    [JsonPropertyName("valorBilhete")]
    public decimal ValorBilhete { get; set; }
    
    [JsonPropertyName("quantidadeBilhetes")]
    public int QuantidadeBilhetes { get; set; }
    
    [JsonPropertyName("dataSorteio")]
    public DateTime DataSorteio { get; set; }
    
    [JsonPropertyName("criadoPor")]
    public string CriadoPor { get; set; } = string.Empty;
}
