
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
            builder.Services.AddSingleton<IDbService,DbService>();
            builder.Services.AddTransient<IRepositoryJogador, RepositoryJogador>();
            builder.Services.AddTransient<IRepositoryPartida, RepositoryPartida>();

            //Pages
            //builder.Services.AddTransient<PageTime>();
            builder.Services.AddTransient<PageJogador>();
            builder.Services.AddTransient<PagePartida>();            
            //builder.Services.AddTransient<PageTimeLista>();
            //builder.Services.AddTransient<PageTimeSorteio>();
            builder.Services.AddTransient<PageJogadorCadastro>();

            //ViewModels
            builder.Services.AddTransient<ViewModelJogador>();
            //builder.Services.AddTransient<ViewModelTime>();
            //builder.Services.AddTransient<ViewModelPartida>();

            return builder.Build();
        }
    }
}
