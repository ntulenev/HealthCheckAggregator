using Abstractions.Transport;

using Microsoft.Extensions.Logging;

namespace Transport.Tests;

public class HttpRawResourceCheckerTests
{
    [Fact(DisplayName = $"{nameof(HttpRawResourceChecker)} can be created")]
    [Trait("Category", "Unit")]
    public void HttpRawResourceCheckerCanBeCreated()
    {
        // Arrange
        var clientProxy = new Mock<IHttpClientProxy>(MockBehavior.Strict);
        var logger = new Mock<ILogger<HttpRawResourceChecker>>();

        // Act
        var exception = Record.Exception(() => { _ = new HttpRawResourceChecker(clientProxy.Object, logger.Object); });

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"{nameof(HttpRawResourceChecker)} can't be created without logger")]
    [Trait("Category", "Unit")]
    public void HttpRawResourceCheckerCannotBeCreatedWithoutLogger()
    {
        // Arrange
        var clientProxy = new Mock<IHttpClientProxy>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() =>
        {
            _ = new HttpRawResourceChecker(clientProxy.Object, null!);
        });

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(HttpRawResourceChecker)} can't be created without proxy")]
    [Trait("Category", "Unit")]
    public void HttpRawResourceCheckerCannotBeCreatedWithoutProxy()
    {
        // Arrange
        var logger = new Mock<ILogger<HttpRawResourceChecker>>();

        // Act
        var exception = Record.Exception(() =>
        {
            _ = new HttpRawResourceChecker(null!, logger.Object);
        });

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(HttpRawResourceChecker)} can't check status with null url")]
    [Trait("Category", "Unit")]
    public async Task HttpRawResourceCheckerCannotCheckStatusWithNullUrl()
    {
        // Arrange
        var clientProxy = new Mock<IHttpClientProxy>(MockBehavior.Strict);
        var logger = new Mock<ILogger<HttpRawResourceChecker>>();
        var checker = new HttpRawResourceChecker(clientProxy.Object, logger.Object);
        using var cts = new CancellationTokenSource();
        
        // Act
        var exception = await Record.ExceptionAsync(async () =>
        {
            await checker.CheckAsync(TimeSpan.FromSeconds(10), null!, cts.Token);
        });

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(HttpRawResourceChecker)} can't check status with cancelled token")]
    [Trait("Category", "Unit")]
    public async Task HttpRawResourceCheckerCannotCheckStatusWithCancelledToken()
    {
        // Arrange
        var clientProxy = new Mock<IHttpClientProxy>(MockBehavior.Strict);
        var logger = new Mock<ILogger<HttpRawResourceChecker>>();
        var checker = new HttpRawResourceChecker(clientProxy.Object, logger.Object);
        using var cts = new CancellationTokenSource();
        cts.Cancel(); 

        // Act
        var exception = await Record.ExceptionAsync(async () =>
        {
            await checker.CheckAsync(TimeSpan.FromSeconds(10), new Uri("http://example.com"), cts.Token);
        });

        // Assert
        exception.Should().BeOfType<OperationCanceledException>();
    }

    [Theory(DisplayName = $"{nameof(HttpRawResourceChecker)} can't check with zero or negative timespan")]
    [Trait("Category", "Unit")]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task HttpRawResourceCheckerCannotCheckWithZeroOrNegativeTimeout(int timeoutSeconds)
    {
        // Arrange
        var clientProxy = new Mock<IHttpClientProxy>(MockBehavior.Strict);
        var logger = new Mock<ILogger<HttpRawResourceChecker>>();
        var checker = new HttpRawResourceChecker(clientProxy.Object, logger.Object);
        using var cts = new CancellationTokenSource();
        
        // Act
        var exception = await Record.ExceptionAsync(async () =>
        {
            await checker.CheckAsync(
                TimeSpan.FromSeconds(timeoutSeconds),
                new Uri("http://example.com"),
                cts.Token);
        });
        
        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }
}