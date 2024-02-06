using Maui.SorteioJusto.Models;

namespace Maui.SorteioJusto.Services.Interfaces
{
    public interface IRepositoryTime
    {
        Task<List<Time>> GetTimesAsync();
        Task<Time> GetTimeAsync(int timeId);
        Task<int> AddTimeAsync(Time time);
        Task<int> UpdateTimeAsync(Time time);
        Task<int> DeleteTimeAsync(int timeId);
        Task DeleteListaDeTimesAsync(List<Time> listaParaExclusao);

        //Métodos TimeJogador
        Task<List<TimeJogador>> GetTimeJogadoresAsync();
        Task<TimeJogador> GetTimeJogadorAsync(int timeId);
        Task<int> AddTimeJogadorAsync(TimeJogador timeJogador);
        Task<int> UpdateTimeJogadorAsync(TimeJogador timeJogador);
        Task<int> DeleteTimeJogadorAsync(int timeId);
        Task DeleteListaDeTimeJogadorAsync(List<TimeJogador> listaParaExclusao);
    }
}
