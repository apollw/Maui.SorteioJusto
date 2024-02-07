using CommunityToolkit.Mvvm.ComponentModel;
using Maui.SorteioJusto.Models;
using Maui.SorteioJusto.Services.Interfaces;
using System.Collections.ObjectModel;

namespace Maui.SorteioJusto.ViewModels
{
    public partial class ViewModelTime : ObservableObject
    {
        private readonly IRepositoryTime    _rpTime;
        private readonly IRepositoryJogador _rpJogador;

        private bool _islistaCarregada         = false;
        private bool _isCriacaoFinalizada      = false;
        private bool _isAptoParaSorteio        = false;
        private int  _tamanhoDaEquipe          = 2;
        private int  _quantidadeDeTimes        = 0;

        [ObservableProperty]
        private int _qtdJogadoresSelecionados;
        [ObservableProperty]
        private Time          _objTime;
        [ObservableProperty]
        private Jogador       _objJogador;

        [ObservableProperty]
        private List<Jogador> _listaDeJogadoresPresentes;
        [ObservableProperty]
        private List<Jogador> _listaDeTroca1;
        [ObservableProperty]
        private List<Jogador> _listaDeTroca2;

        [ObservableProperty]
        private ObservableCollection<Time>    _listaDeTimes;
        [ObservableProperty]
        private ObservableCollection<Jogador> _listaDeJogadores;

        public bool IsCriacaoFinalizada { get => _isCriacaoFinalizada; set => _isCriacaoFinalizada = value; }
        public bool IsListaCarregada    { get => _islistaCarregada;    set => _islistaCarregada    = value; }
        public bool IsAptoParaSorteio   { get => _islistaCarregada;    set => _islistaCarregada    = value; }
        public int  TamanhoDaEquipe     { get => _tamanhoDaEquipe;     set => _tamanhoDaEquipe     = value; }
        public int  QuantidadeDeTimes   { get => _quantidadeDeTimes;   set => _quantidadeDeTimes   = value; }

        public ViewModelTime()
        { }

        public ViewModelTime(IRepositoryJogador rpJogador, IRepositoryTime rpTime)
        {
            ObjTime                      = new Time();
            ObjJogador                   = new Jogador();
            ListaDeTimes                 = new ObservableCollection<Time>();
            ListaDeJogadores             = new ObservableCollection<Jogador>();
            ListaDeTroca1                = new List<Jogador>();
            ListaDeTroca2                = new List<Jogador>();
            ListaDeJogadoresPresentes    = new List<Jogador>();
            
            _rpJogador = rpJogador;
            _rpTime    = rpTime;
        }

        public async Task SortearTimes()
        {
            ListaDeJogadoresPresentes     = ListaDeJogadores.Where(j => j.Status == 1).ToList();
            List<Jogador> listaTemporaria = new List<Jogador>();

            listaTemporaria = new List<Jogador>(ListaDeJogadoresPresentes);

            //Embaralhar listaTemporaria
            Random rng = new Random();
            listaTemporaria = new List<Jogador>(listaTemporaria.OrderBy(x => rng.Next()));

            //Resgata o valor do picker e calcula a quantidade de times a ser gerada
            if (TamanhoDaEquipe != 0 && ListaDeJogadoresPresentes != null)
                QuantidadeDeTimes = ListaDeJogadoresPresentes.Count / TamanhoDaEquipe;

            if (QuantidadeDeTimes >= 1)
            {
                IsAptoParaSorteio = true;
            }
            else
            {
                IsAptoParaSorteio = false;
                TamanhoDaEquipe   = 2;
            }

            if (IsAptoParaSorteio)
            {
                int k = 1;
                int j = 1;

                //Apaga Lista antiga de Times
                await Task.WhenAll(ExcluirListaDeTimes());
                await Task.WhenAll(ExcluirListaDeTimeJogador());

                do
                {
                    Time timeGen = new Time();

                    //Criação de Id
                    if (ListaDeTimes.Count == 0)
                        timeGen.Id = 1;
                    else
                    {
                        int ultimoIdUtilizado = ListaDeTimes.Max(time => time.Id);
                        int novoId = ultimoIdUtilizado + 1;
                        timeGen.Id = novoId;
                    }

                    //Inicializa a propriedade ListaJogadores pela quantidade de jogadores fornecida
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

                    // Cria lista de TimeJogador
                    List<TimeJogador> listaTimeJogador = new List<TimeJogador>();

                    //Laço interno para adicionar jogadores a cada time
                    do
                    {
                        Random rnd      = new Random();
                        int indice      = rnd.Next(listaTemporaria.Count - 1);
                        Jogador element = listaTemporaria[indice];

                        listaTemporaria.RemoveAt(indice);
                        timeGen.ListaJogadores[k - 1] = element;                      

                        k++; //Adicionou um jogador
                    }
                    while (k <= TamanhoDaEquipe);
                    k = 1;//Reinicia o ciclo de adicionar jogadores

                    timeGen.Nome = "Time " + j;
                    timeGen.TotalJogadores = TamanhoDaEquipe;

                    //Adicionar associação de Time + Jogador na Lista
                    foreach (Jogador element in timeGen.ListaJogadores)
                    {
                        TimeJogador timeJogador = new TimeJogador();
                        timeJogador.Time        = timeGen.Id;
                        timeJogador.Jogador     = element.Id;
                        await _rpTime.AddTimeJogadorAsync(timeJogador);
                    }

                    //Salvar timeGen no Banco
                    ListaDeTimes.Add(timeGen);
                    await _rpTime.AddTimeAsync(timeGen);
                    j++;
                } while (j <= QuantidadeDeTimes);

                //Criar lista de espera com os jogadores que sobraram                                            
                if (listaTemporaria.Count != 0)
                {
                    Time timeEspera = new Time();
                    
                    int ultimoIdUtilizado = ListaDeTimes.Max(time => time.Id);
                    int novoId            = ultimoIdUtilizado + 1;
                    timeEspera.Id         = novoId;
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
                        timeEspera.ListaJogadores[k - 1] = element;
                        k++; //Adicionou um jogador
                    }
                    while (listaTemporaria.Count != 0);
                    
                    timeEspera.Nome = "Time " + j;
                    timeEspera.TotalJogadores = TamanhoDaEquipe;

                    foreach (Jogador element in timeEspera.ListaJogadores)
                    {
                        TimeJogador timeJogador = new TimeJogador();
                        timeJogador.Time        = timeEspera.Id;
                        timeJogador.Jogador     = element.Id;
                        await _rpTime.AddTimeJogadorAsync(timeJogador);
                    }

                    //Salvar timeEspera no Banco
                    ListaDeTimes.Add(timeEspera);
                    await _rpTime.AddTimeAsync(timeEspera);
                }

                IsCriacaoFinalizada = true;

                //Após salvar os times, atualiza o status de presença de todos os jogadores
                foreach (Jogador element in ListaDeJogadores)
                {
                    element.Status = 0;
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Aviso","Quantidade incorreta de jogadores selecionada","Retornar");
                await Shell.Current.Navigation.PopAsync();
            }
        }

        public async Task ExcluirListaDeTimes()
        {
            List<Time> listaParaExclusao = await _rpTime.GetTimesAsync();
            await _rpTime.DeleteListaDeTimesAsync(listaParaExclusao);
        }

        public async Task ExcluirListaDeTimeJogador()
        {
            List<TimeJogador> listaParaExclusao = await _rpTime.GetTimeJogadoresAsync();
            await _rpTime.DeleteListaDeTimeJogadorAsync(listaParaExclusao);
        }

        public async void CarregarListaDeTimes()
        {
            List<Time>        listaDeTimes       = await _rpTime.GetTimesAsync();
            List<TimeJogador> listaDeTimeJogador = await _rpTime.GetTimeJogadoresAsync();

            if (listaDeTimes != null)
            {
                foreach (Time timeElement in listaDeTimes)
                {
                    if (listaDeTimeJogador != null)
                    {
                        var jogadoresDoTime = listaDeTimeJogador
                            .Where(element => element.Time == timeElement.Id)
                            .Select(async element => await _rpJogador.GetJogadorAsync(element.Jogador));

                        timeElement.ListaJogadores.AddRange(await Task.WhenAll(jogadoresDoTime));
                    }
                }
            }

            if (listaDeTimes != null)
                ListaDeTimes = new ObservableCollection<Time>(listaDeTimes);
        }

        public async void CarregarListaDeJogadores()
        {
            List<Jogador> ListaTemp = await _rpJogador.GetJogadoresAsync();

            if (ListaTemp != null)
            {
                ListaDeJogadores = new ObservableCollection<Jogador>(ListaTemp);
            }
            else
            {
                ListaDeJogadores = new ObservableCollection<Jogador>();
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

        public void AtualizarQtdDeJogadoresSelecionados()
        {
            // Contagem inicial de jogadores selecionados (status 1)
            QtdJogadoresSelecionados = ListaDeJogadores.Count(jogador => jogador.Status == 1);
        }
    }
}
