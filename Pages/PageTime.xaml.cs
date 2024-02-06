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

    private async void Button_SortearTimes(object sender, EventArgs e)
    {
        //await Navigation.PushAsync(new PageTimeJogadores(_rpJogador,_rpTime));
        await Navigation.PushAsync(new PageTimeJogadores(VMTime));
    }

    private async void MenuListaSorteio(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PageTimeLista());
    }

}