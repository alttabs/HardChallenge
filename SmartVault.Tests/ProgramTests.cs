using Moq;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using Xunit;
using SmartVault.Program;
using SmartVault.Program.BusinessObjects;

namespace SmartVault.Tests
{
  public class ProgramTests
  {
    [Fact]
    public async Task WriteEveryThirdFileToFileAsync_ShouldWriteCorrectContent()
    {

      var mockDataAccess = new Mock<IDataAccess>();
      var documents = new List<Document>
            {
                new Document { FilePath = "TestDoc1.txt" },
                new Document { FilePath = "TestDoc2.txt" },
                new Document { FilePath = "TestDoc3.txt" },
                new Document { FilePath = "TestDoc4.txt" },
                new Document { FilePath = "TestDoc5.txt" },
                new Document { FilePath = "TestDoc6.txt" },
            };

      mockDataAccess.Setup(d => d.QueryDocumentsAsync(It.IsAny<string>())).ReturnsAsync(documents);

      var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { "TestDoc1.txt", new MockFileData("This is my test document") },
                { "TestDoc2.txt", new MockFileData("This is my test document") },
                { "TestDoc3.txt", new MockFileData("This is my test document with Smith Property") },
                { "TestDoc4.txt", new MockFileData("This is my test document") },
                { "TestDoc5.txt", new MockFileData("This is my test document") },
                { "TestDoc6.txt", new MockFileData("This is my test document") }
            });

      var programService = new ProgramService(mockDataAccess.Object, mockFileSystem);

      await programService.WriteEveryThirdFileToFileAsync("1", null);

      var outputFilePath = "OutputFile.txt";
      var outputContent = mockFileSystem.File.ReadAllText(outputFilePath);
      Assert.Contains("This is my test document with Smith Property", outputContent);
    }

    [Fact]
    public async Task GetAllFileSizesAsync_ShouldCalculateCorrectTotalSize()
    {
      // Arrange
      var mockDataAccess = new Mock<IDataAccess>();
      var documents = new List<Document>
    {
        new Document { FilePath = "TestDoc1.txt", Length = 100 },
        new Document { FilePath = "TestDoc2.txt", Length = 200 },
        new Document { FilePath = "TestDoc3.txt", Length = 300 },
    };

      mockDataAccess.Setup(d => d.QueryDocumentsAsync(It.IsAny<string>())).ReturnsAsync(documents);

      var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
    {
        { "TestDoc1.txt", new MockFileData(new byte[100]) },
        { "TestDoc2.txt", new MockFileData(new byte[200]) },
        { "TestDoc3.txt", new MockFileData(new byte[300]) }
    });

      var programService = new ProgramService(mockDataAccess.Object, mockFileSystem);

      var totalSize = await programService.GetAllFileSizesAsync();

      Assert.Equal(600, totalSize);
    }

  }
}
