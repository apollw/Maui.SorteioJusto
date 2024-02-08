using SQLite;

namespace Maui.SorteioJusto.Models
{
    [Table("Time")]
    public class Time
    {
        private int           _id               = 0;
        private string        _nome             = string.Empty;
        private int           _nivel            = 0;
        private int           _totalJogadores   = 0;
        private int           _NumeroDeVitorias = 0;
        private int           _NumeroDeEmpates  = 0;
        private int           _NumeroDeDerrotas = 0;
        private List<Jogador> _listaJogadores   = new List<Jogador>();       

        [PrimaryKey,NotNull]
        public int Id { get => _id; set => _id = value; }
        [MaxLength(15)]
        public string Nome { get => _nome; set => _nome = value; }
        public int  Nivel { get => _nivel; set => _nivel = value; }
        public int  TotalJogadores { get => _totalJogadores; set => _totalJogadores = value; }
        public int  NumeroDeVitorias { get => _NumeroDeVitorias; set => _NumeroDeVitorias = value; }
        public int  NumeroDeEmpates { get => _NumeroDeEmpates; set => _NumeroDeEmpates = value; }
        public int  NumeroDeDerrotas { get => _NumeroDeDerrotas; set => _NumeroDeDerrotas = value; }
        public bool IsTimeIncompleto => ListaJogadores.Count != TotalJogadores;

        [Ignore]
        public List<Jogador> ListaJogadores { get => _listaJogadores; set => _listaJogadores = value; }
    }
}
