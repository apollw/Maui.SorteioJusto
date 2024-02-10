using Maui.SorteioJusto.Models;
using Maui.SorteioJusto.ViewModels;

namespace Maui.SorteioJusto.Pages;

public partial class PageTimeSelecao : ContentPage
{
	ViewModelTime VMTime = new ViewModelTime();

	public PageTimeSelecao(ViewModelTime vmTime)
	{
		InitializeComponent();
		VMTime = vmTime;
		BindingContext = VMTime;		
	}

    private async void Button_TimeSelecionado(object sender, EventArgs e)
    {
        var button = sender as Button;
        var time   = button?.BindingContext as Time;

        if (time != null)
        {
            VMTime.TimeParaEdicao2 = time;
            await Navigation.PushAsync(new PageTimeEdicao(VMTime));
        }
    }
}