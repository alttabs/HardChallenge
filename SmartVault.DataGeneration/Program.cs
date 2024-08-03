using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SmartVault.DataGeneration
{
    partial class Program
    {
        private static ConcurrentBag<int> userIds = [];

        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            var databaseFileName = configuration["DatabaseFileName"];
            if (File.Exists(databaseFileName))
            {
                File.Delete(databaseFileName);
            }

            File.WriteAllText("TestDoc.txt", GenerateTestDocumentContent());

            CreateDatabase(configuration);

            Parallel.For(0, 100, i =>
            {
                using (var connection = new SqliteConnection(configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    using var transaction = connection.BeginTransaction();
                    var randomDay = RandomDay();
                    var createdOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    int userId = GetUniqueUserId();
                    connection.Execute($"INSERT INTO User (Id, FirstName, LastName, DateOfBirth, AccountId, Username, Password, CreatedOn) VALUES('{userId}','FName{userId}','LName{userId}','{randomDay.ToString("yyyy-MM-dd")}','{userId}','UserName-{userId}','e10adc3949ba59abbe56e057f20f883e','{createdOn}')", transaction);
                    connection.Execute($"INSERT INTO Account (Id, Name, CreatedOn) VALUES('{userId}','Account{userId}','{createdOn}')", transaction);

                    var documentInserts = new StringBuilder();
                    var documentPath = new FileInfo("TestDoc.txt").FullName;
                    var documentLength = new FileInfo(documentPath).Length;

                    for (int d = 0; d < 1000; d++)
                    {
                        documentInserts.Append($"INSERT INTO Document (Id, Name, FilePath, Length, AccountId, CreatedOn) VALUES('{userId * 1000 + d}','Document{userId}-{d}.txt','{documentPath}','{documentLength}','{userId}','{createdOn}');");
                    }

                    connection.Execute(documentInserts.ToString(), transaction);
                    transaction.Commit();
                }
            });
        }

        static void CreateDatabase(IConfiguration configuration)
        {
            using (var connection = new SqliteConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                CreateTables(connection);
            }
        }

        static void CreateTables(SqliteConnection connection)
        {
            var createAccountTable = @"
            CREATE TABLE Account (
                Id INTEGER PRIMARY KEY,
                Name TEXT,
                CreatedOn DATETIME DEFAULT CURRENT_TIMESTAMP
            );";

            var createDocumentTable = @"
            CREATE TABLE Document (
                Id INTEGER PRIMARY KEY,
                Name TEXT,
                FilePath TEXT,
                Length INTEGER,
                AccountId INTEGER,
                CreatedOn DATETIME DEFAULT CURRENT_TIMESTAMP
            );";

            var createUserTable = @"
            CREATE TABLE User (
                Id INTEGER PRIMARY KEY,
                FirstName TEXT,
                LastName TEXT,
                DateOfBirth TEXT,
                AccountId INTEGER,
                Username TEXT,
                Password TEXT,
                CreatedOn DATETIME DEFAULT CURRENT_TIMESTAMP
            );";

            var createOAuthIntegrationTable = @"
            CREATE TABLE OAuthIntegration (
                Id INTEGER PRIMARY KEY,
                Provider TEXT,
                ClientId TEXT,
                ClientSecret TEXT,
                CreatedOn DATETIME DEFAULT CURRENT_TIMESTAMP
            );";

            connection.Execute(createAccountTable);
            connection.Execute(createDocumentTable);
            connection.Execute(createUserTable);
            connection.Execute(createOAuthIntegrationTable);
        }

        static int GetUniqueUserId()
        {
            int newUserId;
            lock (userIds)
            {
                newUserId = userIds.Count;
                userIds.Add(newUserId);
            }
            return newUserId;
        }

        static string GenerateTestDocumentContent()
        {
            var content = new StringBuilder();
            for (int i = 0; i < 100; i++)
            {
                if (i % 10 == 0)
                {
                    content.AppendLine("This is my test document with Smith Property");
                }
                else
                {
                    content.AppendLine("This is my test document");
                }
            }
            return content.ToString();
        }

        static DateTime RandomDay()
        {
            DateTime start = new DateTime(1985, 1, 1);
            Random gen = new Random();
            int range = (DateTime.Today - start).Days;
            return start.AddDays(gen.Next(range));
        }
    }
}
