namespace LambdaCriaRifa.Application.Models;

public class SqsMessageDto
{
    public string MessageId { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}
