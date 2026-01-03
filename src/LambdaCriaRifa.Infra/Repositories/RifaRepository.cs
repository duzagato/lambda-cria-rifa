using LambdaCriaRifa.Domain.Interfaces;
using LambdaCriaRifa.Domain.Models;
using LambdaCriaRifa.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace LambdaCriaRifa.Infra.Repositories;

public class RifaRepository : IRifaRepository
{
    private readonly AppDbContext _context;

    public RifaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Rifa> CreateAsync(Rifa rifa)
    {
        await _context.Rifas.AddAsync(rifa);
        await _context.SaveChangesAsync();
        return rifa;
    }

    public async Task<Rifa?> GetByIdAsync(Guid id)
    {
        return await _context.Rifas.FindAsync(id);
    }

    public async Task<IEnumerable<Rifa>> GetAllAsync()
    {
        return await _context.Rifas.ToListAsync();
    }

    public async Task<bool> UpdateAsync(Rifa rifa)
    {
        _context.Rifas.Update(rifa);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var rifa = await _context.Rifas.FindAsync(id);
        if (rifa == null)
        {
            return false;
        }

        _context.Rifas.Remove(rifa);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }
}
