using FluentAssertions;

namespace Models.Tests;

public class ResourceHealthCheckTests
{

    [Fact(DisplayName = $"{nameof(ResourceHealthCheck)} can be created with valid params")]
    [Trait("Category", "Unit")]
    public void CanBeCreatedWithValidParams()
    {
        // Arrange
        var resourceName = new ResourceName("TestResource");
        var expirationPeriod = TimeSpan.FromSeconds(30);
        var requestSettings = new ResourceRequestSettings(
            new Uri("https://example.com"),
            TimeSpan.FromSeconds(15),
            TimeSpan.FromSeconds(60));

        // Act
        var healthCheck = new ResourceHealthCheck(resourceName, expirationPeriod, requestSettings);

        // Assert
        healthCheck.ResourceName.Should().Be(resourceName);
        healthCheck.ExpirationPeriod.Should().Be(expirationPeriod);
        healthCheck.RequestSettings.Should().Be(requestSettings);
    }

    [Fact(DisplayName = $"{nameof(ResourceHealthCheck)} throws ArgumentNullException for null ResourceName")]
    [Trait("Category", "Unit")]
    public void ConstructorThrowsArgumentNullExceptionForNullResourceName()
    {
        // Arrange
        ResourceName resourceName = null!;
        var expirationPeriod = TimeSpan.FromSeconds(30);
        var requestSettings = new ResourceRequestSettings(
            new Uri("https://example.com"),
            TimeSpan.FromSeconds(15),
            TimeSpan.FromSeconds(60));

        // Act
        Action act = () => _ = new ResourceHealthCheck(resourceName, expirationPeriod, requestSettings);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(ResourceHealthCheck)} throws ArgumentNullException for null RequestSettings")]
    [Trait("Category", "Unit")]
    public void ConstructorThrowsArgumentNullExceptionForNullRequestSettings()
    {
        // Arrange
        var resourceName = new ResourceName("TestResource");
        var expirationPeriod = TimeSpan.FromSeconds(30);
        ResourceRequestSettings requestSettings = null!;

        // Act
        Action act = () => _ = new ResourceHealthCheck(resourceName, expirationPeriod, requestSettings);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(ResourceHealthCheck)} IsExpired returns UnHealthy for expired health check")]
    [Trait("Category", "Unit")]
    public void IsExpiredReturnsUnHealthyForExpiredHealthCheck()
    {
        // Arrange
        var resourceName = new ResourceName("TestResource");
        var expirationPeriod = TimeSpan.FromSeconds(30);
        var requestSettings = new ResourceRequestSettings(
            new Uri("https://example.com"), 
            TimeSpan.FromSeconds(15), 
            TimeSpan.FromSeconds(60));
        var healthCheck = new ResourceHealthCheck(
                resourceName, 
                expirationPeriod, 
                requestSettings);

        // Act
        var status = healthCheck.IsExpired();

        // Assert
        status.Should().Be(ResourceStatus.Unhealthy);
    }

    [Fact(DisplayName = $"{nameof(ResourceHealthCheck)} Update updates LastUpdate property")]
    [Trait("Category", "Unit")]
    public void UpdateUpdatesLastUpdateProperty()
    {
        // Arrange
        var resourceName = new ResourceName("TestResource");
        var expirationPeriod = TimeSpan.FromSeconds(30);
        var requestSettings = new ResourceRequestSettings(
            new Uri("https://example.com"), 
            TimeSpan.FromSeconds(15), 
            TimeSpan.FromSeconds(60));
        var healthCheck = new ResourceHealthCheck(
                resourceName, 
                expirationPeriod, 
                requestSettings);
        var initialLastUpdate = healthCheck.LastUpdage;

        // Act
        healthCheck.Update();
        var updatedLastUpdate = healthCheck.LastUpdage;

        // Assert
        updatedLastUpdate.Should().BeAfter(initialLastUpdate);
    }

    [Fact(DisplayName = $"{nameof(ResourceHealthCheck)} Update makes resource healthy")]
    [Trait("Category", "Unit")]
    public void UpdateMakesResourceHealthy()
    {
        // Arrange
        var resourceName = new ResourceName("TestResource");
        var expirationPeriod = TimeSpan.FromSeconds(300);
        var requestSettings = new ResourceRequestSettings(
            new Uri("https://example.com"), 
            TimeSpan.FromSeconds(15), 
            TimeSpan.FromSeconds(60));
        var healthCheck = new ResourceHealthCheck(
                resourceName, 
                expirationPeriod, 
                requestSettings);

        // Act
        healthCheck.Update();
        var status = healthCheck.IsExpired();

        // Assert
        status.Should().Be(ResourceStatus.Healthy);
    }

}
