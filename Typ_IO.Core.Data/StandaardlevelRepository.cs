using Typ_IO.Core.Repositories;
using Typ_IO.Core.Models;

namespace Typ_IO.Core.Data
{
    public class StandaardlevelRepository: IStandaardlevelRepository
    {
        private readonly ISqliteConnectionFactory _factory;
        public StandaardlevelRepository(ISqliteConnectionFactory factory) => _factory = factory;

        public async Task AddAsync(Standaardlevel level, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Standaardlevel (Naam, Tekst, Moeilijkheidsgraad) VALUES ($naam, $tekst, $moeilijkheidsgraad); SELECT last_insert_rowid();";
            cmd.Parameters.AddWithValue("$naam", level.Naam);
            cmd.Parameters.AddWithValue("tekst", level.Tekst);
            cmd.Parameters.AddWithValue("$moeilijkheidsgraad", level.Moeilijkheidsgraad);
            var writer = await cmd.ExecuteScalarAsync(ct);
            if (writer is long)
                level.GetType().GetProperty("Id")?.SetValue(level, Convert.ToInt32(writer));
        }

        public async Task<Standaardlevel?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Naam, Tekst, Moeilijkheidsgraad FROM Standaardlevel WHERE Id = $id;";
            cmd.Parameters.AddWithValue("$id", id);
            using var reader = await cmd.ExecuteReaderAsync(ct);
            if (await reader.ReadAsync(ct))
            {
                var level = new Standaardlevel
                {
                    Naam = reader.GetString(1),
                    Tekst = reader.GetString(2),
                    Moeilijkheidsgraad = reader.GetInt32(3)
                };
                level.GetType().GetProperty("Id")?.SetValue(level, reader.GetInt32(0));
                return level;
            }
            return null;
        }

        public async Task<List<Standaardlevel>> ListAsync(CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Naam, Tekst, Moeilijkheidsgraad FROM Standaardlevel ORDER BY Naam;";
            using var reader = await cmd.ExecuteReaderAsync(ct);

            var list = new List<Standaardlevel>();
            while (await reader.ReadAsync(ct))
            {
                var level = new Standaardlevel
                {
                    Naam = reader.GetString(1),
                    Tekst = reader.GetString(2),
                    Moeilijkheidsgraad = reader.GetInt32(3)
                };
                level.GetType().GetProperty("Id")?.SetValue(level, reader.GetInt32(0));
                list.Add(level);
            }
            return list;
        }

        public async Task<List<Standaardlevel>> ListByDifficltyAsync(int moeilijkheidsgraad, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT Id, Naam, Tekst, Moeilijkheidsgraad FROM Standaardlevel WHERE Moeilijkheidsgraad = {moeilijkheidsgraad} ORDER BY Naam;";
            using var reader = await cmd.ExecuteReaderAsync(ct);

            var list = new List<Standaardlevel>();
            while (await reader.ReadAsync(ct))
            {
                var level = new Standaardlevel
                {
                    Naam = reader.GetString(1),
                    Tekst = reader.GetString(2),
                    Moeilijkheidsgraad = reader.GetInt32(3)
                };
                level.GetType().GetProperty("Id")?.SetValue(level, reader.GetInt32(0));
                list.Add(level);
            }
            return list;
        }

        public async Task UpdateAsync(Standaardlevel level, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Standaardlevel SET Naam=$naam, Tekst=$tekst, Moeilijkheidsgraad=$moeilijkheidsgraad WHERE Id=$id;";
            cmd.Parameters.AddWithValue("$id", level.GetType().GetProperty("Id")?.GetValue(level));
            cmd.Parameters.AddWithValue("$naam", level.Naam);
            cmd.Parameters.AddWithValue("tekst", level.Tekst);
            cmd.Parameters.AddWithValue("$moeilijkheidsgraad", level.Moeilijkheidsgraad);
            await cmd.ExecuteNonQueryAsync(ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Standaardlevel WHERE Id=$id;";
            cmd.Parameters.AddWithValue("$id", id);
            await cmd.ExecuteNonQueryAsync(ct);
        }

        public Task SaveChangesAsync(CancellationToken ct = default) => Task.CompletedTask;
    }
}
