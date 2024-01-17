using Abstractions.Logic;
using Service.Services;

namespace Service.Tests;

public class SenderServiceTests
{
    [Fact(DisplayName = $"{nameof(SenderService)} could be created with valid params")]
    [Trait("Category", "Unit")]
    public void SenderServiceCouldBeCreatedWithValidParams()
    {
        // Arrange
        var reportProcessor = new Mock<IReportProcessor>(MockBehavior.Strict).Object;

        // Act
        var exception = Record.Exception(() => { _ = new SenderService(reportProcessor); });

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"{nameof(SenderService)} can't be created without report processor")]
    [Trait("Category", "Unit")]
    public void SenderServiceCannotBeCreatedWithoutReportProcessor()
    {
        // Arrange
        IReportProcessor reportProcessor = null!;
        
        // Act
        var exception = Record.Exception(() => { _ = new SenderService(reportProcessor); });
        
        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}