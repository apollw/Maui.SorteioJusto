using CommunityToolkit.Mvvm.ComponentModel;
using Maui.SorteioJusto.Models;
using System.Collections.ObjectModel;

namespace Maui.SorteioJusto.ViewModels
{
    public partial class ViewModelTime : ObservableObject
    {
        private bool _istimeCriado      = false;
        private bool _islistaCarregada  = false;
        private int  _tamanhoDaEquipe   = 2;
        private int  _quantidadeDeTimes = 0;

        [ObservableProperty]
        private Time          _objTime;
        [ObservableProperty]
        private Jogador       _objJogador;

        [ObservableProperty]
        private List<Jogador> _listaDeJogadores;
        [ObservableProperty]
        private List<Jogador> _listaDeJogadoresPresentes;
        [ObservableProperty]
        private List<Jogador> _listaDeTroca1;
        [ObservableProperty]
        private List<Jogador> _listaDeTroca2;
        [ObservableProperty]
        private List<Jogador> _listaGeralDeJogadores;

        [ObservableProperty]
        private ObservableCollection<Time> _listaDeTimes;

        public bool IsTimeCriado      { get => _istimeCriado;      set => _istimeCriado      = value; }
        public bool IsListaCarregada  { get => _islistaCarregada;  set => _islistaCarregada  = value; }
        public int  TamanhoDaEquipe   { get => _tamanhoDaEquipe;   set => _tamanhoDaEquipe   = value; }
        public int  QuantidadeDeTimes { get => _quantidadeDeTimes; set => _quantidadeDeTimes = value; }

        public ViewModelTime()
        {
            ObjTime                   = new Time();
            ObjJogador                = new Jogador();
            ListaDeJogadores          = new List<Jogador>();
            ListaDeTroca1             = new List<Jogador>();
            ListaDeTroca2             = new List<Jogador>();
            ListaGeralDeJogadores     = new List<Jogador>();
            ListaDeJogadoresPresentes = new List<Jogador>();
            ListaDeTimes              = new ObservableCollection<Time>();
        }

        public void SortearTimes()
        {
            ListaDeJogadoresPresentes     = ListaDeJogadores.Where(j => j.Status == 1).ToList();
            List<Jogador> listaTemporaria = new List<Jogador>();

            if (ListaDeJogadoresPresentes!=null)
                listaTemporaria = new List<Jogador>(ListaDeJogadoresPresentes);
            
            //Embaralhar listaTemporaria
            Random rng      = new Random();
            listaTemporaria = new List<Jogador>(listaTemporaria.OrderBy(x => rng.Next()));
            
            //Resgata o valor do picker e calcula a quantidade de times a ser gerada
            if (TamanhoDaEquipe != 0 && ListaDeJogadoresPresentes != null)
                QuantidadeDeTimes = ListaDeJogadoresPresentes.Count / TamanhoDaEquipe; 
                //Sorteio é feito com base nos presentes

                if (QuantidadeDeTimes >= 1)
                {
                    int k = 1;
                    int j = 1;

                    // Cria lista de TimeJogador
                    List<TimeJogador> listaTimeJogador = new List<TimeJogador>();

                    // Inicializa a listaTimeJogador
                    for (int i = 0; i < TamanhoDaEquipe; i++)
                    {
                        Jogador jogador     = new Jogador();
                        TimeJogador timeJogador = new TimeJogador();
                        
                        
                        timeJogador.Time        = 0;
                        timeJogador.Jogador     = 0;
                        listaTimeJogador.Add(timeJogador);

                        jogador.Id            = 0;
                        jogador.Nome          = string.Empty;
                        jogador.Telefone      = string.Empty;
                        jogador.Classificacao = 0;
                        jogador.Status        = 0;
                        jogador.Posicao       = 0;
                    }

                    do
                    {
                        Time timeGen = new Time();

                        //Inicializa a propriedade ListaJogador pela quantidade de jogadores fornecida
                        timeGen.ListaJogadores = new List<Jogador>();
                        for (int i = 0; i < TamanhoDaEquipe; i++)
                        {
                            Jogador jogador       = new Jogador();
                            jogador.Id            = 0;
                            jogador.Nome          = string.Empty;
                            jogador.Telefone      = string.Empty;
                            jogador.Classificacao = 0;
                            timeGen.ListaJogadores.Add(jogador);
                        }

                        //Laço interno para adicionar jogadores a cada time
                        do
                        {
                            Random rnd      = new Random();
                            int indice      = rnd.Next(listaTemporaria.Count - 1);
                            Jogador element = listaTemporaria[indice];

                            listaTemporaria.RemoveAt(indice);

                            timeGen.ListaJogadores[k - 1].Id            = element.Id;
                            timeGen.ListaJogadores[k - 1].Nome          = element.Nome;
                            timeGen.ListaJogadores[k - 1].Telefone      = element.Telefone;
                            timeGen.ListaJogadores[k - 1].Classificacao = element.Classificacao;

                            k++; //Adicionou um jogador
                        }
                        while (k <= TamanhoDaEquipe);
                        k = 1;//Reinicia o ciclo de adicionar jogadores

                        //Criação de Id
                        if (ListaDeTimes.Count == 0)
                        {
                            timeGen.Id = 1; // Se a lista está vazia, retorna 1 como o novo ID
                        }
                        else
                        {
                            int ultimoIdUtilizado = ListaDeTimes.Max(time => time.Id);
                            int novoId = ultimoIdUtilizado + 1;
                            timeGen.Id = novoId;
                        }
                        timeGen.Nome           = "Time " + j;
                        timeGen.TotalJogadores = TamanhoDaEquipe;

                        //Após esse processo, adicionamos o time na lista geral de times                    
                        ListaDeTimes.Add(timeGen);
                        j++;

                    } while (j <= QuantidadeDeTimes);
                    IsTimeCriado = true;

                    //Criar lista de espera com os jogadores que sobraram                                            
                    if (listaTemporaria.Count != 0)
                    {
                        Time timeEspera           = new Time();
                        timeEspera.ListaJogadores = new List<Jogador>();

                        for (int i = 0; i < listaTemporaria.Count; i++)
                        {
                            Jogador jogador       = new Jogador();
                            jogador.Id            = 0;
                            jogador.Nome          = string.Empty;
                            jogador.Telefone      = string.Empty;
                            jogador.Classificacao = 0;
                            timeEspera.ListaJogadores.Add(jogador);
                        }

                        do
                        {
                            Random rnd      = new Random();
                            int indice      = rnd.Next(listaTemporaria.Count - 1);
                            Jogador element = listaTemporaria[indice];

                            listaTemporaria.RemoveAt(indice);

                            timeEspera.ListaJogadores[k - 1].Id            = element.Id;
                            timeEspera.ListaJogadores[k - 1].Nome          = element.Nome;
                            timeEspera.ListaJogadores[k - 1].Telefone      = element.Telefone;
                            timeEspera.ListaJogadores[k - 1].Classificacao = element.Classificacao;

                            k++; //Adicionou um jogador
                        }
                        while (listaTemporaria.Count != 0);

                        //Criação de Id
                        if (ListaDeTimes.Count == 0)
                        {
                            timeEspera.Id = 1; // Se a lista está vazia, retorna 1 como o novo ID
                        }
                        else
                        {
                            int ultimoIdUtilizado = ListaDeTimes.Max(time => time.Id);
                            int novoId = ultimoIdUtilizado + 1;
                            timeEspera.Id = novoId;
                        }
                        timeEspera.Nome = "Time " + j;
                        timeEspera.TotalJogadores = TamanhoDaEquipe;

                        ListaDeTimes.Add(timeEspera);
                    }
                }

                //Após salvar os times, atualiza o status de presença de todos os jogadores
                foreach (Jogador element in ListaDeJogadores)
                {
                    element.Status = 0;
                }
            }

            public void AtualizarDadosJogador(Jogador jogador)
            {
                var jogadorExistente = ListaDeJogadores.FirstOrDefault(j => j.Id == jogador.Id);
                if (jogadorExistente != null)
                {
                    jogadorExistente.Status = jogador.Status;
                }
            }

            public List<Jogador> CarregarListaTroca()
            {
                //ObservableCollection<Time> times = new ObservableCollection<Time>(ListaCarregadaDeTimes);
                //List<Jogador> jogadores = times.SelectMany(time => time.ListaJogadores).ToList();
                //List<Jogador> listaAtualizada = new List<Jogador>();

                //// Carregar lista sem jogadores selecionados para troca
                //foreach (Jogador element2 in jogadores)
                //{
                //    if (!ListaParaTroca1.Any(j => j.Id == element2.Id))
                //    {
                //        Jogador objJogador = new Jogador();
                //        objJogador.Id = element2.Id;
                //        objJogador.Nome = element2.Nome;
                //        objJogador.Telefone = element2.Telefone;
                //        objJogador.Status = element2.Status;
                //        objJogador.Classificacao = element2.Classificacao;

                //        listaAtualizada.Add(objJogador);
                //    }
                //}
                ////ListaGeralTroca.AddRange(listaAtualizada);

                return new List<Jogador>();
        }

            public List<Jogador> CarregarListaJogadores()
            {
                return new List<Jogador>();
            }
        }
}
