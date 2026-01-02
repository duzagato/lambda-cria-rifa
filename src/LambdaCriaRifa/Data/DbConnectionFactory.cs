using System.Data;
using Npgsql;

namespace LambdaCriaRifa.Data;

/// <summary>
/// Factory for creating PostgreSQL database connections.
/// </summary>
public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    /// <summary>
    /// Initializes a new instance of the DbConnectionFactory class.
    /// </summary>
    /// <param name="connectionString">The PostgreSQL connection string.</param>
    public DbConnectionFactory(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    /// <summary>
    /// Creates a new PostgreSQL database connection.
    /// </summary>
    /// <returns>A new NpgsqlConnection instance.</returns>
    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}
