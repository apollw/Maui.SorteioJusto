using Maui.SorteioJusto.Models;
using Maui.SorteioJusto.Services.Interfaces;
using Maui.SorteioJusto.ViewModels;

namespace Maui.SorteioJusto.Pages;

public partial class PageJogador : ContentPage
{
    ViewModelJogador VMJogador  = new ViewModelJogador();

    public PageJogador(IRepositoryJogador rpJogador)
    {
        InitializeComponent();
        VMJogador      = new ViewModelJogador(rpJogador);
        BindingContext = VMJogador;
    }

    private async void Button_AdicionarJogador(object sender, EventArgs e)
    {
        VMJogador.ObjJogador = new Jogador();

        _btnAdicionarJogador.IsEnabled = false;
        await Navigation.PushAsync(new PageJogadorCadastro(VMJogador));
        _btnAdicionarJogador.IsEnabled = true;
    }

    private async void OnDeleteSwipeItemInvoked(object sender, EventArgs e)
    {
        var swipeItem = sender as SwipeItem;
        var item = swipeItem?.BindingContext as Jogador;

        string message = "Deseja excluir " + item?.Nome + "?";
        bool confirmar = await DisplayAlert("Aviso", message, "Sim", "Nao");

        if (item != null && confirmar)
        {
            VMJogador.ListaDeJogadores.Remove(item);
            VMJogador.ExcluirJogador(item.Id);
        }
    }

    private async void OnEditSwipeItemInvoked(object sender, EventArgs e)
    {
        var swipeItem = sender as SwipeItem;
        var item = swipeItem.BindingContext as Jogador;
        VMJogador.ObjJogador = item;

        await Navigation.PushAsync(new PageJogadorCadastro(VMJogador));
    }
}