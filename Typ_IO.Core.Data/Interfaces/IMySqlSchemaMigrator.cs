using System;
using System.Data;
namespace Typ_IO.Core.Data
{
    public interface IMySqlSchemaMigrator
    {
        Task MigrateAsync();
    }
}