using Microsoft.Data.Sqlite;

namespace SmartVault.DataGeneration.Persistence
{
  public interface ISqliteCommand
  {
    SqliteCommand GetSQLiteCommand(SqliteConnection connection);
  }
}
