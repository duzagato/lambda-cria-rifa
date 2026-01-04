using System.ComponentModel.DataAnnotations;

namespace CreateLotteryLambda.Domain.Models.DTOs.Lottery;

public class CreateLotteryDTO : DTO
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public int NumTicketsTicketbook { get; set; }

    [Required]
    public int NumTicketbooks { get; set; }

    [Required]
    public decimal PriceTicket { get; set; }

    [Required]
    public bool DoubleChance { get; set; }
}
