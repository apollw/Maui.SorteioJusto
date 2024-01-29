using SQLite;

namespace Maui.SorteioJusto.Models
{
    [Table("TimeJogador")]
    public class TimeJogador
    {
        private int     _time       = 0;
        private int     _jogador    = 0;
        private Jogador _objJogador = new Jogador();

        [PrimaryKey,NotNull]
        public int Time { get => _time; set => _time = value; }
        [PrimaryKey, NotNull]
        public int Jogador { get => _jogador; set => _jogador = value; }
        public Jogador ObjJogador { get => _objJogador; set => _objJogador = value; }
    }
}
