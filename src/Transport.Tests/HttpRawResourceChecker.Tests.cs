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
        var exception = Record.Exception(() =>
        {
            _ = new HttpRawResourceChecker(clientProxy.Object, logger.Object);
        });

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"{nameof(HttpRawResourceChecker)} cant be created without logger")]
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

    [Fact(DisplayName = $"{nameof(HttpRawResourceChecker)} cant be created without proxy")]
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
}