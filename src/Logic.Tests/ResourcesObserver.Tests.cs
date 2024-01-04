using Abstractions.Logic;
using Abstractions.State;
using Models;

using Microsoft.Extensions.Logging;

namespace Logic.Tests;

public class ResourcesObserverTests
{
    [Fact(DisplayName = $"{nameof(ResourcesObserver)} can be created with valid params")]
    [Trait("Category", "Unit")]
    public void CanBeCreatedWithValidParams()
    {
        // Arrange
        var logger = new Mock<ILogger<ResourcesObserver>>();
        var state = new Mock<IHealthChecksState>(MockBehavior.Strict);
        var processorFactory =
            new Mock<Func<ResourceHealthCheck, IResourceCheckerProcessor>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() =>
            _ = new ResourcesObserver(logger.Object, state.Object, processorFactory.Object));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"{nameof(ResourcesObserver)} cant be created without logger")]
    [Trait("Category", "Unit")]
    public void CantBeCreatedWithoutLogger()
    {
        // Arrange
        var state = new Mock<IHealthChecksState>(MockBehavior.Strict);
        var processorFactory =
            new Mock<Func<ResourceHealthCheck, IResourceCheckerProcessor>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() =>
            _ = new ResourcesObserver(null!, state.Object, processorFactory.Object));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(ResourcesObserver)} cant be created without state")]
    [Trait("Category", "Unit")]
    public void CantBeCreatedWithoutState()
    {
        // Arrange
        var logger = new Mock<ILogger<ResourcesObserver>>();
        var processorFactory =
            new Mock<Func<ResourceHealthCheck, IResourceCheckerProcessor>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() =>
            _ = new ResourcesObserver(logger.Object, null!, processorFactory.Object));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(ResourcesObserver)} cant be created without processor factory")]
    [Trait("Category", "Unit")]
    public void CantBeCreatedWithoutProcessorFactory()
    {
        // Arrange
        var logger = new Mock<ILogger<ResourcesObserver>>();
        var state = new Mock<IHealthChecksState>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() =>
            _ = new ResourcesObserver(logger.Object, state.Object, null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(ResourcesObserver)} cant observe with cancelled token")]
    [Trait("Category", "Unit")]
    public async Task CantObserveWithCancelledToken()
    {
        // Arrange
        var logger = new Mock<ILogger<ResourcesObserver>>();
        var state = new Mock<IHealthChecksState>(MockBehavior.Strict);
        var processorFactory =
            new Mock<Func<ResourceHealthCheck, IResourceCheckerProcessor>>(MockBehavior.Strict);
        var observer = new ResourcesObserver(logger.Object, state.Object, processorFactory.Object);
        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await observer.ObserveAsync(cancellationTokenSource.Token));

        // Assert
        exception.Should().BeOfType<OperationCanceledException>();
    }

    [Fact(DisplayName = $"{nameof(ResourcesObserver)} observes data")]
    [Trait("Category", "Unit")]
    public async Task ObservesData()
    {
        // Arrange
        var factoryCallCount = 0;
        var processorCallCount = 0;
        var logger = new Mock<ILogger<ResourcesObserver>>();
        var state = new Mock<IHealthChecksState>(MockBehavior.Strict);
        var processorFactory =
            new Mock<Func<ResourceHealthCheck, IResourceCheckerProcessor>>(MockBehavior.Strict);
        var observer = new ResourcesObserver(logger.Object, state.Object, processorFactory.Object);
        using var cancellationTokenSource = new CancellationTokenSource();
        var resource = new ResourceHealthCheck(
            new ResourceName("Resource1"),
            TimeSpan.FromMinutes(5),
            new ResourceRequestSettings(
                new Uri("http://example.com"),
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(1)));
        var resourceProcessor = new Mock<IResourceCheckerProcessor>(MockBehavior.Strict);
        resourceProcessor.Setup(x => x.ProcessAsync(cancellationTokenSource.Token))
            .Returns(Task.CompletedTask)
            .Callback(() => factoryCallCount++);
        var resources = new List<ResourceHealthCheck>()
        {
            resource
        };
        state.Setup(s => s.HealthChecks)
            .Returns(resources);
        processorFactory.Setup(f => f(resource))
            .Returns(resourceProcessor.Object)
            .Callback(() => processorCallCount++);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await observer.ObserveAsync(cancellationTokenSource.Token));

        // Assert
        exception.Should().BeNull();
        factoryCallCount.Should().Be(1);
        processorCallCount.Should().Be(1);
    }
}