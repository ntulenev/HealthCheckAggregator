using System.Net;
    
using Abstractions.Transport;
using Models;

using Microsoft.Extensions.Logging;

using Moq.Protected;

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
        var exception = Record.Exception(() =>
        {
            _ = new HttpRawResourceChecker(clientProxy.Object, logger.Object);
        });

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
            await checker.CheckAsync(
                TimeSpan.FromSeconds(10), 
                new Uri("http://example.com"), 
                cts.Token);
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

    [Theory(DisplayName = $"{nameof(HttpRawResourceChecker)} can process all statuses")]
    [Trait("Category", "Unit")]
    [InlineData(ResourceStatus.Unhealthy)]
    //[InlineData(ResourceStatus.Healthy)]
    //TODO fix Issue with mocking handler
    public async Task HttpRawResourceCheckerCanProcessAllStatuses(ResourceStatus status)
    {
        // Arrange 
        using var cts = new CancellationTokenSource();
        var timeout = TimeSpan.FromSeconds(10);
        var uri = new Uri("http://example.com");
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                It.IsAny<CancellationToken>())
            .ReturnsAsync(
                new HttpResponseMessage
            {
                //Content = new StringContent("test"),
                StatusCode = status == ResourceStatus.Unhealthy
                    ? HttpStatusCode.InternalServerError
                    : HttpStatusCode.OK
            });

        /*
         *request =>
                    request.Method == HttpMethod.Get && request.RequestUri!.Equals(uri)
         *
         */
        using var httpClient = new HttpClient(handlerMock.Object)
        {
            Timeout = timeout
        };
        var clientProxy = new Mock<IHttpClientProxy>(MockBehavior.Strict);
        clientProxy.Setup(x => x.ResourceClientFactory(timeout)).Returns(httpClient);
        var logger = new Mock<ILogger<HttpRawResourceChecker>>();
        var checker = new HttpRawResourceChecker(clientProxy.Object, logger.Object);

        // Act
        var result = await checker.CheckAsync(
            timeout,
            uri,
            cts.Token);

        // Assert
        result.Should().Be(status);
    }
}