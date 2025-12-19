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
            CREATE TABLE IF NOT EXISTS `Speler` (
              `Id` INTEGER PRIMARY KEY AUTOINCREMENT,
              `Naam` VARCHAR(50) NOT NULL
            );
            CREATE TABLE IF NOT EXISTS `SpelerLevel` (
              `SpelerId` INTEGER NOT NULL,
              `LevelId` INTEGER NOT NULL,
              `Topscore` INTEGER NOT NULL,
              PRIMARY KEY (SpelerId, LevelId)
              FOREIGN KEY (SpelerId) REFERENCES Speler(Id),
              FOREIGN KEY (LevelId) REFERENCES Standaardlevel(Id)
            );
            ";
            await cmd.ExecuteNonQueryAsync();
        }
    }

}