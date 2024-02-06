using CommunityToolkit.Mvvm.ComponentModel;
using Maui.SorteioJusto.Models;
using Maui.SorteioJusto.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Maui.SorteioJusto.ViewModels
{
    public partial class ViewModelPartida : ObservableObject
    {
        private readonly IRepositoryTime    _rpTime;
        private readonly IRepositoryJogador _rpJogador;
        private readonly IRepositoryPartida _rpPartida;

        [ObservableProperty]
        private Partida  _objPartida;
        [ObservableProperty]
        private Partida? _partidaExibida;
        [ObservableProperty]
        private Time?    _timeParaEdicao;

        [ObservableProperty]
        private List<Jogador>  _listaDeAdicao;
        [ObservableProperty]
        private List<Jogador>? _listaDeJogadores;
        [ObservableProperty]
        private List<Time>?    _timesSelecionados;
        [ObservableProperty]
        private List<Time>?    _listaDeTimesTemporaria;

        [ObservableProperty]
        private ObservableCollection<Partida> _listaDePartidas;
        [ObservableProperty]
        private ObservableCollection<Jogador> _listaEditar;
        [ObservableProperty]
        private ObservableCollection<Time>    _listaDeTimes;

        private int      _placar1;
        private int      _placar2;

        private TimeSpan _tempo;
        private TimeSpan _tempoPercorrido;
        private DateTime _dataPartidaEncerrada;
        private readonly IDispatcherTimer _timer;

        private string? _numeroSelecionado1;
        private string? _numeroSelecionado2;
        private object? _timeSelecionado1;
        private object? _timeSelecionado2;

        private bool _isTimerRunning       = false;
        private bool _isTimerPaused        = true;
        private bool _isTimeEditado        = false;
        private bool _isTimeIncompleto     = false;
        private bool _podeIniciarPartida   = true;
        private bool _podeRegistrarPartida = false;
        private bool _isPartidaIniciada    = false;
        private bool _isPartidaRegistrada  = false;

        public int Placar1                   { get => _placar1;              set => _placar1              = value; }
        public int Placar2                   { get => _placar2;              set => _placar2              = value; }
        public TimeSpan Tempo                { get => _tempo;                set => _tempo                = value; }
        public TimeSpan TempoPercorrido      { get => _tempoPercorrido;      set => _tempoPercorrido      = value; }
        public DateTime DataPartidaEncerrada { get => _dataPartidaEncerrada; set => _dataPartidaEncerrada = value; }
        public ICommand ToggleTimerCommand   { get; }
        public IDispatcherTimer Timer => _timer;

        public string NumeroSelecionado1
        {
            get => _numeroSelecionado1;
            set
            {
                _numeroSelecionado1 = value;
                int.TryParse(value?.Replace("numero_", "")?.Replace(".png", ""), out int placar1);
                Placar1 = placar1;
            }
        }
        public string NumeroSelecionado2
        {
            get => _numeroSelecionado2;
            set
            {
                _numeroSelecionado2 = value;
                int.TryParse(value?.Replace("numero_", "")?.Replace(".png", ""), out int placar2);
                Placar2 = placar2;
                OnPropertyChanged(nameof(NumeroSelecionado2));
            }
        }
        public object TimeSelecionado1     { get => _timeSelecionado1;     set => _timeSelecionado1     = value; }
        public object TimeSelecionado2     { get => _timeSelecionado2;     set => _timeSelecionado2     = value; }
        public bool   IsTimerRunning       { get => _isTimerRunning;       set => _isTimerRunning       = value; }
        public bool   IsTimerPaused        { get => _isTimerPaused;        set => _isTimerPaused        = value; }
        public bool   IsTimeEditado        { get => _isTimeEditado;        set => _isTimeEditado        = value; }
        public bool   IsTimeIncompleto     { get => _isTimeIncompleto;     set => _isTimeIncompleto     = value; }
        public bool   PodeIniciarPartida   { get => _podeIniciarPartida;   set => _podeIniciarPartida   = value; }
        public bool   PodeRegistrarPartida { get => _podeRegistrarPartida; set => _podeRegistrarPartida = value; }
        public bool   IsPartidaIniciada    { get => _isPartidaIniciada;    set => _isPartidaIniciada    = value; }
        public bool   IsPartidaRegistrada  { get => _isPartidaRegistrada;  set => _isPartidaRegistrada  = value; }

        public ViewModelPartida()
        { }

        public ViewModelPartida(IRepositoryJogador rpJogador, IRepositoryTime rpTime, IRepositoryPartida rpPartida)
        {
            //_timer         = Application.Current.Dispatcher.CreateTimer();
            _timer           = Shell.Current.Dispatcher.CreateTimer();
            _timer.Interval  = TimeSpan.FromSeconds(1);
            _timer.Tick     += OnTimerElapsed;

            ObjPartida         = new Partida();
            ListaDeAdicao      = new List<Jogador>();
            ListaEditar        = new ObservableCollection<Jogador>();
            ListaDePartidas    = new ObservableCollection<Partida>();
            ToggleTimerCommand = new Command(ToggleTimer);

            _rpTime    = rpTime;
            _rpJogador = rpJogador;
            _rpPartida = rpPartida;
        }

        //Métodos do Timer
        public void StartTimer()
        {
            if (IsPartidaIniciada)
            {
                if (IsTimerRunning)
                {
                    PauseTimer();
                }
                else
                {
                    ResumeTimer();
                }
            }
            else
            {
                //System.Threading.Timer.Start();
                IsPartidaIniciada = true;
                IsTimerRunning    = true;
                IsTimerPaused     = false;
            }
        }

        public void PauseTimer()
        {
            //System.Threading.Timer.Stop();
            TempoPercorrido = Tempo;
            IsTimerRunning  = false;
            IsTimerPaused   = true;
        }

        public void ResumeTimer()
        {
            //System.Threading.Timer.Start();
            IsTimerRunning = true;
            IsTimerPaused  = false;
        }

        public void ResetTimer()
        {
            Tempo = TimeSpan.Zero;
        }

        public void ToggleTimer()
        {
            if (IsPartidaIniciada)
            {
                StartTimer();
            }
        }

        public void OnTimerElapsed(object sender, EventArgs e)
        {
            Tempo = Tempo.Add(TimeSpan.FromSeconds(1));
        }

        //Métodos da Partida
        public bool ValidacaoPartida()
        {
            //Registrar os times selecionados nas CarouselView
            var timesSelecionados = new List<Time>();

            foreach (var time in ListaDeTimes)
            {
                if (TimeSelecionado1 == time)
                {
                    timesSelecionados.Add(time);
                }
                if (TimeSelecionado2 == time)
                {
                    timesSelecionados.Add(time);
                }
            }
            TimesSelecionados = timesSelecionados;

            if (TimesSelecionados.Count < 2)
            {
                throw new Exception("Selecione duas equipes");
            }

            if (TimesSelecionados[0].Id == TimesSelecionados[1].Id)
            {
                throw new Exception("Selecione times diferentes");
            }

            //Caso de times incompletos
            foreach (Time element in TimesSelecionados)
            {
                Time timeParaEdicao = new Time();

                if (element.ListaJogadores.Count < element.TotalJogadores)
                {
                    timeParaEdicao = element;
                    throw new Exception("Um dos times está incompleto");
                }
            }

            return true;
        }

        public async void SalvarPartida()
        {
            try
            {
                IsPartidaRegistrada = false;

                //Finaliza a Partida
                if (ValidacaoPartida())
                {
                    ObjPartida = new Partida();

                    ObjPartida.Id                = 0;
                    ObjPartida.Data              = DateTime.Now;
                    ObjPartida.TimeCasa          = TimesSelecionados[0].Id;
                    ObjPartida.TimeVisitante     = TimesSelecionados[1].Id;
                    ObjPartida.TimeCasaGols      = Placar1;
                    ObjPartida.TimeVisitanteGols = Placar2;

                    if (Placar1 > Placar2)
                        ObjPartida.TimeVencedor = TimesSelecionados[0].Id;
                    else if (Placar2 > Placar1)
                        ObjPartida.TimeVencedor = TimesSelecionados[1].Id;
                    else
                        ObjPartida.TimeVencedor = 0;

                    //Criação de Id
                    if (ListaDePartidas.Count == 0)
                    {
                        ObjPartida.Id = 1;
                    }
                    else
                    {
                        int ultimoIdUtilizado = ListaDePartidas.Max(partida => partida.Id);
                        int novoId = ultimoIdUtilizado + 1;
                        ObjPartida.Id = novoId;
                    }

                    int i = 0;
                    //Adicionar jogadores à partida
                    while (i <= 1) //Devem haver 2 times selecionados sempre
                    {
                        foreach (Jogador element in TimesSelecionados[i].ListaJogadores)
                        {
                            PartidaJogador partidaJogador  = new PartidaJogador();
                            partidaJogador.Partida         = ObjPartida.Id;
                            partidaJogador.Jogador         = element.Id;
                            partidaJogador.ObjJogador.Nome = element.Nome;

                            if (i == 0)
                                partidaJogador.IsTimeCasa = 1;

                            //Salvar tabela de PartidaJogador no Banco
                            await _rpPartida.AddPartidaJogadorAsync(partidaJogador);

                            //Adiciona Jogador à Partida
                            //ObjPartida.ListaPartidaJogador.Add(objPartidaJogador);
                        }
                        i++;
                    }
                    i = 0;

                    DataPartidaEncerrada = DateTime.Now;
                    ListaDePartidas.Add(ObjPartida);

                    //Salvar ObjPartida no Banco
                    await _rpPartida.AddPartidaAsync(ObjPartida);

                    IsPartidaRegistrada = true;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", ex.Message, "Fechar");
            }
        }

        public async void EditarTime(Time timeParaEditar)
        {
            bool _podeEditar         = true;
            int _numeroParaCompletar = timeParaEditar.TotalJogadores - timeParaEditar.ListaJogadores.Count;

            try
            {
                //Não pode Editar se o número for diferente dos que faltam para completar
                if (ListaDeAdicao.Count != _numeroParaCompletar)
                {
                    _podeEditar = false;
                    throw new Exception("Selecione o número correto de jogadores");
                }

                if (_podeEditar)
                {
                    List<Jogador> jogadoresAdicionados = new List<Jogador>();

                    foreach (Time element in ListaDeTimes)
                    {
                        if (element.Id != timeParaEditar.Id)
                        {
                            foreach (Jogador jogador in ListaDeAdicao)
                            {
                                Jogador jogadorExistente = element.ListaJogadores.Find(j => j.Id == jogador.Id);

                                if (jogadorExistente != null)
                                {
                                    Jogador jogadorTemp = new Jogador();

                                    //Remove do time origem
                                    element.ListaJogadores.RemoveAll(j => j.Id == jogador.Id);
                                    jogadorTemp = jogadorExistente;
                                    jogadoresAdicionados.Add(jogadorTemp);
                                }
                            }
                        }
                    }

                    foreach (Time element in ListaDeTimes)
                    {
                        //Após retirar os jogadores, adicinar todos ao time destino
                        if (element.Id == timeParaEditar.Id)
                        {
                            element.ListaJogadores.AddRange(jogadoresAdicionados);
                        }
                    }

                    //Após esse momento, teremos uma lista de times atualizada
                    //Então, apagar a Lista antiga e Salvar a nova no Endpoint

                    //Apaga a lista de times antiga na Base de Dados
                    //IRestResponse responseDelete = CommonApi.DoDeleteWithJson($"{url}/teams/delete", "");

                    //foreach (ModelTime modelTime in ListaDeTimes)
                    //{
                    //    //Adiciona todos os times novos no Endpoint

                    //    //Salvar timeGen em JSON
                    //    var TimeAdd = JsonConvert.SerializeObject(modelTime);
                    //    //Salvar timeGen no Endpoint
                    //    IRestResponse response = CommonApi.DoPostWithJson($"{url}/teams/new", TimeAdd);

                    //}

                    IsTimeEditado = true;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", ex.Message, "Fechar");
            }
        }

        //public List<Jogador> CarregarListaJogadores()
        //{
        //    List<Time>    times     = new List<Time>();
        //    List<Jogador> jogadores = new List<Jogador>();

        //    if (ListaDeJogadores == null)
        //    {
        //        ListaDeTimes = CarregarListaTimes();

        //        foreach (Time element in ListaDeTimes)
        //        {
        //            foreach (Jogador jogador in element.ListaJogadores)
        //            {
        //                Jogador player = new Jogador();
        //                player         = jogador;
        //                jogadores.Add(player);
        //            }
        //        }
        //        return jogadores;
        //    }

        //    return ListaDeJogadores;
        //}

        public List<Jogador> CarregarListaEditar(Time timeParaEdicao)
        {
            List<Jogador>              jogadores = new List<Jogador>();
            ObservableCollection<Time> times     = new ObservableCollection<Time>();

            if (timeParaEdicao != null)
                // Adicionar todos os jogadores da lista de times na lista jogadores
                foreach (Time element in ListaDeTimes)
                {
                    // Verifica se o time é igual ao time em edição
                    if (element.Id != timeParaEdicao.Id)
                    {
                        foreach (Jogador jogador in element.ListaJogadores)
                        {
                            Jogador player = new Jogador();
                            player         = jogador;
                            jogadores.Add(player);
                        }
                    }
                }
            ListaEditar.Clear();

            foreach (var jogador in jogadores)
            {
                ListaEditar.Add(jogador);
            }

            //string jsonedicao = JsonConvert.SerializeObject(jogadores);
            //// Salva a string JSON em um arquivo
            //string filePath2 = Path.Combine(FileSystem.AppDataDirectory, "listaedicao.json");
            //File.WriteAllText(filePath2, jsonedicao);

            return jogadores;
        }

        public async void CarregarListaTimes()
        {
            List<Time> listaDeTimes              = await _rpTime.GetTimesAsync();
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

        public async void CarregarListaPartida()
        {
            List<Partida>        listaDePartidas       = await _rpPartida.GetPartidasAsync();
            List<PartidaJogador> listaDePartidaJogador = await _rpPartida.GetPartidaJogadoresAsync();

            if (listaDePartidas != null)
            {
                foreach (Partida partidaElement in listaDePartidas)
                {
                    if (listaDePartidaJogador != null)
                    {
                        var jogadoresDoTime = listaDePartidaJogador
                            .Where(element => element.Partida == partidaElement.Id)
                            .Select(async element => await _rpJogador.GetJogadorAsync(element.Jogador));

                        partidaElement.GetListaJogadores();

                        //partidaElement.ListaPartidaJogador.AddRange(await Task.WhenAll(jogadoresDoTime));
                    }
                }
            }

            if (listaDePartidas != null)
                ListaDePartidas = new ObservableCollection<Partida>(listaDePartidas);
        }

        public ObservableCollection<Jogador> AtualizarListaEditar()
        {
            return new ObservableCollection<Jogador>();
        }        
    }
}



