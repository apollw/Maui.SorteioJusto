using CommunityToolkit.Mvvm.ComponentModel;
using Maui.SorteioJusto.Models;
using Maui.SorteioJusto.Services.Implementations;
using Maui.SorteioJusto.Services.Interfaces;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace Maui.SorteioJusto.ViewModels
{
    public partial class ViewModelTime : ObservableObject
    {
        private readonly IRepositoryTime    _rpTime;
        private readonly IRepositoryJogador _rpJogador;

        private bool _islistaCarregada    = false;
        private bool _isCriacaoFinalizada = false;
        private bool _isAptoParaSorteio   = false;
        private int  _tamanhoDaEquipe     = 2;
        private int  _quantidadeDeTimes   = 0;

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
        private List<Jogador> _listaGeralDeJogadores;

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
        {
                
        }

        public ViewModelTime(IRepositoryJogador rpJogador, IRepositoryTime rpTime)
        {
            ObjTime                   = new Time();
            ObjJogador                = new Jogador();
            ListaDeTroca1             = new List<Jogador>();
            ListaDeTroca2             = new List<Jogador>();
            ListaDeJogadoresPresentes = new List<Jogador>();
            ListaDeTimes              = new ObservableCollection<Time>();

            _rpJogador     = rpJogador;
            _rpTime        = rpTime;

            CarregarListaDeJogadores();
            CarregarListaDeTimes();
        }

        public async Task SortearTimes()
        {
            //Reseta a Lista Atual de Times
            ListaDeTimes = new ObservableCollection<Time>();
            ListaDeJogadoresPresentes = ListaDeJogadores.Where(j => j.Status == 1).ToList();
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
                TamanhoDaEquipe = 0;
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
                    timeGen.Nome = "Time " + j;
                    timeGen.TotalJogadores = TamanhoDaEquipe;

                    //Após esse processo, adicionamos o time na lista geral de times                    
                    ListaDeTimes.Add(timeGen);

                    //Adicionar associação de Time + Jogador na Lista
                    foreach (Jogador element in timeGen.ListaJogadores)
                    {
                        TimeJogador timeJogador = new TimeJogador();
                        timeJogador.Time        = timeGen.Id;
                        timeJogador.Jogador     = element.Id;
                        //Salvar tabela de TimeJogador do timeGen no Banco
                        await _rpTime.AddTimeJogadorAsync(timeJogador);
                    }

                    //Salvar timeGen no Banco
                    await _rpTime.AddTimeAsync(timeGen);
                    j++;
                } while (j <= QuantidadeDeTimes);

                //Criar lista de espera com os jogadores que sobraram                                            
                if (listaTemporaria.Count != 0)
                {
                    Time timeEspera = new Time();
                    timeEspera.ListaJogadores = new List<Jogador>();

                    for (int i = 0; i < listaTemporaria.Count; i++)
                    {
                        Jogador jogador = new Jogador();
                        jogador.Id = 0;
                        jogador.Nome = string.Empty;
                        jogador.Telefone = string.Empty;
                        jogador.Classificacao = 0;
                        timeEspera.ListaJogadores.Add(jogador);
                    }

                    do
                    {
                        Random rnd = new Random();
                        int indice = rnd.Next(listaTemporaria.Count - 1);
                        Jogador element = listaTemporaria[indice];

                        listaTemporaria.RemoveAt(indice);
                        timeEspera.ListaJogadores[k - 1] = element;
                        k++; //Adicionou um jogador
                    }
                    while (listaTemporaria.Count != 0);

                    //Criação de Id
                    if (ListaDeTimes.Count == 0)
                    {
                        timeEspera.Id = 1;
                    }
                    else
                    {
                        int ultimoIdUtilizado = ListaDeTimes.Max(time => time.Id);
                        int novoId = ultimoIdUtilizado + 1;
                        timeEspera.Id = novoId;
                    }
                    timeEspera.Nome = "Time " + j;
                    timeEspera.TotalJogadores = TamanhoDaEquipe;

                    //Após esse processo, adicionamos o time na lista geral de times                    
                    ListaDeTimes.Add(timeEspera);

                    foreach (Jogador element in timeEspera.ListaJogadores)
                    {
                        TimeJogador timeJogador = new TimeJogador();
                        timeJogador.Time        = timeEspera.Id;
                        timeJogador.Jogador     = element.Id;
                        await _rpTime.AddTimeJogadorAsync(timeJogador);
                    }

                    //Salvar timeEspera no Banco
                    await _rpTime.AddTimeAsync(timeEspera);
                }

                IsCriacaoFinalizada = true;

                ////Após salvar os times, atualiza o status de presença de todos os jogadores
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

    }
}
