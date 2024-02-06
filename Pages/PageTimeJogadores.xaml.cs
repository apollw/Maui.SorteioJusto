using Maui.SorteioJusto.Models;
using Maui.SorteioJusto.Services.Interfaces;
using Maui.SorteioJusto.ViewModels;

namespace Maui.SorteioJusto.Pages;

public partial class PageTimeJogadores : ContentPage
{
    ViewModelTime VMTime = new ViewModelTime();

    public PageTimeJogadores(ViewModelTime vmTime)
    {
        InitializeComponent();
        VMTime = vmTime;
        BindingContext = VMTime;
    }

    public async void IniciarSorteio(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PageTimeSorteio(VMTime));
    }

    private void AtualizarExibicaoCheckBoxes()
    {
        _collectionView.ItemsSource = null;
        _collectionView.ItemsSource = VMTime.ListaDeJogadores;
    }

    public void DesmarcarTodos(object sender, EventArgs e)
    {
        foreach (var jogador in VMTime.ListaDeJogadores)
        {
            jogador.Status = 0;
        }
        AtualizarExibicaoCheckBoxes();
    }

    public void SelecionarTodos(object sender, EventArgs e)
    {
        // Verifica se todos os jogadores já estão selecionados
        bool todosSelecionados = VMTime.ListaDeJogadores.All(jogador => jogador.Status == 1);

        // Define o status de cada jogador para 1 (selecionado) ou 0 (não selecionado)
        foreach (var jogador in VMTime.ListaDeJogadores)
        {
            jogador.Status = todosSelecionados ? 0 : 1;
        }
        AtualizarExibicaoCheckBoxes();
    }

    public void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var checkBox = sender as CheckBox;
        var jogador = checkBox.BindingContext as Jogador;

        if (jogador != null)
        {
            // Atualizar o estado da checkbox no jogador
            jogador.Status = checkBox.IsChecked ? 1 : 0;

        }
    }
}