using Maui.SorteioJusto.Models;
using Maui.SorteioJusto.Services.Interfaces;
using Maui.SorteioJusto.ViewModels;
using System.Collections.ObjectModel;

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
        await Navigation.PushAsync(new PageTimeJogadores(VMTime));
    }

    private async void OnEditButtonClicked(object sender, EventArgs e) //PARA EDITAR
    {
        var button = sender as ImageButton;
        var time   = button?.BindingContext as Time;

        if (time != null)
        {
            VMTime.TimeParaEdicao1 = time;
            VMTime.ListaDeEdicao = 
                new ObservableCollection<Time>
                (
                    VMTime.ListaDeTimes.
                    Where(t => t.Id != VMTime.TimeParaEdicao1.Id).
                    ToList()
                );

            await Navigation.PushAsync(new PageTimeSelecao(VMTime));
        }
    }

    private async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var button = sender as ImageButton;
        var time = button?.BindingContext as Time;

        if (time != null)
        {
            string message = "Deseja excluir " + time.Nome + "?";
            bool confirmar = await DisplayAlert("Aviso", message, "Sim", "Não");

            if (confirmar)
            {
                VMTime.ListaDeTimes.Remove(time);
                await _rpTime.DeleteTimeAsync(time.Id);
            }
        }
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