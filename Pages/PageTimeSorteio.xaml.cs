using Maui.SorteioJusto.ViewModels;

namespace Maui.SorteioJusto.Pages;

public partial class PageTimeSorteio : ContentPage
{
    ViewModelTime VMTime = new ViewModelTime();

    public PageTimeSorteio(ViewModelTime vmTime)
    {
        InitializeComponent();
        VMTime = vmTime;
        BindingContext = VMTime;
    }

    //protected override async void OnNavigatedFrom(NavigatedFromEventArgs e)
    //{
    //    base.OnNavigatedFrom(e);
    //    await Task.WhenAll(VMTime.CarregarListaDeTimes());
    //}

    private async void SortearTimes(object sender, EventArgs e)
    {
        await Task.WhenAll(VMTime.SortearTimes());

        if (VMTime.IsCriacaoFinalizada)
        {
            await DisplayAlert("Alerta", "Times Sorteados com Sucesso!", "Concluir");
            await Navigation.PopToRootAsync();
        }
    }

    private void OnStepperValueChanged(object sender, ValueChangedEventArgs e)
    {
        VMTime.TamanhoDaEquipe = (int)e.NewValue;
    }

}