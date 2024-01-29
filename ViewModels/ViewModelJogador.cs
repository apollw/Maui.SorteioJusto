using CommunityToolkit.Mvvm.ComponentModel;
using Maui.SorteioJusto.Models;
using Maui.SorteioJusto.Services.Interfaces;
using System.Collections.ObjectModel;

namespace Maui.SorteioJusto.ViewModels
{
    public partial class ViewModelJogador : ObservableObject
    {
        private readonly IRepositoryJogador _rpJogador;
        private bool   _isCadastrado;
        [ObservableProperty]
        private string _entryNome     = String.Empty;
        [ObservableProperty]
        private string _entryTelefone = String.Empty;
        [ObservableProperty]
        private Jogador _objJogador;
        [ObservableProperty]
        private ObservableCollection<Jogador> _listaDeJogadores;

        public bool IsCadastrado { get => _isCadastrado; set => _isCadastrado = value; }

        public ViewModelJogador()
        {
            
        }

        public ViewModelJogador(IRepositoryJogador rpJogador)
        {
            _rpJogador       = rpJogador;
            ObjJogador       = new Jogador();
            CarregarLista();
        }

        private int GerarNovoId(int id)
        {
            if (id != 0)
            {
                return id;
            }
            else
            {
                if (ListaDeJogadores.Count == 0)
                {
                    return 1;
                }
                else
                {
                    int ultimoIdUtilizado = ListaDeJogadores.Max(jogador => jogador.Id);
                    int novoId = ultimoIdUtilizado + 1;
                    return novoId;
                }
            }
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
            catch(Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", ex.Message, "Fechar");
            }       

            return result;
        }

        public async void SalvarJogador(Jogador jogador)
        {
            bool isEdicao     = false;
            bool isVerificado = await VerificarJogador(jogador);
            
            //Salva ObjJogador
            if (isVerificado)
            {
                ObjJogador = jogador;
            }
            
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

            //Salva na memória se não existir ainda
            if (isVerificado && !isEdicao)
            {
                ListaDeJogadores.Add(ObjJogador);
                IsCadastrado = true;
                await _rpJogador.AddJogadorAsync(jogador);                
            }
            
            //Salva o jogador editado
            if (isVerificado && isEdicao)
            {
                IsCadastrado = true;
                await _rpJogador.UpdateJogadorAsync(jogador);
            }
        }

        public async void ExcluirJogador(int id)
        {
            await _rpJogador.DeleteJogadorAsync(id);
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
