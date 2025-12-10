namespace Typ_IO.Core.Models
{
    public class Standaardlevel
    {
        public Standaardlevel() { }
        public int Id { get; private set; }
        public string Naam { get; set; }
        public string Tekst { get; set; }
        public int Moeilijkheidsgraad { get; set; }
    }
}
