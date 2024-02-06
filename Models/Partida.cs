using SQLite;

namespace Maui.SorteioJusto.Models
{
    [Table("Partida")]
    public class Partida
    {
        private int                  _id                  = 0;
        private int                  _timeCasa            = 0;
        private int                  _timeVisitante       = 0;
        private DateTime             _data                = DateTime.Now;
        private int                  _timeVencedor        = 0;
        private int                  _timeCasaGols        = 0;
        private int                  _timeVisitanteGols   = 0;
        private List<PartidaJogador> _listaPartidaJogador = new List<PartidaJogador>();

        [PrimaryKey,NotNull]
        public int Id { get => _id; set => _id = value; }
        public int TimeCasa { get => _timeCasa; set => _timeCasa = value; }
        public int TimeVisitante { get => _timeVisitante; set => _timeVisitante = value; }
        public DateTime Data { get => _data; set => _data = value; }
        public int TimeVencedor { get => _timeVencedor; set => _timeVencedor = value; }
        public int TimeCasaGols { get => _timeCasaGols; set => _timeCasaGols = value; }
        public int TimeVisitanteGols { get => _timeVisitanteGols; set => _timeVisitanteGols = value; }

        [Ignore]
        public List<PartidaJogador> ListaPartidaJogador { get => _listaPartidaJogador; set => _listaPartidaJogador = value; }

        public List<Jogador> GetListaJogadores()
        {
            List<Jogador> ListaDeJogadores = new List<Jogador>();

            foreach (PartidaJogador element in ListaPartidaJogador)
                ListaDeJogadores.Add(element.ObjJogador);

            return ListaDeJogadores;
        }
    }
}
