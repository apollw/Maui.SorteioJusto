using Maui.SorteioJusto.Models;

namespace Maui.SorteioJusto.Services.Interfaces
{
    public interface IRepositoryPartida
    {
        Task<List<Partida>> GetPartidasAsync();
        Task<Partida> GetPartidaAsync(int partidaId);
        Task<int> AddPartidaAsync(Partida partida);
        Task<int> UpdatePartidaAsync(Partida partida);
        Task<int> DeletePartidaAsync(int partidaId);

        //Métodos PartidaJogador
        Task<List<PartidaJogador>> GetPartidaJogadoresAsync();
        Task<PartidaJogador> GetPartidaJogadorAsync(int partidaId);
        Task<int> AddPartidaJogadorAsync(PartidaJogador partidaJogador);
        Task<int> UpdatePartidaJogadorAsync(PartidaJogador partidaJogador);
        Task<int> DeletePartidaJogadorAsync(int partidaId);
        Task DeleteListaDePartidaJogadorAsync(List<PartidaJogador> listaParaExclusao);
    }
}
