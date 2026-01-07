using Abstractions.Logic;
using Service.Services;

namespace Service.Tests;

public class ResourceObserverServiceTests
{
    [Fact(DisplayName = $"{nameof(ResourceObserverService)} could be created with valid params")]
    [Trait("Category", "Unit")]
    public void ResourceObserverServiceCouldBeCreatedWithValidParams()
    {
        // Arrange
        var resourceObserver = new Mock<IResourcesObserver>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() =>
        {
            _ = new ResourceObserverService(resourceObserver.Object);
        });

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"{nameof(ResourceObserverService)} can't be created without resource observer")]
    [Trait("Category", "Unit")]
    public void ResourceObserverServiceCannotBeCreatedWithoutResourceObserver()
    {
        // Arrange
        IResourcesObserver resourceObserver = null!;

        // Act
        var exception = Record.Exception(() =>
        {
            _ = new ResourceObserverService(resourceObserver);
        });

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(ResourceObserverService)} can be started")]
    [Trait("Category", "Unit")]
    public async Task ResourceObserverServiceCanBeStarted()
    {
        // Arrange
        var resourceObserver = new Mock<IResourcesObserver>(MockBehavior.Strict);
        var tcs = new TaskCompletionSource();
        using var stoppingToken = new CancellationTokenSource();
        var service = new ResourceObserverService(resourceObserver.Object);
        var runCount = 0;
        resourceObserver.Setup(x =>
                x.ObserveAsync(It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask)
            .Callback(() =>
            {
                tcs.TrySetResult(); 
                runCount++;
            });

        // Act
        await service.StartAsync(stoppingToken.Token);
        await tcs.Task; // Because of https://github.com/dotnet/runtime/pull/116283
        // Assert
        runCount.Should().Be(1);
    }

    [Fact(DisplayName = $"{nameof(ResourceObserverService)} can be stopped correctly")]
    [Trait("Category", "Unit")]
    public async Task ResourceObserverServiceCanBeStoppedCorrectly()
    {
        // Arrange
        var resourceObserver = new Mock<IResourcesObserver>(MockBehavior.Strict);
        using var stoppingToken = new CancellationTokenSource();
        var service = new ResourceObserverService(resourceObserver.Object);
        resourceObserver.Setup(x =>
                x.ObserveAsync(It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
        {
            await service.StopAsync(stoppingToken.Token);
        });

        // Assert
        exception.Should().BeNull();
    }
}