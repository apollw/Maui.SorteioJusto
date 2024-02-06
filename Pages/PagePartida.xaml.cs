using Maui.SorteioJusto.Services.Interfaces;
using Maui.SorteioJusto.ViewModels;

namespace Maui.SorteioJusto.Pages;

public partial class PagePartida : ContentPage
{
    private readonly IRepositoryTime    _rpTime;
    private readonly IRepositoryJogador _rpJogador;
    private readonly IRepositoryPartida _rpPartida;
    ViewModelPartida VMPartida = new ViewModelPartida();

    public PagePartida(IRepositoryJogador rpJogador, IRepositoryTime rpTime, IRepositoryPartida rpPartida)
    {
        InitializeComponent();

        _rpTime        = rpTime;
        _rpJogador     = rpJogador;
        _rpPartida     = rpPartida; 
        VMPartida      = new ViewModelPartida(_rpJogador, _rpTime, _rpPartida);
        BindingContext = VMPartida;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs e)
    {
        base.OnNavigatedTo(e);
        VMPartida.CarregarListaTimes();
        VMPartida.CarregarListaPartida();
    }


    private async void Button_AdicionarPartida(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PagePartidaRegistro(VMPartida));
    }
}