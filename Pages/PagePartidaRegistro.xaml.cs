using Maui.SorteioJusto.ViewModels;

namespace Maui.SorteioJusto.Pages;

public partial class PagePartidaRegistro : ContentPage
{
    ViewModelPartida VMPartida = new ViewModelPartida();

    public PagePartidaRegistro()
    { }

    public PagePartidaRegistro(ViewModelPartida vmPartida)
	{
		InitializeComponent();
        VMPartida      = vmPartida;
        BindingContext = VMPartida;

        VMPartida.IsPartidaIniciada    = true;
        VMPartida.PodeRegistrarPartida = true;

        VMPartida.StartTimer();
    }

    private async void Button_FinalizarPartida(object sender, EventArgs e)
    {
        VMPartida.SalvarPartida();

        if (VMPartida.IsPartidaRegistrada)
        {
            await DisplayAlert("Aviso", "Partida Registrada com Sucesso!", "Concluir");

            VMPartida.PodeIniciarPartida   = true;
            VMPartida.PodeRegistrarPartida = false;
            VMPartida.IsPartidaIniciada    = false;
            VMPartida.IsPartidaRegistrada  = false;
            VMPartida.ResetTimer();

            await Navigation.PopToRootAsync();
        }
    }

    private void OnStartButtonClicked(object sender, EventArgs e)
    {
        VMPartida.StartTimer();
    }

    private void OnResetButtonClicked(object sender, EventArgs e)
    {
        VMPartida.ResetTimer();
    }

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