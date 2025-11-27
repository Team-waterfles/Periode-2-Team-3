using System;
using MySqlConnector;
using System.Data;
namespace Typ_IO.Core.Data
{
    public interface IMySqlConnectionFactory
    {
        Task<MySqlConnection> CreateOpenConnectionAsync();
    }
}