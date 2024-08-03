using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace SmartVault.Program
{
    partial class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                return;
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            var connectionString = string.Format(configuration["ConnectionStrings:DefaultConnection"], configuration["DatabaseFileName"]);
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var dataAccess = new DataAccess(connection);
                var fileSystem = new System.IO.Abstractions.FileSystem();
                var programService = new ProgramService(dataAccess, fileSystem);

                await programService.WriteEveryThirdFileToFileAsync(args[0], connection);
                await programService.GetAllFileSizesAsync();
            }
        }
    }
}
