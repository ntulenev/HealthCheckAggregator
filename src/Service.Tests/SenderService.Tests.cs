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

    [Fact(DisplayName = $"{nameof(SenderService)} can start sender service")]
    [Trait("Category", "Unit")]
    public async Task SenderServiceCanStartSenderService()
    {
        // Arrange
        var reportProcessor = new Mock<IReportProcessor>(MockBehavior.Strict);
        using var stoppingToken = new CancellationTokenSource();
        using var senderService = new SenderService(reportProcessor.Object);
        var runCount = 0;
        reportProcessor.Setup(x =>
                x.ProcessAsync(It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask)
            .Callback(() => runCount++);

        // Act
        await senderService.StartAsync(stoppingToken.Token);

        // Assert
        runCount.Should().Be(1);
    }


    [Fact(DisplayName = $"{nameof(SenderService)} stops correctly")]
    [Trait("Category", "Unit")]
    public async Task SenderServiceStopsCorrectly()
    {
        // Arrange
        var reportProcessor = new Mock<IReportProcessor>(MockBehavior.Strict);
        using var stoppingToken = new CancellationTokenSource();
        using var senderService = new SenderService(reportProcessor.Object);
        reportProcessor.Setup(x =>
                x.ProcessAsync(It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask);
        await senderService.StartAsync(stoppingToken.Token);

        // Act
        var exception = await Record.ExceptionAsync(async () => { await senderService.StopAsync(stoppingToken.Token); });

        // Assert
        exception.Should().BeNull();
    }
}