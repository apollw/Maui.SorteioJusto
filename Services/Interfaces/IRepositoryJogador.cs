
using Maui.SorteioJusto.Models;

namespace Maui.SorteioJusto.Services.Interfaces
{
    public interface IRepositoryJogador
    {
        Task<List<Jogador>> GetJogadoresAsync();
        Task<Jogador> GetJogadorAsync(int jogadorId);
        Task<int> AddJogadorAsync(Jogador jogador);
        Task<int> UpdateJogadorAsync(Jogador jogador);
        Task<int> DeleteJogadorAsync(int jogadorId);
    }
}
