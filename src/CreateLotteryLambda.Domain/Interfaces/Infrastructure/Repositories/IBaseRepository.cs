using CreateLotteryLambda.Domain.Models.DTOs;

namespace CreateLotteryLambda.Domain.Interfaces.Infrastructure.Repositories;

public interface IBaseRepository<T> where T : class
{
    Task<T?> SelectById(Guid? id);
    Task<IEnumerable<T>> SelectAll();
    Task Insert(DTO dto);
    Task Update(DTO dto);
    Task DeleteById(Guid id);
    Task<int> SelectCountById(Guid id);
    Task IdExists(Guid? id);
}
