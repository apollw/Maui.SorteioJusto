
using Maui.SorteioJusto.Services.Interfaces;

namespace Maui.SorteioJusto
{
    public partial class App : Application
    {
        string syncfusion_license = "Ngo9BigBOggjHTQxAR8/V1NAaF1cWWhIfEx1RHxQdld5ZFRHallYTnNWUj0eQnxTdEFjW39YcXVVQWVVUkx0Xg==";

        public App(IDbService dbService)
        {
            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(syncfusion_license);

            InitializeComponent();

            //Inicializando instância única do Banco no início da aplicação
            dbService.InitializeAsync();
            MainPage = new AppShell();
        }
    }
}
