using LambdaCriaRifa.Models;

namespace LambdaCriaRifa.Services;

/// <summary>
/// Service interface for lottery operations.
/// </summary>
public interface ILotteryService
{
    /// <summary>
    /// Creates a new lottery.
    /// </summary>
    /// <param name="request">The lottery creation request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CreateLotteryAsync(CreateLotteryRequest request, CancellationToken cancellationToken = default);
}
