using Microsoft.Data.Sqlite;
using System.Threading.Tasks;

namespace SmartVault.DataGeneration
{
    public interface IDataSeeder
    {
        Task SeedDataAsync(SqliteConnection connection);
    }
}
