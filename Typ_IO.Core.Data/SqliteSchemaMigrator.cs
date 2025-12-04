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
              `Id` INT UNSIGNED NOT NULL,
              `Naam` VARCHAR(50) NOT NULL,
              `Tekst` VARCHAR(2000) NOT NULL,
              `Moeilijkheidsgraad` INT UNSIGNED NOT NULL,
            PRIMARY KEY (`Id`));
            CREATE TABLE IF NOT EXISTS `Oefenlevel` (
              `Id` INT UNSIGNED NOT NULL,
              `Naam` VARCHAR(50) NOT NULL,
              `Letteropties` VARCHAR(2000) NOT NULL,
            PRIMARY KEY (`Id`));
            CREATE UNIQUE INDEX IF NOT EXISTS `Id_Standaardlevel_UNIQUE` ON Standaardlevel(Id);
            CREATE UNIQUE INDEX IF NOT EXISTS `Id_Oefenlevel_UNIQUE` ON Oefenlevel(Id);
            ";
            await cmd.ExecuteNonQueryAsync();
        }
    }

}