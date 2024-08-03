using Moq;
using System.Collections.Concurrent;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using Xunit;
using SmartVault.DataGeneration;
using SmartVault.DataGeneration.Repository;
using SmartVault.DataGeneration.Persistence;
using SmartVault.Program.BusinessObjects;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace SmartVault.Tests
{
    public class AsyncDataSeederTests
    {
        [Fact]
        public async Task SeedDataAsync_ShouldInsertData()
        {
            var mockRepo = new Mock<IDataGenerationRepository>();
            var mockCommand = new Mock<ISqliteCommand>();
            var mockSqliteCommand = new Mock<SqliteCommand>();

            mockCommand.Setup(c => c.GetSQLiteCommand(It.IsAny<SqliteConnection>())).Returns(mockSqliteCommand.Object);
            mockSqliteCommand.SetupProperty(c => c.Transaction);

            using var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { "TestDoc.txt", new MockFileData("This is my test document with Smith Property") }
            });

            var seeder = new AsyncDataSeeder(mockRepo.Object, mockCommand.Object, mockFileSystem);

            await seeder.SeedDataAsync(connection);

            mockRepo.Verify(r => r.InsertUsers(It.IsAny<SqliteCommand>(), It.IsAny<ConcurrentBag<User>>()), Times.Once);
            mockRepo.Verify(r => r.InsertAccounts(It.IsAny<SqliteCommand>(), It.IsAny<ConcurrentBag<Account>>()), Times.Once);
            mockRepo.Verify(r => r.InsertDocuments(It.IsAny<SqliteCommand>(), It.IsAny<ConcurrentBag<Document>>()), Times.Once);
        }
    }
}
