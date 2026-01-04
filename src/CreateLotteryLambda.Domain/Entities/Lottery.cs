using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CreateLotteryLambda.Domain.Entities;

[Table("lottery", Schema = "lottery")]
public class Lottery : Entity
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
