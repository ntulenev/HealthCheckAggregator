using Abstractions.Transport;

using Microsoft.Extensions.Logging;

using Models;

namespace Logic.Tests;

public class ResourceCheckerTests
{
    [Fact(DisplayName = $"{nameof(ResourceChecker)} can be created with valid params")]
    [Trait("Category", "Unit")]
    public void ResourceCheckerCanBeCreatedWithValidParams()
    {
        // Arrange
        var rawChecker = new Mock<IRawResourceChecker>(MockBehavior.Strict).Object;
        var logger = new Mock<ILogger<ResourceChecker>>(MockBehavior.Strict).Object;

        // Act
        var exception = Record.Exception(() =>
            _ = new ResourceChecker(rawChecker, logger));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"{nameof(ResourceChecker)} can't be created without raw checker")]
    [Trait("Category", "Unit")]
    public void ResourceCheckerCantBeCreatedWithoutRawChecker()
    {
        // Arrange
        var logger = new Mock<ILogger<ResourceChecker>>(MockBehavior.Strict).Object;

        // Act
        var exception = Record.Exception(() =>
            _ = new ResourceChecker(null!, logger));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(ResourceChecker)} can't be created without logger")]
    [Trait("Category", "Unit")]
    public void ResourceCheckerCantBeCreatedWithoutLogger()
    {
        // Arrange
        var rawChecker = new Mock<IRawResourceChecker>(MockBehavior.Strict).Object;

        // Act
        var exception = Record.Exception(() =>
            _ = new ResourceChecker(rawChecker, null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(ResourceChecker)} can't check null resource")]
    [Trait("Category", "Unit")]
    public async Task ResourceCheckerCantCheckNullResource()
    {
        // Arrange
        var rawChecker = new Mock<IRawResourceChecker>(MockBehavior.Strict).Object;
        var logger = new Mock<ILogger<ResourceChecker>>(MockBehavior.Strict).Object;
        var resourceChecker = new ResourceChecker(rawChecker, logger);
        using var cts = new CancellationTokenSource();

        // Act
        var exception = await Record.ExceptionAsync(() =>
            resourceChecker.CheckAsync(null!, cts.Token));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(ResourceChecker)} can't check with cancelled token")]
    [Trait("Category", "Unit")]
    public async Task ResourceCheckerCantCheckWithCancelledToken()
    {
        // Arrange
        var rawChecker = new Mock<IRawResourceChecker>(MockBehavior.Strict).Object;
        var logger = new Mock<ILogger<ResourceChecker>>(MockBehavior.Strict).Object;
        var resourceChecker = new ResourceChecker(rawChecker, logger);
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        var targetResource = new ResourceHealthCheck(
            new ResourceName("test"),
            TimeSpan.FromMicroseconds(1),
            new ResourceRequestSettings(
                new Uri("http://example.com"),
                TimeSpan.FromMicroseconds(1),
                TimeSpan.FromMicroseconds(1)));

        // Act
        var exception = await Record.ExceptionAsync(() =>
            resourceChecker.CheckAsync(targetResource, cts.Token));

        // Assert
        exception.Should().BeOfType<OperationCanceledException>();
    }

    [Fact(DisplayName = $"{nameof(ResourceChecker)} should update resource on healthy status")]
    [Trait("Category", "Unit")]
    public async Task ResourceCheckerShouldUpdateResourceOnHealthyStatus()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var targetUri = new Uri("http://example.com");
        var timeout = TimeSpan.FromSeconds(42);
        var rawChecker = new Mock<IRawResourceChecker>(MockBehavior.Strict);
        rawChecker.Setup(x => x.CheckAsync(
                timeout,
                targetUri,
                cts.Token))
            .ReturnsAsync(ResourceStatus.Healthy);
        var logger = new Mock<ILogger<ResourceChecker>>(MockBehavior.Strict).Object;
        var resourceChecker = new ResourceChecker(rawChecker.Object, logger);
        var targetResource = new ResourceHealthCheck(
            new ResourceName("test"),
            TimeSpan.FromMicroseconds(1),
            new ResourceRequestSettings(
                targetUri,
                TimeSpan.FromMicroseconds(1),
                timeout));
        // Act

        await resourceChecker.CheckAsync(targetResource, cts.Token);

        // Assert
        targetResource.LastUpdate.Should()
            .BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMinutes(1));
    }

    [Fact(DisplayName = $"{nameof(ResourceChecker)} should not update resource on unhealthy status")]
    [Trait("Category", "Unit")]
    public async Task ResourceCheckerShouldNotUpdateResourceOnUnhealthyStatus()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var targetUri = new Uri("http://example.com");
        var timeout = TimeSpan.FromSeconds(42);
        var rawChecker = new Mock<IRawResourceChecker>(MockBehavior.Strict);
        rawChecker.Setup(x => x.CheckAsync(
                timeout,
                targetUri,
                cts.Token))
            .ReturnsAsync(ResourceStatus.Unhealthy);
        var logger = new Mock<ILogger<ResourceChecker>>(MockBehavior.Strict).Object;
        var resourceChecker = new ResourceChecker(rawChecker.Object, logger);
        var targetResource = new ResourceHealthCheck(
            new ResourceName("test"),
            TimeSpan.FromMicroseconds(1),
            new ResourceRequestSettings(
                targetUri,
                TimeSpan.FromMicroseconds(1),
                timeout));
        // Act

        await resourceChecker.CheckAsync(targetResource, cts.Token);

        // Assert
        targetResource.LastUpdate.Should()
            .NotBeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMinutes(1));
    }
}