using Maui.SorteioJusto.Models;
using Maui.SorteioJusto.ViewModels;

namespace Maui.SorteioJusto.Pages;

public partial class PagePartidaRegistro : ContentPage
{
	ViewModelPartida VMPartida = new ViewModelPartida();

	public PagePartidaRegistro(ViewModelPartida vmPartida)
	{
		InitializeComponent();
		VMPartida      = vmPartida;
		BindingContext = VMPartida;
	}

    private void OnStartButtonClicked(object sender, EventArgs e)
    {
        VMPartida.StartTimer();
    }

    private void OnResetButtonClicked(object sender, EventArgs e)
    {
        VMPartida.ResetTimer();
    }

    private async void ButtonEditarTime(object sender, EventArgs e)
    {
        Button button       = (Button)sender;
        string carouselName = (string)button.CommandParameter;

        if (carouselName == "_picker1")
        {
            VMPartida.TimeParaEdicao = (Time)_picker1.SelectedItem;
            if (_picker1.SelectedItem != null)
            {
                if (true /*(VMPartida.TimeParaEdicao.Incompleto)*/)
                {
                    VMPartida.CarregarListaEditar(VMPartida.TimeParaEdicao);
                    //await Navigation.PushAsync(new MenuEdicaoTime((VMPartida.TimeParaEdicao), VMPartida));
                }
                else
                {
                    await DisplayAlert("Aviso", "A equipe já está completa", "Retornar");
                }
            }
            else
            {
                await DisplayAlert("Aviso", "Selecione uma Equipe", "Retornar");
            }
        }
        else if (carouselName == "_picker2")
        {
            VMPartida.TimeParaEdicao = (Time)_picker2.SelectedItem;
            if (_picker2.SelectedItem != null)
            {
                if (true/*(VMPartida.TimeParaEdicao.Incompleto)*/)
                {
                    VMPartida.CarregarListaEditar(VMPartida.TimeParaEdicao);
                    //await Navigation.PushAsync(new MenuEdicaoTime((VMPartida.TimeParaEdicao), VMPartida));
                }
                else
                {
                    await DisplayAlert("Aviso", "A equipe já está completa", "Retornar");
                }
            }
            else
            {
                await DisplayAlert("Aviso", "Selecione uma Equipe", "Retornar");
            }
        }

    }

    private async void ButtonPartidaIniciada(object sender, EventArgs e)
    {
        try
        {
            if (VMPartida.ValidacaoPartida())
            {
                VMPartida.IsPartidaIniciada  = true;
                VMPartida.PodeIniciarPartida = false;

                VMPartida.StartTimer();
                VMPartida.PodeRegistrarPartida = true;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "Fechar");
        }
    }

    private async void FinalizarPartida(object sender, EventArgs e)
    {
        VMPartida.SalvarPartida();
        if (VMPartida.IsPartidaRegistrada)
        {
            await DisplayAlert("", "Partida Registrada com Sucesso!", "Concluir");

            VMPartida.PodeIniciarPartida   = true;
            VMPartida.PodeRegistrarPartida = false;
            VMPartida.IsPartidaIniciada    = false;
            VMPartida.IsPartidaRegistrada  = false;
            VMPartida.ResetTimer();

            await Navigation.PopToRootAsync();
        }
    }

    //protected override void OnAppearing()
    //{
    //    base.OnAppearing();
    //    VMPartida.ListaDeTimes = VMPartida.CarregarListaTimes();
    //}

    protected override bool OnBackButtonPressed()
    {
        if (VMPartida.PodeRegistrarPartida)
        {
            // Exibir o DisplayAlert com a mensagem de aviso pro usuário
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                bool result = await DisplayAlert("Aviso", "Ao sair, perderá o status atual da partida", "Sair", "Ficar");
                if (result)
                {
                    await Navigation.PopAsync();
                }
            });
            return true; // Impede o comportamento padrão do botão de voltar
        }
        return false;
    }

}