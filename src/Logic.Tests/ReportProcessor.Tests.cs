using Abstractions.State;
using Abstractions.Transport;
using Logic.Configuration;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using FluentAssertions;

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

    [Fact(DisplayName = $"{nameof(ReportProcessor)} can't be created with null healthcheck state")]
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
}