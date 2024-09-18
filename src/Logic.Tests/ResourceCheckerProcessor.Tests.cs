using Abstractions.Logic;
using Models;

using Microsoft.Extensions.Logging;

namespace Logic.Tests;

public class ResourceCheckerProcessorTests
{
    [Fact(DisplayName = $"{nameof(ResourceCheckerProcessor)} can be created with valid params")]
    [Trait("Category", "Unit")]
    public void ResourceCheckerProcessorCanBeCreatedWithValidParams()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<ResourceCheckerProcessor>>();
        var resourceChecker = new Mock<IResourceChecker>(MockBehavior.Strict);
        var resourceHealthCheck = new ResourceHealthCheck(
            new ResourceName("test"),
            TimeSpan.FromMicroseconds(1),
            new ResourceRequestSettings(
                new Uri("http://www.example.com"),
                TimeSpan.FromMicroseconds(1),
                TimeSpan.FromSeconds(1)));

        // Act
        var exception = Record.Exception(() =>
            _ = new ResourceCheckerProcessor(loggerMock.Object, resourceChecker.Object,
                resourceHealthCheck));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"{nameof(ResourceCheckerProcessor)} cant be created without logger")]
    [Trait("Category", "Unit")]
    public void ResourceCheckerProcessorCannotBeCreatedWithoutLogger()
    {
        // Arrange
        var resourceChecker = new Mock<IResourceChecker>(MockBehavior.Strict);
        var resourceHealthCheck = new ResourceHealthCheck(
            new ResourceName("test"),
            TimeSpan.FromMicroseconds(1),
            new ResourceRequestSettings(
                new Uri("http://www.example.com"),
                TimeSpan.FromMicroseconds(1),
                TimeSpan.FromSeconds(1)));

        // Act
        var exception = Record.Exception(() =>
            _ = new ResourceCheckerProcessor(null!, resourceChecker.Object,
                resourceHealthCheck));
        
        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(ResourceCheckerProcessor)} cant be created without resource checker")]
    [Trait("Category", "Unit")]
    public void ResourceCheckerProcessorCannotBeCreatedWithoutResourceChecker()
    {
        // Arrange
        var logger = new Mock<ILogger<ResourceCheckerProcessor>>();
        var resourceHealthCheck = new ResourceHealthCheck(
            new ResourceName("test"),
            TimeSpan.FromMicroseconds(1),
            new ResourceRequestSettings(
                new Uri("http://www.example.com"),
                TimeSpan.FromMicroseconds(1),
                TimeSpan.FromSeconds(1)));
        
        // Act
        var exception = Record.Exception(() =>
            _ = new ResourceCheckerProcessor(logger.Object, null!,
                resourceHealthCheck));
        
        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(ResourceCheckerProcessor)} cant be created without health check")]
    [Trait("Category", "Unit")]
    public void ResourceCheckerProcessorCannotBeCreatedWithoutHealthCheck()
    {
        // Arrange
        var logger = new Mock<ILogger<ResourceCheckerProcessor>>();
        var resourceChecker = new Mock<IResourceChecker>(MockBehavior.Strict);
        
        // Act
        var exception = Record.Exception(() =>
            _ = new ResourceCheckerProcessor(logger.Object, resourceChecker.Object,
                null!));
        
        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(ResourceCheckerProcessor)} cant process on cancelled token")]
    [Trait("Category", "Unit")]
    public async Task ResourceCheckerProcessorCannotProcessOnCancelledToken()
    {
        // Arrange
        var logger = new Mock<ILogger<ResourceCheckerProcessor>>();
        var resourceChecker = new Mock<IResourceChecker>(MockBehavior.Strict);
        var resourceHealthCheck = new ResourceHealthCheck(
            new ResourceName("test"),
            TimeSpan.FromMicroseconds(1),
            new ResourceRequestSettings(
                new Uri("http://www.example.com"),
                TimeSpan.FromMicroseconds(1),
                TimeSpan.FromSeconds(1)));
        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();
        var processor = new ResourceCheckerProcessor(logger.Object, resourceChecker.Object,
            resourceHealthCheck);

        // Act
        var exception =
            await Record.ExceptionAsync(async () =>
                await processor.ProcessAsync(cancellationTokenSource.Token));

        // Assert
        exception.Should().BeAssignableTo<OperationCanceledException>();
    }

    [Fact(DisplayName = $"{nameof(ResourceCheckerProcessor)} can process")]
    [Trait("Category", "Unit")]
    public async Task ResourceCheckerProcessorCanProcess()
    {
        // Arrange
        var logger = new Mock<ILogger<ResourceCheckerProcessor>>();
        var resourceChecker = new Mock<IResourceChecker>(MockBehavior.Strict);
        var resourceHealthCheck = new ResourceHealthCheck(
            new ResourceName("test"),
            TimeSpan.FromMicroseconds(1),
            new ResourceRequestSettings(
                new Uri("http://www.example.com"),
                TimeSpan.FromMicroseconds(1),
                TimeSpan.FromSeconds(1)));
        using var cancellationTokenSource = new CancellationTokenSource();
        var checkCallCount = 0;
        resourceChecker.Setup(x =>
                x.CheckAsync(resourceHealthCheck, cancellationTokenSource.Token))
            .Returns(Task.CompletedTask).Callback(() =>
            {
                cancellationTokenSource.Cancel();
                checkCallCount++;
            });
        
        // Act
        var processor = new ResourceCheckerProcessor(logger.Object, resourceChecker.Object,
            resourceHealthCheck);
        var exception =
            await Record.ExceptionAsync(async () =>
                await processor.ProcessAsync(cancellationTokenSource.Token));
        
        // Assert
        exception.Should().BeAssignableTo<OperationCanceledException>();
        checkCallCount.Should().Be(1);
    }
}