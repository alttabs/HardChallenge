using System.Collections.Concurrent;
using System.Threading.Tasks;
using SmartVault.Program.BusinessObjects;
using Microsoft.Data.Sqlite;

namespace SmartVault.DataGeneration.Repository
{
  public class DataGenerationRepository : IDataGenerationRepository
  {
    public async Task InsertUsers(SqliteCommand command, ConcurrentBag<User> users)
    {
      foreach (var user in users)
      {
        command.CommandText = $"INSERT INTO User (Id, FirstName, LastName, DateOfBirth, AccountId, Username, Password, CreatedOn) VALUES(@Id, @FirstName, @LastName, @DateOfBirth, @AccountId, @Username, @Password, @CreatedOn)";
        command.Parameters.AddWithValue("@Id", user.Id);
        command.Parameters.AddWithValue("@FirstName", user.FirstName);
        command.Parameters.AddWithValue("@LastName", user.LastName);
        command.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
        command.Parameters.AddWithValue("@AccountId", user.AccountId);
        command.Parameters.AddWithValue("@Username", user.Username);
        command.Parameters.AddWithValue("@Password", user.Password);
        command.Parameters.AddWithValue("@CreatedOn", user.CreatedOn);
        await command.ExecuteNonQueryAsync();
      }
    }

    public async Task InsertAccounts(SqliteCommand command, ConcurrentBag<Account> accounts)
    {
      foreach (var account in accounts)
      {
        command.CommandText = $"INSERT INTO Account (Id, Name, CreatedOn) VALUES(@Id, @Name, @CreatedOn)";
        command.Parameters.AddWithValue("@Id", account.Id);
        command.Parameters.AddWithValue("@Name", account.Name);
        command.Parameters.AddWithValue("@CreatedOn", account.CreatedOn);
        await command.ExecuteNonQueryAsync();
      }
    }

    public async Task InsertDocuments(SqliteCommand command, ConcurrentBag<Document> documents)
    {
      foreach (var document in documents)
      {
        command.CommandText = $"INSERT INTO Document (Id, Name, FilePath, Length, AccountId, CreatedOn) VALUES(@Id, @Name, @FilePath, @Length, @AccountId, @CreatedOn)";
        command.Parameters.AddWithValue("@Id", document.Id);
        command.Parameters.AddWithValue("@Name", document.Name);
        command.Parameters.AddWithValue("@FilePath", document.FilePath);
        command.Parameters.AddWithValue("@Length", document.Length);
        command.Parameters.AddWithValue("@AccountId", document.AccountId);
        command.Parameters.AddWithValue("@CreatedOn", document.CreatedOn);
        await command.ExecuteNonQueryAsync();
      }
    }
  }
}
