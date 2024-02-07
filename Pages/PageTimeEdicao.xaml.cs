using Maui.SorteioJusto.Models;
using Maui.SorteioJusto.ViewModels;
using Syncfusion.Maui.Core.Carousel;
using System.Diagnostics;

namespace Maui.SorteioJusto.Pages;

public partial class PageTimeEdicao : ContentPage
{
    Time _timeParaEdicao = new Time();
    ViewModelPartida VMPartida = new ViewModelPartida();

	public PageTimeEdicao(ViewModelPartida vmPartida)
	{
		InitializeComponent();
        VMPartida       = vmPartida;
        _timeParaEdicao = VMPartida.TimeParaEdicao;
        BindingContext  = VMPartida;
	}


    public async void Button_SalvarTime(object sender, EventArgs e)
    {
        VMPartida.EditarTime(_timeParaEdicao);

        if (VMPartida.IsTimeEditado == true)
        {
            VMPartida.IsTimeEditado = false;
            await Navigation.PopToRootAsync();
        }
    }

    public void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var checkBox = (CheckBox)sender;
        var meuItem = (Jogador)checkBox.BindingContext;

        if (e.Value)
        {
            // Se a CheckBox estiver marcada, incluir o item na lista
            VMPartida.ListaDeAdicao.Add(meuItem);
        }
        else
        {
            // Se a CheckBox estiver desmarcada, remover o item da lista
            VMPartida.ListaDeAdicao.Remove(meuItem);
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        //ViewModel.ListaEditar = ViewModel.AtualizarListaEditar();
    }
}


