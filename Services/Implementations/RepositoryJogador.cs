using Maui.SorteioJusto.Models;
using Maui.SorteioJusto.Services.Interfaces;
using SQLite;

namespace Maui.SorteioJusto.Services.Implementations
{
    public class RepositoryJogador : IRepositoryJogador
    {
        private readonly SQLiteAsyncConnection _dbConnection;

        public RepositoryJogador(IDbService dbService)
        {
            _dbConnection = dbService.GetDbConnection();
        }

        public async Task<List<Jogador>> GetJogadoresAsync()
        {
            return await _dbConnection.Table<Jogador>().ToListAsync();
        }

        public async Task<Jogador> GetJogadorAsync(int jogadorId)
        {
            return await _dbConnection.Table<Jogador>().FirstOrDefaultAsync(j => j.Id == jogadorId);
        }

        public async Task<int> AddJogadorAsync(Jogador jogador)
        {
            return await _dbConnection.InsertAsync(jogador);
        }

        public async Task<int> UpdateJogadorAsync(Jogador jogador)
        {
            return await _dbConnection.UpdateAsync(jogador);
        }

        public async Task<int> DeleteJogadorAsync(int jogadorId)
        {
            return await _dbConnection.DeleteAsync<Jogador>(jogadorId);
        }
    }
}
