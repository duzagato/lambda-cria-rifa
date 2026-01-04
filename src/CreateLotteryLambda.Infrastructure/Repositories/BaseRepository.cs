using CreateLotteryLambda.Domain.Helpers;
using CreateLotteryLambda.Domain.Interfaces.Infrastructure.Repositories;
using CreateLotteryLambda.Domain.Models.DTOs;
using Dapper;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;
using System.Text;

namespace CreateLotteryLambda.Infrastructure.Repositories;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly IDbConnection _connection;
    protected readonly string _tableName;
    protected readonly string _schema;
    protected readonly string _idColumnName;

    protected BaseRepository(IDbConnection connection)
    {
        _connection = connection;

        // Get table name and schema from TableAttribute
        var tableAttribute = typeof(T).GetCustomAttribute<TableAttribute>();
        if (tableAttribute != null)
        {
            _tableName = tableAttribute.Name;
            _schema = tableAttribute.Schema ?? "public";
        }
        else
        {
            _tableName = typeof(T).Name.ToLower();
            _schema = "public";
        }

        _idColumnName = "Id";
    }

    public virtual async Task<T?> SelectById(Guid? id)
    {
        var query = $"SELECT * FROM {_schema}.{_tableName} WHERE {StringHelper.PascalToSnakeCase(_idColumnName)} = @Id";
        return await _connection.QueryFirstOrDefaultAsync<T>(query, new { Id = id });
    }

    public virtual async Task<IEnumerable<T>> SelectAll()
    {
        var query = $"SELECT * FROM {_schema}.{_tableName}";
        return await _connection.QueryAsync<T>(query);
    }

    public virtual async Task Insert(DTO dto)
    {
        var properties = dto.GetProperties(_idColumnName);
        var columns = string.Join(", ", properties.Select(p => StringHelper.PascalToSnakeCase(p.Name)));
        var values = string.Join(", ", properties.Select(p => $"@{p.Name}"));

        var query = $"INSERT INTO {_schema}.{_tableName} ({columns}) VALUES ({values})";
        await _connection.ExecuteAsync(query, dto);
    }

    public virtual async Task Update(DTO dto)
    {
        var properties = dto.GetProperties(_idColumnName);
        var setClause = string.Join(", ", properties.Select(p => $"{StringHelper.PascalToSnakeCase(p.Name)} = @{p.Name}"));

        var query = $"UPDATE {_schema}.{_tableName} SET {setClause} WHERE {StringHelper.PascalToSnakeCase(_idColumnName)} = @{_idColumnName}";
        await _connection.ExecuteAsync(query, dto);
    }

    public virtual async Task DeleteById(Guid id)
    {
        var query = $"DELETE FROM {_schema}.{_tableName} WHERE {StringHelper.PascalToSnakeCase(_idColumnName)} = @Id";
        await _connection.ExecuteAsync(query, new { Id = id });
    }

    public virtual async Task<int> SelectCountById(Guid id)
    {
        var query = $"SELECT COUNT(*) FROM {_schema}.{_tableName} WHERE {StringHelper.PascalToSnakeCase(_idColumnName)} = @Id";
        return await _connection.ExecuteScalarAsync<int>(query, new { Id = id });
    }

    public virtual async Task IdExists(Guid? id)
    {
        if (id == null)
        {
            throw new ArgumentException("Id cannot be null");
        }

        var count = await SelectCountById(id.Value);
        if (count == 0)
        {
            throw new KeyNotFoundException($"Entity with id {id} not found in {_tableName}");
        }
    }
}
