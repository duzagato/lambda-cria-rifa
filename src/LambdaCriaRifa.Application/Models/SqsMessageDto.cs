using System.Text.Json.Serialization;

namespace LambdaCriaRifa.Application.Models;

public class SqsMessageDto
{
    [JsonPropertyName("messageId")]
    public string MessageId { get; set; } = string.Empty;
    
    [JsonPropertyName("body")]
    public string Body { get; set; } = string.Empty;
}
