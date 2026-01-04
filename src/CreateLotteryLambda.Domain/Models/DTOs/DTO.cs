using System.Reflection;

namespace CreateLotteryLambda.Domain.Models.DTOs;

public abstract class DTO
{
    public IEnumerable<PropertyInfo> GetProperties(string idColumnName)
    {
        return GetType()
               .GetProperties()
               .Where(p => p.Name != idColumnName);
    }
}
