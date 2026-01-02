namespace LambdaCriaRifa.Models;

/// <summary>
/// DTO for creating a lottery based on the SQS message payload.
/// </summary>
public class CreateLotteryRequest
{
    /// <summary>
    /// Name of the lottery.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Number of tickets per ticketbook.
    /// </summary>
    public int NumTicketsTicketbook { get; set; }

    /// <summary>
    /// Number of ticketbooks.
    /// </summary>
    public int NumTicketbooks { get; set; }

    /// <summary>
    /// Price per ticket.
    /// </summary>
    public decimal PriceTicket { get; set; }

    /// <summary>
    /// Indicates if the lottery has double chance.
    /// </summary>
    public bool DoubleChance { get; set; }
}
