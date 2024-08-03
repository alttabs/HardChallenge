using System.Collections.Generic;
using System.Threading.Tasks;
using SmartVault.Program.BusinessObjects;

namespace SmartVault.Program
{
  public interface IDataAccess
  {
    Task<IEnumerable<Document>> QueryDocumentsAsync(string sql);
  }
}
