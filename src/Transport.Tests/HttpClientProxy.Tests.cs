namespace Transport.Tests;

public class HttpClientProxyTests
{
    [Fact(DisplayName = $"{nameof(HttpClientProxy)} can be created")]
    [Trait("Category", "Unit")]
    public void HttpClientProxyCanBeCreated()
    {
        // Arrange
        using var responseHttpClient = new HttpClient();
        Func<TimeSpan, HttpClient> resourceClientFactory = timeout => new HttpClient();

        // Act
        var exception = Record.Exception(() => { _ = new HttpClientProxy(responseHttpClient, resourceClientFactory); });

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"{nameof(HttpClientProxy)} can't be created without Http client")]
    [Trait("Category", "Unit")]
    public void HttpClientProxyCannotBeCreatedWithoutHttpClient()
    {
        // Arrange
        Func<TimeSpan, HttpClient> resourceClientFactory = timeout => new HttpClient();
        // Act
        var exception = Record.Exception(() => { _ = new HttpClientProxy(null!, resourceClientFactory); });

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(HttpClientProxy)} can't be created without Http client factory")]
    [Trait("Category", "Unit")]
    public void HttpClientProxyCannotBeCreatedWithoutHttpClientFactory()
    {
        // Arrange
        using var responseHttpClient = new HttpClient();

        // Act
        var exception = Record.Exception(() => { _ = new HttpClientProxy(responseHttpClient, null!); });

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(HttpClientProxy)} returns wrapped http client")]
    [Trait("Category", "Unit")]
    public void HttpClientProxyReturnsWrappedHttpClient()
    {
        // Arrange
        using var responseHttpClient = new HttpClient();
        Func<TimeSpan, HttpClient> resourceClientFactory = timeout => new HttpClient();
        var httpClientProxy = new HttpClientProxy(responseHttpClient, resourceClientFactory);

        // Act
        var result = httpClientProxy.SenderClient;

        // Assert
        result.Should().Be(responseHttpClient);
    }

    [Fact(DisplayName = $"{nameof(HttpClientProxy)} creates httpClient by factory")]
    [Trait("Category", "Unit")]
    public void HttpClientProxyCreatesHttpClientByFactory()
    {
        // Arrange
        using var responseHttpClient = new HttpClient();
        var callCount = 0;
        Func<TimeSpan, HttpClient> resourceClientFactory = timeout =>
        {
            if (timeout == TimeSpan.FromSeconds((10)))
            {
                callCount++;
                return new HttpClient();
            }
            else
            {
                return null!;
            }
        };
        var httpClientProxy = new HttpClientProxy(responseHttpClient, resourceClientFactory);
        var timeout = TimeSpan.FromSeconds(10);

        // Act
        var result = httpClientProxy.ResourceClientFactory(timeout);

        // Assert
        result.Should().NotBeNull();
        callCount.Should().Be(1);
    }
}