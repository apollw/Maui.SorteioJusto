using Maui.SorteioJusto.Models;
using Maui.SorteioJusto.Services.Interfaces;
using Maui.SorteioJusto.ViewModels;

namespace Maui.SorteioJusto.Pages;

public partial class PageJogador : ContentPage
{
    Jogador _jogadorFiltrado   = new Jogador();
    ViewModelJogador VMJogador = new ViewModelJogador();
    
    public PageJogador(IRepositoryJogador rpJogador, IRepositoryTime rpTime, IRepositoryPartida rpPartida)
    {
        InitializeComponent();        
        VMJogador      = new ViewModelJogador(rpJogador, rpTime, rpPartida);
        BindingContext = VMJogador;
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        _collectionView.ItemsSource = SearchUsers(((SearchBar)sender).Text);
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

    private async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var button = sender as ImageButton;
        var jogador = button?.BindingContext as Jogador;

        if (jogador != null)
        {
            string message = "Deseja excluir " + jogador.Nome + "?";
            bool confirmar = await DisplayAlert("Aviso", message, "Sim", "Não");

            if (confirmar)
            {
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


//private async void OnDeleteSwipeItemInvoked(object sender, EventArgs e)
//{
//    var swipeItem = sender as SwipeItem;
//    var item = swipeItem?.BindingContext as Jogador;

//    string message = "Deseja excluir " + item?.Nome + "?";
//    bool confirmar = await DisplayAlert("Aviso", message, "Sim", "Nao");

//    if (item != null && confirmar)
//    {
//        VMJogador.ListaDeJogadores.Remove(item);
//        VMJogador.ExcluirJogador(item);
//    }
//}

//private async void OnEditSwipeItemInvoked(object sender, EventArgs e)
//{
//    var swipeItem = sender as SwipeItem;
//    var item = swipeItem.BindingContext as Jogador;
//    VMJogador.ObjJogador = item;

//    await Navigation.PushAsync(new PageJogadorCadastro(VMJogador));
//}