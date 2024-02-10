using Maui.SorteioJusto.Models;
using Maui.SorteioJusto.ViewModels;

namespace Maui.SorteioJusto.Pages;

public partial class PageTimeEdicao : ContentPage
{
    ViewModelTime    VMTime    = new ViewModelTime();
    ViewModelPartida VMPartida = new ViewModelPartida();    

    public PageTimeEdicao(ViewModelTime vmTime)
    {
        InitializeComponent();
        VMTime          = vmTime;
        BindingContext  = VMTime;
    }

    public PageTimeEdicao(ViewModelPartida vmPartida)
	{
		InitializeComponent();
        VMPartida       = vmPartida;
        BindingContext  = VMPartida;
	}

    public void Button_MoverJogador(object sender, EventArgs e)
    {
        var button  = sender as ImageButton;
        var jogador = button?.BindingContext as Jogador;

        if (jogador != null)
        {
            //Verificar se o jogador é do TimeParaEdicao1 ou TimeParaEdicao2

            if (VMTime.TimeParaEdicao1.ListaJogadores.Contains(jogador))
            {
                VMTime.MoverJogador(VMTime.TimeParaEdicao1,
                                    VMTime.TimeParaEdicao2,
                                    jogador);            }
            else
            {
                VMTime.MoverJogador(VMTime.TimeParaEdicao2,
                                    VMTime.TimeParaEdicao1,
                                    jogador);
            }            
        }
    }

    //public async void Button_SalvarTime(object sender, EventArgs e)
    //{
    //    VMPartida.EditarTime(_timeParaEdicao);

    //    if (VMPartida.IsTimeEditado == true)
    //    {
    //        VMPartida.IsTimeEditado = false;
    //        await Navigation.PopToRootAsync();
    //    }
    //}

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


