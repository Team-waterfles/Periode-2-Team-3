using MySqlConnector;
using System.Data;
public interface IMySqlConnectionFactory
{
    Task<MySqlConnection> CreateOpenConnectionAsync();
}