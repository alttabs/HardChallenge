using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using SmartVault.Program.BusinessObjects;

namespace SmartVault.Program
{
  public class DataAccess : IDataAccess
  {
    private readonly IDbConnection _dbConnection;

    public DataAccess(IDbConnection dbConnection)
    {
      _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<Document>> QueryDocumentsAsync(string sql)
    {
      return await _dbConnection.QueryAsync<Document>(sql);
    }
  }
}
