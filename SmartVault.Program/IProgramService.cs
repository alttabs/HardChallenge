using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace SmartVault.Program
{
  public interface IProgramService
  {
    Task WriteEveryThirdFileToFileAsync(string accountId, SqliteConnection connection);
    Task<long> GetAllFileSizesAsync();
  }
}
