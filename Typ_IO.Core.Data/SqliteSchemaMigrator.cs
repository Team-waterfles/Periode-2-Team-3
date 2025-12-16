namespace Typ_IO.Core.Data
{
    public class SqliteSchemaMigrator
    {
        private readonly ISqliteConnectionFactory _factory;
        public SqliteSchemaMigrator(ISqliteConnectionFactory factory) => _factory = factory;

        public async Task MigrateAsync()
        {
            using var conn = await _factory.CreateOpenConnectionAsync();
            using var cmd = conn.CreateCommand();

            cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS `Standaardlevel` (
              `Id` INTEGER PRIMARY KEY AUTOINCREMENT,
              `Naam` VARCHAR(50) NOT NULL,
              `Tekst` VARCHAR(2000) NOT NULL,
              `Moeilijkheidsgraad` INT UNSIGNED NOT NULL
            );
            CREATE TABLE IF NOT EXISTS `Oefenlevel` (
              `Id` INTEGER PRIMARY KEY,
              `Naam` VARCHAR(50) NOT NULL,
              `Letteropties` VARCHAR(2000) NOT NULL
            );
            ";
            await cmd.ExecuteNonQueryAsync();
        }
    }

}