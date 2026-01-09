using Abstractions.State;
using Abstractions.Transport;
using Logic.Configuration;
using Models;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Logic.Tests;

public class ReportProcessorTests
{
    [Fact(DisplayName = $"{nameof(ReportProcessor)} can be created with valid params")]
    [Trait("Category", "Unit")]
    public void CanBeCreatedWithValidParams()
    {
        // Arrange
        var config = new Mock<IOptions<ReportProcessorConfiguration>>(MockBehavior.Strict);
        config.Setup(x => x.Value).Returns(() => null!);
        var logger = Mock.Of<ILogger<ReportProcessor>>();
        var healthChecksState = Mock.Of<IHealthChecksState>(MockBehavior.Strict);
        var reportSender = Mock.Of<IReportSender>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() =>
            _ = new ReportProcessor(
                config.Object,
                logger,
                healthChecksState,
                reportSender));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"{nameof(ReportProcessor)} can't be created with null config")]
    [Trait("Category", "Unit")]
    public void CannotBeCreatedWithNullConfig()
    {
        // Arrange
        var logger = Mock.Of<ILogger<ReportProcessor>>();
        var healthChecksState = Mock.Of<IHealthChecksState>(MockBehavior.Strict);
        var reportSender = Mock.Of<IReportSender>(MockBehavior.Strict);
        // Act
        var exception = Record.Exception(() =>
            _ = new ReportProcessor(
                null!,
                logger,
                healthChecksState,
                reportSender));
        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(ReportProcessor)} can't be created with null logger")]
    [Trait("Category", "Unit")]
    public void CannotBeCreatedWithNullLogger()
    {
        // Arrange
        var config = new Mock<IOptions<ReportProcessorConfiguration>>(MockBehavior.Strict);
        config.Setup(x => x.Value).Returns(() => null!);
        var healthChecksState = Mock.Of<IHealthChecksState>(MockBehavior.Strict);
        var reportSender = Mock.Of<IReportSender>(MockBehavior.Strict);
        // Act
        var exception = Record.Exception(() =>
            _ = new ReportProcessor(
                config.Object,
                null!,
                healthChecksState,
                reportSender));
        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(ReportProcessor)} can't be created with null health check state")]
    [Trait("Category", "Unit")]
    public void CannotBeCreatedWithNullHealthCheckState()
    {
        // Arrange
        var config = new Mock<IOptions<ReportProcessorConfiguration>>(MockBehavior.Strict);
        config.Setup(x => x.Value).Returns(() => null!);
        var logger = Mock.Of<ILogger<ReportProcessor>>();
        var reportSender = Mock.Of<IReportSender>(MockBehavior.Strict);
        // Act
        var exception = Record.Exception(() =>
            _ = new ReportProcessor(
                config.Object,
                logger,
                null!,
                reportSender));
        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(ReportProcessor)} can't be created with null report sender")]
    [Trait("Category", "Unit")]
    public void CannotBeCreatedWithNullReportSender()
    {
        // Arrange
        var config = new Mock<IOptions<ReportProcessorConfiguration>>(MockBehavior.Strict);
        config.Setup(x => x.Value).Returns(() => null!);
        var logger = Mock.Of<ILogger<ReportProcessor>>();
        var healthChecksState = Mock.Of<IHealthChecksState>(MockBehavior.Strict);
        // Act
        var exception = Record.Exception(() =>
            _ = new ReportProcessor(
                config.Object,
                logger,
                healthChecksState,
                null!));
        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(ReportProcessor)} not executed processing on cancelled token")]
    [Trait("Category", "Unit")]
    public async Task NotExecutedProcessingOnCancelledToken()
    {
        // Arrange
        var config = new Mock<IOptions<ReportProcessorConfiguration>>(MockBehavior.Strict);
        config.Setup(x => x.Value)
            .Returns(() => null!);
        var logger = Mock.Of<ILogger<ReportProcessor>>();
        var healthChecksState = Mock.Of<IHealthChecksState>(MockBehavior.Strict);
        var reportSender = Mock.Of<IReportSender>(MockBehavior.Strict);
        var processor = new ReportProcessor(config.Object, logger, healthChecksState, reportSender);
        using var cancellationTokenSource = new CancellationTokenSource();
        await cancellationTokenSource.CancelAsync();

        // Act
        var exception = await Record.ExceptionAsync(async () =>
        await processor.ProcessAsync(cancellationTokenSource.Token));

        // Assert
        exception.Should().BeOfType<OperationCanceledException>();
    }

    [Fact(DisplayName = $"{nameof(ReportProcessor)} processing but not send unhealthy report")]
    [Trait("Category", "Unit")]
    public async Task ProcessingNotHealthReport()
    {
        // Arrange
        var reportBuildCount = 0;
        using var cancellationTokenSource = new CancellationTokenSource();
        var report = new HealthCheckReport(
        [
            new ResourceHealthCheck(
                new ResourceName("test"),
                TimeSpan.FromMicroseconds(1),
                new ResourceRequestSettings(
                    new Uri("http://example.com"),
                    TimeSpan.FromMicroseconds(1),
                    TimeSpan.FromMicroseconds(1)))
        ]);
        var tcs = new TaskCompletionSource();
        var config = new Mock<IOptions<ReportProcessorConfiguration>>(MockBehavior.Strict);
        config.Setup(x => x.Value)
            .Returns(() => new ReportProcessorConfiguration
            {
                SendInterval = TimeSpan.FromSeconds(10)
            });
        var logger = Mock.Of<ILogger<ReportProcessor>>();
        var healthChecksState = new Mock<IHealthChecksState>(MockBehavior.Strict);
        healthChecksState.Setup(x => x.BuildReport())
            .Returns(report).Callback(() =>
            {
                reportBuildCount++;
                cancellationTokenSource.Cancel();
            });
        var reportSender = Mock.Of<IReportSender>(MockBehavior.Strict);
        var processor = new ReportProcessor(
            config.Object,
            logger,
            healthChecksState.Object,
            reportSender);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await processor.ProcessAsync(cancellationTokenSource.Token));

        // Assert
        reportBuildCount.Should().Be(1);
        exception.Should().BeAssignableTo<OperationCanceledException>();
    }

    [Fact(DisplayName = $"{nameof(ReportProcessor)} processing and send healthy report")]
    [Trait("Category", "Unit")]
    public async Task ProcessingHealthReport()
    {
        // Arrange
        var reportBuildCount = 0;
        var reportSendCount = 0;
        using var cancellationTokenSource = new CancellationTokenSource();
        var reportItem = new ResourceHealthCheck(
            new ResourceName("test"),
            TimeSpan.FromDays(1),
            new ResourceRequestSettings(
                new Uri("http://example.com"),
                TimeSpan.FromDays(1),
                TimeSpan.FromDays(1)));
        reportItem.Update();
        var report = new HealthCheckReport(
        [
            reportItem
        ]);
        var tcs = new TaskCompletionSource();
        var config = new Mock<IOptions<ReportProcessorConfiguration>>(MockBehavior.Strict);
        config.Setup(x => x.Value)
            .Returns(() => new ReportProcessorConfiguration
            {
                SendInterval = TimeSpan.FromSeconds(10)
            });
        var logger = Mock.Of<ILogger<ReportProcessor>>();
        var healthChecksState = new Mock<IHealthChecksState>(MockBehavior.Strict);
        healthChecksState.Setup(x => x.BuildReport())
            .Returns(report).Callback(() => reportBuildCount++);
        var reportSender = new Mock<IReportSender>(MockBehavior.Strict);
        reportSender.Setup(x =>
                x.SendAsync(report, cancellationTokenSource.Token))
            .Returns(Task.CompletedTask)
            .Callback(() =>
            {
                reportSendCount++;
                cancellationTokenSource.Cancel();
            });
        var processor = new ReportProcessor(
            config.Object,
            logger,
            healthChecksState.Object,
            reportSender.Object);

        // Act
        var exception = await Record.ExceptionAsync(async ()
            => await processor.ProcessAsync(cancellationTokenSource.Token));

        // Assert
        reportBuildCount.Should().Be(1);
        reportSendCount.Should().Be(1);
        exception.Should().BeAssignableTo<OperationCanceledException>();
    }
}