using Maui.SorteioJusto.Models;
using Maui.SorteioJusto.ViewModels;

namespace Maui.SorteioJusto.Pages;

public partial class PageTimeJogadores : ContentPage
{
    Jogador _jogadorFiltrado = new Jogador();
    ViewModelTime VMTime     = new ViewModelTime();

    public PageTimeJogadores(ViewModelTime vmTime)
    {
        InitializeComponent();
        VMTime = vmTime;
        BindingContext = VMTime;
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        _collectionView.ItemsSource = SearchUsers(((SearchBar)sender).Text);
    }

    private List<Jogador> SearchUsers(string textoFiltro)
    {
        var jogadores = VMTime.ListaDeJogadores.Where(
                            x => !string.IsNullOrWhiteSpace(x.Nome) &&
                            x.Nome.StartsWith(textoFiltro, StringComparison.OrdinalIgnoreCase)
                        )?.ToList();

        // Se houver apenas um jogador filtrado, armazene-o
        _jogadorFiltrado = (jogadores.Count == 1) ? jogadores[0] : null;

        return jogadores;
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

    public void SelecionarTodos(object sender, EventArgs e)
    {
        // Verifica se todos os jogadores já estão selecionados
        bool todosSelecionados = VMTime.ListaDeJogadores.All(jogador => jogador.Status == 1);

        // Define o status de cada jogador para 1 (selecionado) ou 0 (não selecionado)
        foreach (var jogador in VMTime.ListaDeJogadores)
        {
            jogador.Status = todosSelecionados ? 0 : 1;
        }
        VMTime.AtualizarQtdDeJogadoresSelecionados();
        AtualizarExibicaoCheckBoxes();
    }

    public void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var checkBox = sender as CheckBox;
        var jogador  = checkBox.BindingContext as Jogador;

        if (jogador != null)
        {
            // Atualizar o estado da checkbox no jogador
            VMTime.AtualizarQtdDeJogadoresSelecionados();
        }
    }
}