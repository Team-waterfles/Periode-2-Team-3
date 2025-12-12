namespace Typ_IO.Core.Models
{
    public class Speler(int id, string naam)
    {
        public int Id { get; private set; } = id;
        public string Naam { get; set; } = naam;
    }
}
