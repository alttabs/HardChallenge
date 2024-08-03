using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using SmartVault.DataGeneration.Persistence;
using SmartVault.DataGeneration.Repository;
using SmartVault.Program.BusinessObjects;

namespace SmartVault.DataGeneration
{
  public class AsyncDataSeeder : IDataSeeder
  {
    private readonly IDataGenerationRepository _sqliteRepository;
    private readonly ISqliteCommand _sqliteCommand;
    private readonly IFileSystem _fileSystem;

    public AsyncDataSeeder(IDataGenerationRepository sqliteRepository, ISqliteCommand sqliteCommand, IFileSystem fileSystem)
    {
      _sqliteRepository = sqliteRepository;
      _sqliteCommand = sqliteCommand;
      _fileSystem = fileSystem;
    }

    public async Task SeedDataAsync(SqliteConnection connection)
    {
      var users = new ConcurrentBag<User>();
      var accounts = new ConcurrentBag<Account>();
      var documents = new ConcurrentBag<Document>();

      int documentNumber = 0;
      for (int i = 0; i < 100; i++)
      {
        var randomDayIterator = RandomDay().GetEnumerator();
        randomDayIterator.MoveNext();

        var user = new User
        {
          Id = i,
          FirstName = $"FName{i}",
          LastName = $"LName{i}",
          DateOfBirth = randomDayIterator.Current,
          AccountId = i,
          Username = $"UserName-{i}",
          Password = "e10adc3949ba59abbe56e057f20f883e"
        };
        users.Add(user);

        var account = new Account
        {
          Id = i,
          Name = $"Account{i}"
        };
        accounts.Add(account);

        for (int d = 0; d < 10000; d++, documentNumber++)
        {
          var documentPath = _fileSystem.Path.GetFullPath("TestDoc.txt");
          if (_fileSystem.File.Exists(documentPath))
          {
            var document = new Document
            {
              Id = documentNumber,
              Name = $"Document{i}-{d}.txt",
              FilePath = documentPath,
              Length = (int)_fileSystem.FileInfo.FromFileName(documentPath).Length,
              AccountId = i
            };
            documents.Add(document);
          }
        }
      }

      connection.Open();
      SqliteTransaction? transaction = null;

      try
      {
        if (connection.State == System.Data.ConnectionState.Open)
        {
          transaction = connection.BeginTransaction();
        }

        using (var command = _sqliteCommand.GetSQLiteCommand(connection))
        {
          if (transaction != null)
          {
            command.Transaction = transaction;
          }

          await _sqliteRepository.InsertUsers(command, users);
          await _sqliteRepository.InsertAccounts(command, accounts);
          await _sqliteRepository.InsertDocuments(command, documents);
        }

        transaction?.Commit();
      }
      catch
      {
        transaction?.Rollback();
        throw;
      }
      finally
      {
        connection.Close();
      }
    }

    static IEnumerable<DateTime> RandomDay()
    {
      DateTime start = new DateTime(1985, 1, 1);
      Random gen = new();
      int range = (DateTime.Today - start).Days;
      while (true)
        yield return start.AddDays(gen.Next(range));
    }
  }
}
