using Maui.SorteioJusto.Models;

namespace Maui.SorteioJusto.Services.Interfaces
{
    public interface IRepositoryPartidaJogador
    {
        Task<List<PartidaJogador>> GetPartidaJogadoresAsync();
        Task<PartidaJogador> GetPartidaJogadorAsync(int partidaId);
        Task<int> AddPartidaJogadorAsync(PartidaJogador partidaJogador);
        Task<int> UpdatePartidaJogadorAsync(PartidaJogador partidaJogador);
        Task<int> DeletePartidaJogadorAsync(int partidaId);
    }
}
