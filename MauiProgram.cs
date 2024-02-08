using Maui.SorteioJusto.Pages;
using Maui.SorteioJusto.Services.Implementations;
using Maui.SorteioJusto.Services.Interfaces;
using Maui.SorteioJusto.ViewModels;
using Microsoft.Extensions.Logging;

namespace Maui.SorteioJusto
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
#if DEBUG
            builder.Logging.AddDebug();
#endif
            //Services
            builder.Services.AddSingleton <IDbService        , DbService>();
            builder.Services.AddScoped    <IRepositoryTime   , RepositoryTime>();
            builder.Services.AddScoped    <IRepositoryJogador, RepositoryJogador>();
            builder.Services.AddScoped    <IRepositoryPartida, RepositoryPartida>();

            //Pages
            builder.Services.AddTransient<PageJogador>();
            builder.Services.AddTransient<PageJogadorCadastro>();

            builder.Services.AddTransient<PagePartida>();
            builder.Services.AddTransient<PagePartidaSelecao>();
            builder.Services.AddTransient<PagePartidaRegistro>();

            builder.Services.AddTransient<PageTime>();
            builder.Services.AddTransient<PageTimeEdicao>();
            builder.Services.AddTransient<PageTimeSorteio>();
            builder.Services.AddTransient<PageTimeJogadores>();

            //ViewModels
            builder.Services.AddTransient<ViewModelTime>();
            builder.Services.AddTransient<ViewModelJogador>();
            builder.Services.AddTransient<ViewModelPartida>();

            //Syncfusion
            //builder.ConfigureSyncfusionCore();

            return builder.Build();
        }
    }
}
