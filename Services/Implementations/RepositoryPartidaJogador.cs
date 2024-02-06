using Maui.SorteioJusto.Models;
using Maui.SorteioJusto.Services.Interfaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui.SorteioJusto.Services.Implementations
{
    public class RepositoryPartidaJogador : IRepositoryPartidaJogador
    {
        private readonly SQLiteAsyncConnection _dbConnection;

        public RepositoryPartidaJogador(IDbService dbService)
        {
            _dbConnection = dbService.GetDbConnection();
        }

        public async Task<List<PartidaJogador>> GetPartidaJogadoresAsync()
        {
            return await _dbConnection.Table<PartidaJogador>().ToListAsync();
        }

        public async Task<PartidaJogador> GetPartidaJogadorAsync(int partidaId)
        {
            return await _dbConnection.Table<PartidaJogador>().FirstOrDefaultAsync(pj => pj.Partida == partidaId);
        }

        public async Task<int> AddPartidaJogadorAsync(PartidaJogador partidaJogador)
        {
            return await _dbConnection.InsertAsync(partidaJogador);
        }

        public async Task<int> UpdatePartidaJogadorAsync(PartidaJogador partidaJogador)
        {
            return await _dbConnection.UpdateAsync(partidaJogador);
        }

        public async Task<int> DeletePartidaJogadorAsync(int partidaId)
        {
            return await _dbConnection.DeleteAsync<PartidaJogador>(partidaId);
        }
    }
}
