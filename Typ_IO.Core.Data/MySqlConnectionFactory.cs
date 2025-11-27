using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySqlConnector;

namespace Typ_IO.Core.Data
{
    public class MySqlConnectionFactory : IMySqlConnectionFactory
    {
        private readonly string _connectionString;
        public MySqlConnectionFactory()
        {
            MySqlConnection _connectionString = new MySqlConnection(
                "server=localhost;" +
                "user=???;" +
                "database=???;" +
                "port=3306;" +
                "password=???");
        }
        public async Task<MySqlConnection> CreateOpenConnectionAsync()
        {
            var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            return conn;
        }
    }
}
