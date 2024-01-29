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

        [PrimaryKey, NotNull]
        public int Partida { get => _partida; set => _partida = value; }
        [PrimaryKey,NotNull]
        public int Jogador { get => _jogador; set => _jogador = value; }
        public int IsTimeCasa { get => _isTimeCasa; set => _isTimeCasa = value; }
        public Jogador ObjJogador { get => _objJogador; set => _objJogador = value; }
    }
}
