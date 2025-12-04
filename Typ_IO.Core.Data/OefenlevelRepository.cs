using Typ_IO.Core.Repositories;
using Typ_IO.Core.Models;

namespace Typ_IO.Core.Data
{
    public class OefenlevelRepository : IOefenlevelRepository
    {
        private readonly ISqliteConnectionFactory _factory;
        public OefenlevelRepository(ISqliteConnectionFactory factory) => _factory = factory;

        public async Task AddAsync(Oefenlevel level, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Oefenlevel (Naam, Tekst, Moeilijkheidsgraad) VALUES ($naam, $tekst, $moeilijkheidsgraad); SELECT last_insert_rowid();";
            cmd.Parameters.AddWithValue("$naam", level.Naam);
            cmd.Parameters.AddWithValue("tekst", level.Tekst);
            cmd.Parameters.AddWithValue("$moeilijkheidsgraad", level.Moeilijkheidsgraad);
            var writer = await cmd.ExecuteScalarAsync(ct);
            if (writer is long)
                level.GetType().GetProperty("Id")?.SetValue(level, Convert.ToInt32(writer));
        }

        public async Task<Oefenlevel?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Naam, Tekst, Moeilijkheidsgraad FROM Oefenlevel WHERE Id = $id;";
            cmd.Parameters.AddWithValue("$id", id);
            using var reader = await cmd.ExecuteReaderAsync(ct);
            if (await reader.ReadAsync(ct))
            {
                var level = new Oefenlevel
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

        public async Task<List<Oefenlevel>> ListAsync(CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Naam, Tekst, Moeilijkheidsgraad FROM Oefenlevel ORDER BY Naam;";
            using var reader = await cmd.ExecuteReaderAsync(ct);

            var list = new List<Oefenlevel>();
            while (await reader.ReadAsync(ct))
            {
                var level = new Oefenlevel
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

        public async Task UpdateAsync(Oefenlevel level, CancellationToken ct = default)
        {
            using var conn = await _factory.CreateOpenConnectionAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Oefenlevel SET Naam=$naam, Tekst=$tekst, Moeilijkheidsgraad=$moeilijkheidsgraad WHERE Id=$id;";
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
            cmd.CommandText = "DELETE FROM Oefenlevel WHERE Id=$id;";
            cmd.Parameters.AddWithValue("$id", id);
            await cmd.ExecuteNonQueryAsync(ct);
        }

        public Task SaveChangesAsync(CancellationToken ct = default) => Task.CompletedTask;
    }
}
