namespace Transport.Tests;

public class HttpClientProxyTests
{
    [Fact(DisplayName = $"{nameof(HttpClientProxy)} can be created")]
    [Trait("Category", "Unit")]
    public void HttpClientProxyCanBeCreated()
    {
        // Arrange
        var responseHttpClient = new HttpClient();
        Func<TimeSpan, HttpClient> resourceClientFactory = timeout => new HttpClient();

        // Act
        var exception = Record.Exception(() =>
        {
            _ = new HttpClientProxy(responseHttpClient, resourceClientFactory);
        });

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
        var exception = Record.Exception(() =>
        {
            _ = new HttpClientProxy(null!, resourceClientFactory);
        });

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(HttpClientProxy)} can't be created without Http client factory")]
    [Trait("Category", "Unit")]
    public void HttpClientProxyCannotBeCreatedWithoutHttpClientFactory()
    {
        // Arrange
        var responseHttpClient = new HttpClient();

        // Act
        var exception = Record.Exception(() =>
        {
            _ = new HttpClientProxy(responseHttpClient, null!);
        });

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}