using Microsoft.Data.Sqlite;

namespace SmartVault.DataGeneration.Persistence
{
  public class SqliteCommandWrapper : ISqliteCommand
  {
    public SqliteCommand GetSQLiteCommand(SqliteConnection connection)
    {
      return connection.CreateCommand();
    }
  }
}
