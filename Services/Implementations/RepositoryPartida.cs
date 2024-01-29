
using Maui.SorteioJusto.Models;
using Maui.SorteioJusto.Services.Interfaces;
using SQLite;

namespace Maui.SorteioJusto.Services.Implementations
{
    public class RepositoryPartida : IRepositoryPartida
    {
        private readonly SQLiteAsyncConnection _dbConnection;

        public RepositoryPartida(IDbService dbService)
        {
            _dbConnection = dbService.GetDbConnection();
        }

        public async Task<List<Partida>> GetPartidasAsync()
        {
            return await _dbConnection.Table<Partida>().ToListAsync();
        }

        public async Task<Partida> GetPartidaAsync(int partidaId)
        {
            return await _dbConnection.Table<Partida>().FirstOrDefaultAsync(p => p.Id == partidaId);
        }

        public async Task<int> AddPartidaAsync(Partida partida)
        {
            return await _dbConnection.InsertAsync(partida);
        }

        public async Task<int> UpdatePartidaAsync(Partida partida)
        {
            return await _dbConnection.UpdateAsync(partida);
        }

        public async Task<int> DeletePartidaAsync(int partidaId)
        {
            return await _dbConnection.DeleteAsync<Partida>(partidaId);
        }
    }
}
