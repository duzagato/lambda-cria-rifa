using LambdaCriaRifa.Domain.Models;

namespace LambdaCriaRifa.Domain.Interfaces;

public interface IRifaRepository
{
    Task<Rifa> CreateAsync(Rifa rifa);
    Task<Rifa?> GetByIdAsync(Guid id);
    Task<IEnumerable<Rifa>> GetAllAsync();
    Task<bool> UpdateAsync(Rifa rifa);
    Task<bool> DeleteAsync(Guid id);
}
