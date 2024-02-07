using Maui.SorteioJusto.Models;
using Maui.SorteioJusto.Services.Interfaces;
using SQLite;

namespace Maui.SorteioJusto.Services.Implementations
{
    public class DbService : IDbService
    {
        private SQLiteAsyncConnection _dbConnection;

        public async Task InitializeAsync()
        {
            await SetUpDb();
        }

        public async Task SetUpDb()
        {

            if (_dbConnection == null)
            {
                // Caminho para o desktop
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                //Caminho de Local Application Data
                string localDevicePath = Path.Combine(
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.
                        LocalApplicationData));

                string dbFileName = "SorteioJustoDB.db3";

                // Caminho completo para o arquivo do banco de dados no desktop
                string dbPath = Path.Combine(localDevicePath, dbFileName);

                // Caminho completo para o arquivo do banco de dados no desktop
                //string dbPath = Path.Combine(desktopPath, dbFileName);                

                // Cria uma nova conexão após excluir o banco de dados antigo
                _dbConnection = new SQLiteAsyncConnection(dbPath);

                await CreateTables();

            }
        }

        public SQLiteAsyncConnection GetDbConnection()
        {
            return _dbConnection;
        }

        public async Task CreateTables()
        {
            await _dbConnection.CreateTablesAsync(CreateFlags.AllImplicit, 
                typeof(Jogador), 
                typeof(Time),
                typeof(TimeJogador),
                typeof(Partida),
                typeof(PartidaJogador)
                );
        }
    }
}
