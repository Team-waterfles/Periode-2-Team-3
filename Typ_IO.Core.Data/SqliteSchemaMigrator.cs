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
            CREATE TABLE IF NOT EXISTS `Level` (
              `Id` INT UNSIGNED NOT NULL,
              `Naam` VARCHAR(50) NOT NULL,
              `Tekst` VARCHAR(2000) NOT NULL,
              `Moeilijkheidsgraad` ENUM('MAKKELIJK', 'NORMAAL', 'MOEILIJK') NOT NULL,
              `Type` ENUM('OEFENING', 'EXAMEN') NOT NULL,
              `NummerBestand` VARCHAR(100) NULL,
              `MinimumScore` INT UNSIGNED NULL,
            PRIMARY KEY (`Id`),
            UNIQUE INDEX `Id_UNIQUE` (`Id` ASC));
            ";
            await cmd.ExecuteNonQueryAsync();
        }
    }

}