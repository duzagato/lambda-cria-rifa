using System.Data;

namespace CreateLotteryLambda.Domain.Interfaces.Infrastructure.Context;

public interface IDbConnectionContext
{
    IDbConnection CreateConnection();
}
