using Dapper;
using Microsoft.Data.Sqlite;
using Parcial1_P4_Breidy.Models;

namespace Parcial1_P4_Breidy.Services;

public class NumberService(IConfiguration config)
{
private readonly string _connectionString =
config.GetConnectionString("DefaultConnection")
?? "Data Source=numeros.db";

private SqliteConnection CreateConnection =>
    new SqliteConnection(_connectionString);

public async Task InitializeAsync()
{
    const string query = """
        CREATE TABLE IF NOT EXISTS Numeros (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Fecha TEXT NOT NULL,
            Numero INTEGER NOT NULL,
            Resultado INTEGER NOT NULL
        );
        """;

    using var connection = CreateConnection;

    await connection.ExecuteAsync(query);
}

public async Task<bool> SaveAsync(NumberRecordSet number)
{
    const string query = """
        INSERT INTO Numeros (Fecha, Numero, Resultado)
        VALUES (DATETIME('now'), @Numero, @Resultado);
        """;

    using var connection = CreateConnection;

    int rowsAffected = await connection.ExecuteAsync(query, number);

    return rowsAffected > 0;
}

public async Task<NumberRecordGet?> GetByIdAsync(int id)
{
    const string query = """
        SELECT Id, Fecha, Numero, Resultado
        FROM Numeros
        WHERE Id = @Id;
        """;

    using var connection = CreateConnection;

    return await connection.QueryFirstOrDefaultAsync<NumberRecordGet>(
        query,
        new { Id = id }
    );
}

public async Task<IEnumerable<NumberRecordGet>> GetListAsync()
{
    const string query = """
        SELECT Id, Fecha, Numero, Resultado
        FROM Numeros;
        """;

    using var connection = CreateConnection;

    return await connection.QueryAsync<NumberRecordGet>(query);
}

}