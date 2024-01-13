using AutoMapper;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Abstractions.Transport;
using Models;
using Transport.Configuration;

using TModel = Transport.Models;

namespace Transport.Tests;

public class ReportSenderTests
{
    [Fact(DisplayName = $"{nameof(ReportSender)} can be created with valid params")]
    [Trait("Category", "Unit")]
    public void CanBeCreatedWithValidParams()
    {
        // Arrange
        var logger = new Mock<ILogger<ReportSender>>();
        var mapper = new Mock<IMapper>(MockBehavior.Strict);
        var rawSender = new Mock<IRawSender<string>>(MockBehavior.Strict);
        var serializer = new Mock<ISerializer<TModel.HealthCheckReport, string>>(MockBehavior.Strict);
        var config = new Mock<IOptions<ReportSenderConfiguration>>(MockBehavior.Strict);
        var configData = new ReportSenderConfiguration
        {
            Url = new Uri("http://example.com"),
            Timeout = TimeSpan.FromSeconds(10)
        };
        config.Setup(x => x.Value).Returns(configData);

        // Act
        var exception = Record.Exception(() =>
        {
            _ = new ReportSender(
                logger.Object,
                mapper.Object,
                rawSender.Object,
                serializer.Object,
                config.Object);
        });

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"{nameof(ReportSender)} can't be created without logger")]
    [Trait("Category", "Unit")]
    public void CannotBeCreatedWithoutLogger()
    {
        // Arrange
        var mapper = new Mock<IMapper>(MockBehavior.Strict);
        var rawSender = new Mock<IRawSender<string>>(MockBehavior.Strict);
        var serializer = new Mock<ISerializer<TModel.HealthCheckReport, string>>(MockBehavior.Strict);
        var config = new Mock<IOptions<ReportSenderConfiguration>>(MockBehavior.Strict);
        var configData = new ReportSenderConfiguration
        {
            Url = new Uri("http://example.com"),
            Timeout = TimeSpan.FromSeconds(10)
        };
        config.Setup(x => x.Value).Returns(configData);

        // Act
        var exception = Record.Exception(() =>
        {
            _ = new ReportSender(
                null!,
                mapper.Object,
                rawSender.Object,
                serializer.Object,
                config.Object);
        });

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(ReportSender)} can't be created without mapper")]
    [Trait("Category", "Unit")]
    public void CannotBeCreatedWithoutMapper()
    {
        // Arrange
        var logger = new Mock<ILogger<ReportSender>>();
        var rawSender = new Mock<IRawSender<string>>(MockBehavior.Strict);
        var serializer = new Mock<ISerializer<TModel.HealthCheckReport, string>>(MockBehavior.Strict);
        var config = new Mock<IOptions<ReportSenderConfiguration>>(MockBehavior.Strict);
        var configData = new ReportSenderConfiguration
        {
            Url = new Uri("http://example.com"),
            Timeout = TimeSpan.FromSeconds(10)
        };
        config.Setup(x => x.Value).Returns(configData);

        // Act
        var exception = Record.Exception(() =>
        {
            _ = new ReportSender(
                logger.Object,
                null!,
                rawSender.Object,
                serializer.Object,
                config.Object);
        });

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(ReportSender)} can't be created without raw sender")]
    [Trait("Category", "Unit")]
    public void CannotBeCreatedWithoutRawSender()
    {
        // Arrange
        var logger = new Mock<ILogger<ReportSender>>();
        var mapper = new Mock<IMapper>(MockBehavior.Strict);
        var serializer = new Mock<ISerializer<TModel.HealthCheckReport, string>>(MockBehavior.Strict);
        var config = new Mock<IOptions<ReportSenderConfiguration>>(MockBehavior.Strict);
        var configData = new ReportSenderConfiguration
        {
            Url = new Uri("http://example.com"),
            Timeout = TimeSpan.FromSeconds(10)
        };
        config.Setup(x => x.Value).Returns(configData);

        // Act
        var exception = Record.Exception(() =>
        {
            _ = new ReportSender(
                logger.Object,
                mapper.Object,
                null!,
                serializer.Object,
                config.Object);
        });

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(ReportSender)} can't be created without serializer")]
    [Trait("Category", "Unit")]
    public void CannotBeCreatedWithoutSerializer()
    {
        // Arrange
        var logger = new Mock<ILogger<ReportSender>>();
        var mapper = new Mock<IMapper>(MockBehavior.Strict);
        var rawSender = new Mock<IRawSender<string>>(MockBehavior.Strict);
        var config = new Mock<IOptions<ReportSenderConfiguration>>(MockBehavior.Strict);
        var configData = new ReportSenderConfiguration
        {
            Url = new Uri("http://example.com"),
            Timeout = TimeSpan.FromSeconds(10)
        };
        config.Setup(x => x.Value).Returns(configData);

        // Act
        var exception = Record.Exception(() =>
        {
            _ = new ReportSender(
                logger.Object,
                mapper.Object,
                rawSender.Object,
                null!,
                config.Object);
        });

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(ReportSender)} can't be created without config")]
    [Trait("Category", "Unit")]
    public void CannotBeCreatedWithoutConfig()
    {
        // Arrange
        var logger = new Mock<ILogger<ReportSender>>();
        var mapper = new Mock<IMapper>(MockBehavior.Strict);
        var rawSender = new Mock<IRawSender<string>>(MockBehavior.Strict);
        var serializer = new Mock<ISerializer<TModel.HealthCheckReport, string>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() =>
        {
            _ = new ReportSender(
                logger.Object,
                mapper.Object,
                rawSender.Object,
                serializer.Object,
                null!);
        });

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(ReportSender)} can't send null report")]
    [Trait("Category", "Unit")]
    public async Task CannotSendNullReport()
    {
        // Arrange
        var logger = new Mock<ILogger<ReportSender>>();
        var mapper = new Mock<IMapper>(MockBehavior.Strict);
        var rawSender = new Mock<IRawSender<string>>(MockBehavior.Strict);
        var serializer = new Mock<ISerializer<TModel.HealthCheckReport, string>>(MockBehavior.Strict);
        var config = new Mock<IOptions<ReportSenderConfiguration>>(MockBehavior.Strict);
        var configData = new ReportSenderConfiguration
        {
            Url = new Uri("http://example.com"),
            Timeout = TimeSpan.FromSeconds(10)
        };
        config.Setup(x => x.Value).Returns(configData);
        var reportSender = new ReportSender(
            logger.Object,
            mapper.Object,
            rawSender.Object,
            serializer.Object,
            config.Object);
        using var cts = new CancellationTokenSource();

        // Act
        var exception = await Record.ExceptionAsync(() =>
            reportSender.SendAsync(null!, cts.Token)
        );

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(ReportSender)} can't send report with cancelled token")]
    [Trait("Category", "Unit")]
    public async Task CannotSendReportWithCancelledToken()
    {
        // Arrange
        var logger = new Mock<ILogger<ReportSender>>();
        var mapper = new Mock<IMapper>(MockBehavior.Strict);
        var rawSender = new Mock<IRawSender<string>>(MockBehavior.Strict);
        var serializer = new Mock<ISerializer<TModel.HealthCheckReport, string>>(MockBehavior.Strict);
        var config = new Mock<IOptions<ReportSenderConfiguration>>(MockBehavior.Strict);
        var configData = new ReportSenderConfiguration
        {
            Url = new Uri("http://example.com"),
            Timeout = TimeSpan.FromSeconds(10)
        };
        config.Setup(x => x.Value).Returns(configData);
        var reportSender = new ReportSender(
            logger.Object,
            mapper.Object,
            rawSender.Object,
            serializer.Object,
            config.Object);
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        var url = new Uri("https://example.com");
        var checkInterval = TimeSpan.FromSeconds(20);
        var timeout = TimeSpan.FromSeconds(60);
        var res1 = new ResourceName("Resource1");
        var res2 = new ResourceName("Resource2");
        var settings = new ResourceRequestSettings(url, checkInterval, timeout);
        var resourceHealthChecks = new List<ResourceHealthCheck>
        {
            new(res1, TimeSpan.FromSeconds(30),settings),
            new(res2, TimeSpan.FromSeconds(45),settings)
        };
        var report = new HealthCheckReport(resourceHealthChecks);
        
        // Act
        var exception = await Record.ExceptionAsync(() =>
            reportSender.SendAsync(report, cts.Token)
        );
        
        // Assert
        exception.Should().BeOfType<OperationCanceledException>();
    }
}