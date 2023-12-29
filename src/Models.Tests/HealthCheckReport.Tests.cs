using FluentAssertions;

namespace Models.Tests;

[Trait("Category", "Unit")]
public class HealthCheckReportTests
{
    [Fact(DisplayName = $"{nameof(HealthCheckReport)} can be created with valid params")]
    [Trait("Category", "Unit")]
    public void CanBeCreatedWithValidParams()
    {
        // Arrange
        var url = new Uri("https://example.com");
        var checkInterval = TimeSpan.FromSeconds(20);
        var timeout = TimeSpan.FromSeconds(60);
        var res1 = new ResourceName("Resource1");
        var res2 = new ResourceName("Resource2");
        var settings = new ResourceRequestSettings(url, checkInterval, timeout);

        var resourceHealthChecks = new List<ResourceHealthCheck>
        {
            new(res1, TimeSpan.FromSeconds(30),settings),
            new(res2, TimeSpan.FromSeconds(45),settings)
        };

        // Act
        var report = new HealthCheckReport(resourceHealthChecks);

        // Assert
        report.ReportItems.Should().HaveCount(2);
    }

    [Fact(DisplayName = $"{nameof(HealthCheckReport)} throws ArgumentNullException for null resourceHealthChecks")]
    [Trait("Category", "Unit")]
    public void ConstructorThrowsArgumentNullExceptionForNullResourceHealthChecks()
    {
        // Arrange
        IEnumerable<ResourceHealthCheck> resourceHealthChecks = null!;

        // Act
        Action act = () => _ = new HealthCheckReport(resourceHealthChecks);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(HealthCheckReport)} throws ArgumentException for empty resourceHealthChecks")]
    [Trait("Category", "Unit")]
    public void ConstructorThrowsArgumentExceptionForEmptyResourceHealthChecks()
    {
        // Arrange
        var resourceHealthChecks = Enumerable.Empty<ResourceHealthCheck>();

        // Act
        Action act = () => _ = new HealthCheckReport(resourceHealthChecks);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact(DisplayName = $"{nameof(HealthCheckReport)} IsUnhealthy returns true for all unhealthy resources")]
    [Trait("Category", "Unit")]
    public void IsUnhealthyReturnsFalseForAllHealthyResources()
    {
        // Arrange
        var url = new Uri("https://example.com");
        var checkInterval = TimeSpan.FromSeconds(20);
        var timeout = TimeSpan.FromSeconds(60);
        var settings = new ResourceRequestSettings(url, checkInterval, timeout);

        var resourceHealthChecks = new List<ResourceHealthCheck>
        {
            new(new ResourceName("Resource1"), TimeSpan.FromSeconds(1),settings),
            new(new ResourceName("Resource2"), TimeSpan.FromSeconds(1),settings)
        };

        // Act
        var report = new HealthCheckReport(resourceHealthChecks);

        // Assert
        report.IsUnhealthy.Should().BeTrue();
    }

    [Fact(DisplayName = $"{nameof(HealthCheckReport)} IsUnhealthy returns false when some resources is healthy")]
    [Trait("Category", "Unit")]
    public void IsUnhealthyReturnsTrueWhenAnyResourceIsUnhealthy()
    {
        // Arrange
        var url = new Uri("https://example.com");
        var checkInterval = TimeSpan.FromSeconds(20);
        var timeout = TimeSpan.FromSeconds(60);
        var settings = new ResourceRequestSettings(url, checkInterval, timeout);
        var res = new ResourceHealthCheck(new ResourceName("Resource1"), TimeSpan.FromDays(1), settings);
        res.Update();
        var resourceHealthChecks = new List<ResourceHealthCheck>
        {
            res,
            new(new ResourceName("Resource2"), TimeSpan.FromSeconds(30),settings),
            new(new ResourceName("Resource3"), TimeSpan.FromSeconds(30),settings)
        };

        // Act
        var report = new HealthCheckReport(resourceHealthChecks);

        // Assert
        report.IsUnhealthy.Should().BeFalse();
    }
}
