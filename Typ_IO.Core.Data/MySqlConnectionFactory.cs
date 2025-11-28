using MySqlConnector;

namespace Typ_IO.Core.Data
{
    public interface IMySqlConnectionFactory
    {
        Task<MySqlConnection> CreateOpenConnectionAsync();
    }
    public class MySqlConnectionFactory : IMySqlConnectionFactory
    {
        private readonly string _connectionString;
        public MySqlConnectionFactory()
        {
            MySqlConnection _connectionString = new MySqlConnection(
                "server=localhost;" +
                "user=speler;" +
                "database=typio;" +
                "port=3306;" +
                "password=typio");
        }
        public async Task<MySqlConnection> CreateOpenConnectionAsync()
        {
            var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            return conn;
        }
    }
}
