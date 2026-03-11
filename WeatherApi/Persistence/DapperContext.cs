using System.Data;
using Npgsql;

namespace WeatherApi.Persistence;

public class DapperContext
{
    private readonly string _connectionString;

    public DapperContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
                            ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found");
    }

    public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
}