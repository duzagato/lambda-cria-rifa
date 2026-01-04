using CreateLotteryLambda.Domain.Entities;
using CreateLotteryLambda.Domain.Interfaces.Infrastructure.Repositories;
using System.Data;

namespace CreateLotteryLambda.Infrastructure.Repositories;

public class LotteryRepository : BaseRepository<Lottery>, ILotteryRepository
{
    public LotteryRepository(IDbConnection connection) : base(connection) 
    { 
    }
}
