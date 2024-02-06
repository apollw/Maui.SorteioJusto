using SQLite;

namespace Maui.SorteioJusto.Models
{
    [Table("PartidaJogador")]
    public class PartidaJogador
    {
        private int     _partida    = 0;
        private int     _jogador    = 0;
        private int     _isTimeCasa = 0;
        private Jogador _objJogador = new Jogador();

        public int Partida { get => _partida; set => _partida = value; }
        public int Jogador { get => _jogador; set => _jogador = value; }
        public int IsTimeCasa { get => _isTimeCasa; set => _isTimeCasa = value; }

        [Ignore]
        public Jogador ObjJogador { get => _objJogador; set => _objJogador = value; }
    }
}
