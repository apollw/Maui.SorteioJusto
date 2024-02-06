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

        //Métodos de PartidaJogador

        public async Task<int> AddPartidaJogadorAsync(PartidaJogador partidaJogador)
        {
            return await _dbConnection.InsertAsync(partidaJogador);
        }

        public async Task<List<PartidaJogador>> GetPartidaJogadoresAsync()
        {
            return await _dbConnection.Table<PartidaJogador>().ToListAsync();
        }

        public async Task<PartidaJogador> GetPartidaJogadorAsync(int partidaId)
        {
            return await _dbConnection.Table<PartidaJogador>().FirstOrDefaultAsync(pj => pj.Partida == partidaId);
        }

        public async Task<int> UpdatePartidaJogadorAsync(PartidaJogador partidaJogador)
        {
            return await _dbConnection.UpdateAsync(partidaJogador);
        }

        public async Task<int> DeletePartidaJogadorAsync(int partidaId)
        {
            return await _dbConnection.DeleteAsync<PartidaJogador>(partidaId);
        }

        public async Task DeleteListaDePartidaJogadorAsync(List<PartidaJogador> listaParaExclusao)
        {
            foreach (PartidaJogador element in listaParaExclusao)
            {
                await _dbConnection.DeleteAsync<PartidaJogador>(element.Id);
            }
        }
    }
}
