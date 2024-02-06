using Maui.SorteioJusto.Models;
using Maui.SorteioJusto.Services.Interfaces;
using SQLite;

namespace Maui.SorteioJusto.Services.Implementations
{
    public class RepositoryTime : IRepositoryTime
    {
        private readonly SQLiteAsyncConnection _dbConnection;

        public RepositoryTime(IDbService dbService)
        {
            _dbConnection = dbService.GetDbConnection();
        }

        public async Task<List<Time>> GetTimesAsync()
        {
            return await _dbConnection.Table<Time>().ToListAsync();
        }

        public async Task<Time> GetTimeAsync(int timeId)
        {
            return await _dbConnection.Table<Time>().FirstOrDefaultAsync(t => t.Id == timeId);
        }

        public async Task<int> AddTimeAsync(Time time)
        {
            return await _dbConnection.InsertAsync(time);
        }

        public async Task<int> UpdateTimeAsync(Time time)
        {
            return await _dbConnection.UpdateAsync(time);
        }

        public async Task<int> DeleteTimeAsync(int timeId)
        {
            return await _dbConnection.DeleteAsync<Time>(timeId);
        }

        public async Task DeleteListaDeTimesAsync(List<Time> listaParaExclusao)
        {
            foreach(Time element in  listaParaExclusao)
            {
                await _dbConnection.DeleteAsync<Time>(element.Id);
            }
        }

        //Métodos de TimeJogador

        public async Task<int> AddTimeJogadorAsync(TimeJogador timeJogador)
        {
            return await _dbConnection.InsertAsync(timeJogador);
        }

        public async Task<List<TimeJogador>> GetTimeJogadoresAsync()
        {
            return await _dbConnection.Table<TimeJogador>().ToListAsync();
        }

        public async Task<TimeJogador> GetTimeJogadorAsync(int timeId)
        {
            return await _dbConnection.Table<TimeJogador>().FirstOrDefaultAsync(tj => tj.Time == timeId);
        }

        public async Task<int> UpdateTimeJogadorAsync(TimeJogador timeJogador)
        {
            return await _dbConnection.UpdateAsync(timeJogador);
        }

        public async Task<int> DeleteTimeJogadorAsync(int timeId)
        {
            return await _dbConnection.DeleteAsync<TimeJogador>(timeId);
        }

        public async Task DeleteListaDeTimeJogadorAsync(List<TimeJogador> listaParaExclusao)
        {
            foreach (TimeJogador element in listaParaExclusao)
            {
                await _dbConnection.DeleteAsync<TimeJogador>(element.Id);
            }
        }
    }
}
