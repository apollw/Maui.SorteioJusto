
using Maui.SorteioJusto.Services.Interfaces;

namespace Maui.SorteioJusto
{
    public partial class App : Application
    {
        public App(IDbService dbService)
        {
            InitializeComponent();

            //Inicializando instância única do Banco no início da aplicação
            dbService.InitializeAsync();
            MainPage = new AppShell();
        }
    }
}
