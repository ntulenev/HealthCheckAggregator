using FluentAssertions;

namespace Models.Tests;

public class ResourceRequestSettingsTests
{
    [Fact(DisplayName = $"{nameof(ResourceRequestSettings)} can be created with valid params")]
    [Trait("Category", "Unit")]
    public void CanBeCreatedWithValidParams()
    {
        // Arrange
        var url = new Uri("https://example.com");
        var checkInterval = TimeSpan.FromSeconds(30);
        var timeout = TimeSpan.FromSeconds(60);

        // Act
        var settings = new ResourceRequestSettings(url, checkInterval, timeout);

        // Assert
        settings.Url.Should().Be(url);
        settings.CheckInterval.Should().Be(checkInterval);
        settings.Timeout.Should().Be(timeout);
    }

    [Fact(DisplayName = $"{nameof(ResourceRequestSettings)} throws ArgumentNullException for null URL")]
    [Trait("Category", "Unit")]
    public void CantBeCreatedWithoutUrl()
    {
        // Arrange
        Uri url = null!;
        var checkInterval = TimeSpan.FromSeconds(30);
        var timeout = TimeSpan.FromSeconds(60);

        // Act and Assert
        Action act = () => _ = new ResourceRequestSettings(url, checkInterval, timeout);
        act.Should().Throw<ArgumentNullException>();
    }

    [Theory(DisplayName = $"{nameof(ResourceRequestSettings)} throws ArgumentOutOfRangeException for invalid check interval")]
    [Trait("Category", "Unit")]
    [InlineData(0)]
    [InlineData(-1)]
    public void CantBeCreatedWithBadInterval(int seconds)
    {
        // Arrange
        var url = new Uri("https://example.com");
        var checkInterval = TimeSpan.FromSeconds(seconds);
        var timeout = TimeSpan.FromSeconds(60);

        // Act and Assert
        Action act = () => _ = new ResourceRequestSettings(url, checkInterval, timeout);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory(DisplayName = $"{nameof(ResourceRequestSettings)} throws ArgumentOutOfRangeException for invalid timeout")]
    [Trait("Category", "Unit")]
    [InlineData(0)]
    [InlineData(-1)]
    public void CantBeCreatedWithBadTimeout(int seconds)
    {
        // Arrange
        var url = new Uri("https://example.com");
        var checkInterval = TimeSpan.FromSeconds(30);
        var timeout = TimeSpan.FromSeconds(seconds);

        // Act and Assert
        Action act = () => _ = new ResourceRequestSettings(url, checkInterval, timeout);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}