using Maui.SorteioJusto.Models;
using Maui.SorteioJusto.ViewModels;

namespace Maui.SorteioJusto.Pages;

public partial class PagePartidaSelecao : ContentPage
{
    ViewModelPartida VMPartida = new ViewModelPartida();

    public PagePartidaSelecao(ViewModelPartida vmPartida)
    {
        InitializeComponent();
        VMPartida      = vmPartida;
        BindingContext = VMPartida;
    }

    private async void Button_EditarTime(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        string carouselName = (string)button.CommandParameter;

        if (carouselName == "_picker1")
        {
            VMPartida.TimeParaEdicao = (Time)_picker1.SelectedItem;
            if (_picker1.SelectedItem != null)
            {
                if (VMPartida.TimeParaEdicao.IsTimeIncompleto)
                {
                    VMPartida.CarregarListaEditar(VMPartida.TimeParaEdicao);
                    await Navigation.PushAsync(new PageTimeEdicao(VMPartida));
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
                if (VMPartida.TimeParaEdicao.IsTimeIncompleto)
                {
                    VMPartida.CarregarListaEditar(VMPartida.TimeParaEdicao);
                    await Navigation.PushAsync(new PageTimeEdicao(VMPartida));
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

    private async void Button_IniciarPartida(object sender, EventArgs e)
    {
        VMPartida.ValidacaoPartida();

        if (VMPartida.PodeIniciarPartida)
        {
            await Navigation.PushAsync(new PagePartidaRegistro(VMPartida));
        }
        else
        {
            VMPartida.PodeIniciarPartida = true;
        }
    }

}