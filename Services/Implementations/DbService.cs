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
            if(_dbConnection == null)
            {
                string dbPath = Path.Combine(
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.
                        LocalApplicationData),"SorteioJustoDB.db3");

                _dbConnection = new SQLiteAsyncConnection(dbPath);

                //Criação Assíncrona de Tabelas
                await CreateTables();
            }
        }

        public SQLiteAsyncConnection GetDbConnection()
        {
            return _dbConnection;
        }

        public async Task CreateTables()
        {
            await _dbConnection.CreateTableAsync<Time>();
            await _dbConnection.CreateTableAsync<Jogador>();
            await _dbConnection.CreateTableAsync<Partida>();
            await _dbConnection.CreateTableAsync<TimeJogador>();
            await _dbConnection.CreateTableAsync<PartidaJogador>();  
        }
    }
}
