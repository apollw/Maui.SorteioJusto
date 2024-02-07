using Maui.SorteioJusto.Models;
using Maui.SorteioJusto.Services.Interfaces;
using Maui.SorteioJusto.ViewModels;

namespace Maui.SorteioJusto.Pages;

public partial class PageJogadorCadastro : ContentPage
{
    Jogador          ObjJogador = new Jogador();
    ViewModelJogador VMJogador = new ViewModelJogador();

    public PageJogadorCadastro()
    {
        InitializeComponent();
    }

    public PageJogadorCadastro(ViewModelJogador vmJogador)
    {
        InitializeComponent();
        VMJogador                    = vmJogador;
        VMJogador.ObjJogadorOriginal = new Jogador(VMJogador.ObjJogador);
        BindingContext               = VMJogador;
    }

    private async void Button_SalvarJogador(object sender, EventArgs e)
    {
        // Modificar o ObjJogador com os novos valores
        ObjJogador.Id            = VMJogador.ObjJogador.Id;
        ObjJogador.Nome          = _entryNome.Text;
        ObjJogador.Telefone      = _entryTelefone.Text;
        ObjJogador.Classificacao = _stepper.Value;

        VMJogador.IsCadastrado = false;
        VMJogador.SalvarJogador(ObjJogador);

        if (VMJogador.IsCadastrado)
        {
            await Navigation.PopAsync();
        }
        else
        {
            // Resetar os valores das entradas
            _entryNome.Text     = VMJogador.ObjJogadorOriginal.Nome;
            _entryTelefone.Text = VMJogador.ObjJogadorOriginal.Telefone;
        }
    }

    private void EntryNome_Focused(object sender, FocusEventArgs e)
    {
        _entryNome.Completed += (s, e) =>
        {
            _entryTelefone.Focus();
        };
    }

    private void EntryTelefone_Focused(object sender, FocusEventArgs e)
    {
        _entryTelefone.Completed += (s, e) =>
        {
            _entryTelefone.IsEnabled = false;
            _entryTelefone.Unfocus();
            _entryTelefone.IsEnabled = true;
        };
    }
}