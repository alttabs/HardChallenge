using System.Collections.Concurrent;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using SmartVault.Program.BusinessObjects;

namespace SmartVault.DataGeneration.Repository
{
  public interface IDataGenerationRepository
  {
    Task InsertUsers(SqliteCommand command, ConcurrentBag<User> users);
    Task InsertAccounts(SqliteCommand command, ConcurrentBag<Account> accounts);
    Task InsertDocuments(SqliteCommand command, ConcurrentBag<Document> documents);
  }
}
