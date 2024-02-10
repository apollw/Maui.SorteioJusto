using Maui.SorteioJusto.Models;
using Maui.SorteioJusto.Services.Interfaces;
using Maui.SorteioJusto.ViewModels;
using System.Collections.ObjectModel;

namespace Maui.SorteioJusto.Pages;

public partial class PageJogador : ContentPage
{
    private readonly IRepositoryJogador _rpJogador;
    Jogador _jogadorFiltrado     = new Jogador();
    List<Jogador> _listaFiltrada = new List<Jogador>();
    ViewModelJogador VMJogador   = new ViewModelJogador();
    
    public PageJogador(IRepositoryJogador rpJogador, IRepositoryTime rpTime, IRepositoryPartida rpPartida)
    {
        InitializeComponent();  
        _rpJogador     = rpJogador;
        VMJogador      = new ViewModelJogador(rpJogador, rpTime, rpPartida);
        BindingContext = VMJogador;
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        _listaFiltrada = SearchUsers(((SearchBar)sender).Text);
        VMJogador.ListaFiltrada     = new ObservableCollection<Jogador>(_listaFiltrada);
        _collectionView.ItemsSource = VMJogador.ListaFiltrada;
    }

    private List<Jogador> SearchUsers(string textoFiltro)
    {
        var jogadores = VMJogador.ListaDeJogadores.Where(
                            x => !string.IsNullOrWhiteSpace(x.Nome) &&
                            x.Nome.StartsWith(textoFiltro, StringComparison.OrdinalIgnoreCase)
                        )?.ToList();

        // Se houver apenas um jogador filtrado, armazene-o
        _jogadorFiltrado = (jogadores.Count == 1) ? jogadores[0] : null;

        return jogadores;
    }

    private async void Button_AdicionarJogador(object sender, EventArgs e)
    {
        VMJogador.ObjJogador = new Jogador();

        _btnAdicionarJogador.IsEnabled = false;
        await Navigation.PushAsync(new PageJogadorCadastro(VMJogador));
        _btnAdicionarJogador.IsEnabled = true;
    }

    private async void Button_ExcluirJogadores(object sender, EventArgs e)
    {
        string message = "Deseja limpar a lista?";
        bool confirmar = await DisplayAlert("Aviso", message, "Sim", "Não");

        if (confirmar)
        {
            List<Jogador> listaParaExclusao = new List<Jogador>(VMJogador.ListaDeJogadores);

            //Exclui todos os jogadores da lista
            VMJogador.ListaDeJogadores.Clear();
            await _rpJogador.DeleteListaDeJogadoresAsync(listaParaExclusao);
        }
    }

    private async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var button  = sender as ImageButton;
        var jogador = button?.BindingContext as Jogador;

        if (jogador != null)
        {
            string message = "Deseja excluir " + jogador.Nome + "?";
            bool confirmar = await DisplayAlert("Aviso", message, "Sim", "Não");

            if (confirmar)
            {
                VMJogador.ListaFiltrada.Remove(jogador);
                VMJogador.ListaDeJogadores.Remove(jogador);
                VMJogador.ExcluirJogador(jogador);
            }
        }
    }

    private async void OnEditButtonClicked(object sender, EventArgs e)
    {
        var button = sender as ImageButton;
        var jogador = button?.BindingContext as Jogador;

        if (jogador != null)
        {
            VMJogador.ObjJogador = jogador;
            await Navigation.PushAsync(new PageJogadorCadastro(VMJogador));
        }
    }
}