using System;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using SmartVault.Program.BusinessObjects;

namespace SmartVault.Program
{
  public class ProgramService : IProgramService
  {
    private readonly IDataAccess _dataAccess;
    private readonly IFileSystem _fileSystem;

    public ProgramService(IDataAccess dataAccess, IFileSystem fileSystem = null)
    {
      _dataAccess = dataAccess;
      _fileSystem = fileSystem ?? new FileSystem();
    }

    public async Task WriteEveryThirdFileToFileAsync(string accountId, SqliteConnection connection)
    {
      var outputFilePath = "OutputFile.txt";
      var documents = (await _dataAccess.QueryDocumentsAsync($"SELECT * FROM Document WHERE AccountId = {accountId}")).ToList();

      using (var writer = _fileSystem.File.CreateText(outputFilePath))
      {
        for (int i = 2; i < documents.Count; i += 3)
        {
          var document = documents[i];
          var fileContent = await _fileSystem.File.ReadAllTextAsync(document.FilePath);

          if (fileContent.Contains("Smith Property"))
          {
            await writer.WriteLineAsync(fileContent);
            await writer.WriteLineAsync();
          }
        }
      }

      Console.WriteLine($"Contents of every third file containing 'Smith Property' have been written to {outputFilePath}");
    }

    public async Task<long> GetAllFileSizesAsync()
    {
      var documents = (await _dataAccess.QueryDocumentsAsync("SELECT * FROM Document")).ToList();
      long totalSize = 0;

      foreach (var document in documents)
      {
        var fileInfo = _fileSystem.FileInfo.FromFileName(document.FilePath);
        if (fileInfo.Exists)
        {
          totalSize += fileInfo.Length;
        }
      }

      Console.WriteLine($"Total file size of all files: {totalSize} bytes");
      return totalSize;
    }
  }
}
