using Maui.SorteioJusto.ViewModels;

namespace Maui.SorteioJusto.Pages;

public partial class PageTime : ContentPage
{
    ViewModelTime VMTime = new ViewModelTime();

    public PageTime()
    {
        InitializeComponent();
        BindingContext = VMTime;
        //_lblCarregando.IsVisible = false;
    }

    private async void Button_SortearTimes(object sender, EventArgs e)
    {
        // Exibir o ActivityIndicator imediatamente após o clique
        //_activity.IsVisible = true;
        //_activity.IsRunning = true;

        try
        {
            //_btnSortear.IsVisible = false;
            //_btnListar.IsVisible = false;
            //_imgSorteio.IsVisible = false;
            //_lblCarregando.IsVisible = true;

            //await Task.Yield(); // Aguardar a atualização do layout

            // Realizar o processamento aqui
            await Navigation.PushAsync(new PageTimeSorteio());

            //// Agendar o processamento em segundo plano        
            //await Task.Run(() =>
            //{
            //    // Ocultar o ActivityIndicator
            //    MainThread.BeginInvokeOnMainThread(() =>
            //    {
            //        //_activity.IsVisible = false;
            //        //_activity.IsRunning = false;
            //    });
            //});
        }
        finally
        {
            //Retornar os botões
            //_btnSortear.IsVisible = true;
            //_btnListar.IsVisible = true;
            //_imgSorteio.IsVisible = true;
            //_lblCarregando.IsVisible = false;
        }

    }
    private async void MenuListaSorteio(object sender, EventArgs e)
    {
        // Exibir o ActivityIndicator imediatamente após o clique
        //_activity.IsVisible = true;
        //_activity.IsRunning = true;

        try
        {
            //_btnSortear.IsVisible = false;
            //_btnListar.IsVisible = false;
            //_imgSorteio.IsVisible = false;
            //_lblCarregando.IsVisible = true;

            await Task.Yield(); // Aguardar a atualização do layout

            // Realizar o processamento aqui
            await Navigation.PushAsync(new PageTimeLista());

            // Agendar o processamento em segundo plano        
            await Task.Run(() =>
            {
                // Ocultar o ActivityIndicator
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    //_activity.IsVisible = false;
                    //_activity.IsRunning = false;
                });
            });
        }
        finally
        {
            //Retornar os botões
            //_btnSortear.IsVisible = true;
            //_btnListar.IsVisible = true;
            //_imgSorteio.IsVisible = true;
            //_lblCarregando.IsVisible = false;
        }
    }
}