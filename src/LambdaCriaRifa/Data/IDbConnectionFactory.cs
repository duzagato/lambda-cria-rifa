using System.Data;

namespace LambdaCriaRifa.Data;

/// <summary>
/// Factory interface for creating database connections.
/// </summary>
public interface IDbConnectionFactory
{
    /// <summary>
    /// Creates a new database connection.
    /// </summary>
    /// <returns>A new database connection instance.</returns>
    IDbConnection CreateConnection();
}
