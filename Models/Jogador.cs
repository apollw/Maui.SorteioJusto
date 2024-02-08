using SQLite;

namespace Maui.SorteioJusto.Models
{
    [Table("Jogador")]
    public class Jogador
    {
        private int    _id             = 0;
        private string _nome           = string.Empty;
        private string _telefone       = string.Empty;
        private int    _posicao        = 0;
        private int    _status         = 0;
        private double _classificacao  = 0;
        private int    _golsNaCarreira = 0;

        public Jogador()
        {
            
        }

        public Jogador(Jogador jogador)
        {
            this.Id             = jogador.Id;
            this.Nome           = jogador.Nome;
            this.Telefone       = jogador.Telefone;
            this.Posicao        = jogador.Posicao;
            this.Status         = jogador.Status;
            this.Classificacao  = jogador.Classificacao;
            this.GolsNaCarreira = jogador.GolsNaCarreira;
        }

        [PrimaryKey, NotNull]
        public int Id { get => _id; set => _id = value; }
        [MaxLength(20)]
        public string Nome { get => _nome; set => _nome = value; }
        [MaxLength(15)]
        public string Telefone { get => _telefone; set => _telefone = value; }        
        public int    Posicao { get => _posicao; set => _posicao        = value; }
        public int    Status { get => _status; set => _status         = value; }
        public int    GolsNaCarreira { get => _golsNaCarreira; set => _golsNaCarreira = value; }
        public double Classificacao { get => _classificacao;  set => _classificacao  = value; }
    }
}
