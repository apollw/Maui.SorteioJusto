using SQLite;

namespace Maui.SorteioJusto.Models
{
    [Table("TimeJogador")]
    public class TimeJogador
    {
        private int     _id         = 0;
        private int     _time       = 0;
        private int     _jogador    = 0;
        private Jogador _objJogador = new Jogador();

        [PrimaryKey,NotNull,AutoIncrement]
        public int Id      { get => _id;      set => _id      = value; }   
        public int Time    { get => _time;    set => _time    = value; }
        public int Jogador { get => _jogador; set => _jogador = value; }

        [Ignore]
        public Jogador ObjJogador { get => _objJogador; set => _objJogador = value; }
    }
}
