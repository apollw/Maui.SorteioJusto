using Maui.SorteioJusto.Models;
using Maui.SorteioJusto.Services.Interfaces;
using Maui.SorteioJusto.ViewModels;

namespace Maui.SorteioJusto.Pages;

public partial class PageTime : ContentPage
{
    private readonly IRepositoryJogador _rpJogador;
    private readonly IRepositoryTime    _rpTime;
    ViewModelTime    VMTime = new ViewModelTime();

    public PageTime(IRepositoryJogador rpJogador, IRepositoryTime rpTime)
    {
        InitializeComponent();
        _rpJogador     = rpJogador;
        _rpTime        = rpTime;
        VMTime         = new ViewModelTime(_rpJogador, _rpTime);
        BindingContext = VMTime;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs e)
    {
        base.OnNavigatedTo(e);
        VMTime.CarregarListaDeJogadores();
        VMTime.CarregarListaDeTimes();
    }

    private async void Button_SortearTimes(object sender, EventArgs e)
    {
        //await Navigation.PushAsync(new PageTimeJogadores(_rpJogador,_rpTime));
        await Navigation.PushAsync(new PageTimeJogadores(VMTime));
    }

    private async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var button = sender as ImageButton;
        var jogador = button?.BindingContext as Jogador;

        //if (jogador != null)
        //{
        //    string message = "Deseja excluir " + jogador.Nome + "?";
        //    bool confirmar = await DisplayAlert("Aviso", message, "Sim", "Não");

        //    if (confirmar)
        //    {
        //        VMJogador.ListaDeJogadores.Remove(jogador);
        //        VMJogador.ExcluirJogador(jogador);
        //    }
        //}
    }

    private async void Button_ExcluirTimes(object sender, EventArgs e)
    {
        string message = "Deseja limpar a lista?";
        bool confirmar = await DisplayAlert("Aviso", message, "Sim", "Não");

        if (confirmar)
        {
            List<Time> listaParaExclusao = new List<Time>(VMTime.ListaDeTimes);
            List<TimeJogador> listaTimeJogadorParaExclusao = new List<TimeJogador>();

            //Exclui todos os times da lista
            VMTime.ListaDeTimes.Clear();
            await _rpTime.DeleteListaDeTimesAsync(listaParaExclusao);

            //Exclui todas as elações com jogadores
            listaTimeJogadorParaExclusao = await _rpTime.GetTimeJogadoresAsync();
            await _rpTime.DeleteListaDeTimeJogadorAsync(listaTimeJogadorParaExclusao);
        }        
        
    }
}