using CommunityToolkit.Mvvm.ComponentModel;
using Maui.SorteioJusto.Models;
using Maui.SorteioJusto.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Maui.SorteioJusto.ViewModels
{
    public partial class ViewModelJogador : ObservableObject
    {
        //Acesso aos Dados
        private readonly IRepositoryTime    _rpTime;
        private readonly IRepositoryJogador _rpJogador;
        private readonly IRepositoryPartida _rpPartida;

        //Variáveis 
        private bool _isCadastrado;
        [ObservableProperty]
        private string _entryNome     = String.Empty;
        [ObservableProperty]
        private string _entryTelefone = String.Empty;
        [ObservableProperty]
        private Jogador _objJogador;
        [ObservableProperty]
        private Jogador _objJogadorOriginal;
        [ObservableProperty]
        private ObservableCollection<Jogador> _listaDeJogadores;

        public bool IsCadastrado { get => _isCadastrado; set => _isCadastrado = value; }

        public ViewModelJogador()
        {
            
        }

        public ViewModelJogador(IRepositoryJogador rpJogador, IRepositoryTime rpTime, IRepositoryPartida rpPartida)
        {
            _rpJogador = rpJogador;
            _rpTime    = rpTime;
            _rpPartida = rpPartida;

            ObjJogador       = new Jogador();
            ListaDeJogadores = new ObservableCollection<Jogador>();

            CarregarLista();
        }        

        private async Task<bool> VerificarJogador(Jogador jogador) 
        {
            bool result = true;
            List<Jogador> listaTempJogadores = new List<Jogador>();
            listaTempJogadores.AddRange(ListaDeJogadores);

            try
            {
                if (string.IsNullOrEmpty(jogador.Nome))
                {
                    result = false;
                    throw new Exception("Nome não informado");
                }
                if (string.IsNullOrEmpty(jogador.Telefone))
                {
                    result = false;
                    throw new Exception("Telefone não informado");
                }

                foreach (Jogador element in listaTempJogadores)
                {
                    if (element.Telefone == jogador.Telefone && jogador.Id != element.Id)
                    {
                        result = false;
                        throw new Exception("Telefone já registrado!");
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", ex.Message, "Fechar");
            }       

            return result;
        }

        public async void SalvarJogador(Jogador jogadorNovo)
        {
            ObjJogador        = new Jogador(jogadorNovo);
            bool isEdicao     = false;
            bool isVerificado = await VerificarJogador(ObjJogador);

            //Verifica se é Edição
            foreach (Jogador element in ListaDeJogadores)
            {
                if (element.Id == ObjJogador.Id && isVerificado)
                {
                    isEdicao = true;
                    ListaDeJogadores.Remove(element);
                    ListaDeJogadores.Add(ObjJogador);
                    break;
                }
            }

            //Salva ObjJogador
            if (isVerificado&&!isEdicao)
            {
                //Criação de Id
                if (ListaDeJogadores.Count == 0)
                    ObjJogador.Id = 1;
                else
                {
                    int ultimoIdUtilizado = ListaDeJogadores.Max(time => time.Id);
                    int novoId = ultimoIdUtilizado + 1;
                    ObjJogador.Id = novoId;
                }
            }

            //Salva na memória se não existir ainda
            if (isVerificado && !isEdicao)
            {
                IsCadastrado = true;
                ListaDeJogadores.Add(ObjJogador);
                await _rpJogador.AddJogadorAsync(ObjJogador);
            }

            //Salva o jogador editado
            if (isVerificado && isEdicao)
            {
                IsCadastrado = true;
                await _rpJogador.UpdateJogadorAsync(ObjJogador);
            }
        }

        public async void ExcluirJogador(Jogador jogador)
        {
            await ExcluirJogadorVinculado(jogador.Id);
            await _rpJogador.DeleteJogadorAsync(jogador.Id);
        }

        private async Task ExcluirJogadorVinculado(int jogadorId)
        {
            //Ao excluir jogador, excluir tanto da tabela TimeJogador quanto da PartidaJogador
            List<TimeJogador>    listaTimeJogador    = await _rpTime.GetTimeJogadoresAsync();
            List<PartidaJogador> listaPartidaJogador = await _rpPartida.GetPartidaJogadoresAsync();

            foreach (TimeJogador element in listaTimeJogador)
            {
                if (element.Jogador == jogadorId)
                {
                    await _rpTime.DeleteTimeJogadorAsync(element.Id);
                    break;
                }
            }

            foreach (PartidaJogador element in listaPartidaJogador)
            {
                if (element.Jogador == jogadorId)
                {
                    await _rpPartida.DeletePartidaJogadorAsync(element.Id);
                    break;
                }
            }
        }

        public async void CarregarLista()
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
    }
}
