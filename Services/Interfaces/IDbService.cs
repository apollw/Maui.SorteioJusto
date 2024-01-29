
using SQLite;

namespace Maui.SorteioJusto.Services.Interfaces
{
    public interface IDbService
    {
        Task InitializeAsync();
        SQLiteAsyncConnection GetDbConnection();
    }
}
