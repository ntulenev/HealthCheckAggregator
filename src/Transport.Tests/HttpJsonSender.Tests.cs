using System.Net;

using Abstractions.Transport;

using Microsoft.Extensions.Logging;

using Moq.Protected;

namespace Transport.Tests;

public class HttpJsonSenderTests
{
    [Fact(DisplayName = $"{nameof(HttpJsonSender)} can be created")]
    [Trait("Category", "Unit")]
    public void HttpJsonSenderCanBeCreated()
    {
        // Arrange
        var clientProxy = new Mock<IHttpClientProxy>(MockBehavior.Strict);
        var logger = new Mock<ILogger<HttpJsonSender>>();

        // Act
        var exception = Record.Exception(() => { _ = new HttpJsonSender(clientProxy.Object, logger.Object); });

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"{nameof(HttpJsonSender)} can't be created without logger")]
    [Trait("Category", "Unit")]
    public void HttpJsonSenderCannotBeCreatedWithoutLogger()
    {
        // Arrange
        var clientProxy = new Mock<IHttpClientProxy>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => { _ = new HttpJsonSender(clientProxy.Object, null!); });

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(HttpJsonSender)} can't be created without client proxy")]
    [Trait("Category", "Unit")]
    public void HttpJsonSenderCannotBeCreatedWithoutClientProxy()
    {
        // Arrange
        var logger = new Mock<ILogger<HttpJsonSender>>();

        // Act
        var exception = Record.Exception(() => { _ = new HttpJsonSender(null!, logger.Object); });

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(HttpJsonSender)} can't send null data")]
    [Trait("Category", "Unit")]
    public async Task HttpJsonSenderCannotSendNullData()
    {
        // Arrange
        var clientProxy = new Mock<IHttpClientProxy>(MockBehavior.Strict);
        var logger = new Mock<ILogger<HttpJsonSender>>();
        var sender = new HttpJsonSender(clientProxy.Object, logger.Object);
        using var cts = new CancellationTokenSource();

        // Act
        var exception = await Record.ExceptionAsync(async ()
            => await sender.SendAsync(null!, new Uri("http://example.com"), cts.Token));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(HttpJsonSender)} can't send null uri")]
    [Trait("Category", "Unit")]
    public async Task HttpJsonSenderCannotSendNullUri()
    {
        // Arrange
        var clientProxy = new Mock<IHttpClientProxy>(MockBehavior.Strict);
        var logger = new Mock<ILogger<HttpJsonSender>>();
        var sender = new HttpJsonSender(clientProxy.Object, logger.Object);
        using var cts = new CancellationTokenSource();

        // Act
        var exception = await Record.ExceptionAsync(async () => await sender.SendAsync("data", null!, cts.Token));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(HttpJsonSender)} can't send with cancelled token")]
    [Trait("Category", "Unit")]
    public async Task HttpJsonSenderCannotSendWithCancelledToken()
    {
        // Arrange
        var clientProxy = new Mock<IHttpClientProxy>(MockBehavior.Strict);
        var logger = new Mock<ILogger<HttpJsonSender>>();
        var sender = new HttpJsonSender(clientProxy.Object, logger.Object);
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        // Act
        var exception = await Record.ExceptionAsync(async ()
            => await sender.SendAsync("data", new Uri("http://example.com"), cts.Token));

        // Assert
        exception.Should().BeOfType<OperationCanceledException>();
    }

    [Fact(DisplayName = $"{nameof(HttpJsonSender)} returns true on success sending")]
    [Trait("Category", "Unit")]
    public async Task HttpJsonSenderReturnsTrueOnSuccessSending()
    {
        // Arrange
        var clientProxy = new Mock<IHttpClientProxy>(MockBehavior.Strict);
        var logger = new Mock<ILogger<HttpJsonSender>>();
        var sender = new HttpJsonSender(clientProxy.Object, logger.Object);
        using var cts = new CancellationTokenSource();
        var url = new Uri("http://example.com");
        var data = "test";
        var handlerMock = new Mock<HttpMessageHandler>();
#pragma warning disable CA2000 // Dispose objects before losing scope
        handlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(request =>
                    request.Method == HttpMethod.Post &&
                    request.RequestUri!.Equals(url)),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("mocked API response")
                });
#pragma warning restore CA2000 // Dispose objects before losing scope
        using var httpClient = new HttpClient(handlerMock.Object);
        clientProxy.Setup(c =>
                c.SenderClient).Returns(() => httpClient);

        // Act
        var result = await sender.SendAsync(data, url, cts.Token);

        // Assert
        result.Should().BeTrue();
    }

    [Fact(DisplayName = $"{nameof(HttpJsonSender)} returns false on failed sending")]
    [Trait("Category", "Unit")]
    public async Task HttpJsonSenderReturnsFalseOnErrorSending()
    {
        // Arrange
        var clientProxy = new Mock<IHttpClientProxy>(MockBehavior.Strict);
        var logger = new Mock<ILogger<HttpJsonSender>>();
        var sender = new HttpJsonSender(clientProxy.Object, logger.Object);
        using var cts = new CancellationTokenSource();
        var url = new Uri("http://example.com");
        var data = "test";
        var handlerMock = new Mock<HttpMessageHandler>();
#pragma warning disable CA2000 // Dispose objects before losing scope
        handlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(request =>
                    request.Method == HttpMethod.Post &&
                    request.RequestUri!.Equals(url)),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent("mocked API response")
                });
#pragma warning restore CA2000 // Dispose objects before losing scope
        using var httpClient = new HttpClient(handlerMock.Object);
        clientProxy.Setup(c =>
            c.SenderClient).Returns(() => httpClient);

        // Act
        var result = await sender.SendAsync(data, url, cts.Token);

        // Assert
        result.Should().BeFalse();
    }
}