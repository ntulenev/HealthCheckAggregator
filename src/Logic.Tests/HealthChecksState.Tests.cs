using Logic.Configuration;
using Models;

using Microsoft.Extensions.Options;

namespace Logic.Tests;

public class HealthChecksStateTests
{
    [Fact(DisplayName = $"{nameof(HealthChecksState)} can be created with valid params")]
    [Trait("Category", "Unit")]
    public void CanBeCreatedWithValidParams()
    {
        // Arrange
        var options = new Mock<IOptions<HealthChecksStateConfiguration>>(MockBehavior.Strict);
        options.Setup(x => x.Value)
            .Returns(new HealthChecksStateConfiguration
            {
                Resources = new List<ResourceConfiguration>
                {
                    new()
                    {
                        Name = "Resource1",
                        ExpirationPeriod = TimeSpan.FromMinutes(5),
                        Url = new Uri("http://example.com"),
                        CheckInterval = TimeSpan.FromSeconds(10),
                        Timeout = TimeSpan.FromSeconds(5)
                    },
                    new()
                    {
                        Name = "Resource2",
                        ExpirationPeriod = TimeSpan.FromMinutes(10),
                        Url = new Uri("http://example2.com"),
                        CheckInterval = TimeSpan.FromSeconds(15),
                        Timeout = TimeSpan.FromSeconds(7)
                    }
                }
            });
        var resourceHealthCheckFactory = new Func<ResourceConfiguration, ResourceHealthCheck>(config =>
        {
            var requestSettings = new ResourceRequestSettings(
                config.Url,
                config.CheckInterval,
                config.Timeout);

            return new ResourceHealthCheck(
                new ResourceName(config.Name),
                config.ExpirationPeriod,
                requestSettings);
        });

        // Act
        var exception = Record.Exception(() =>
            _ = new HealthChecksState(options.Object, resourceHealthCheckFactory));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"{nameof(HealthChecksState)} cant be created without configuration options")]
    [Trait("Category", "Unit")]
    public void CantBeCreatedWithoutConfigurationOptions()
    {
        // Arrange
        var resourceHealthCheckFactory = new Func<ResourceConfiguration, ResourceHealthCheck>(config =>
        {
            var requestSettings = new ResourceRequestSettings(
                config.Url,
                config.CheckInterval,
                config.Timeout);
            return new ResourceHealthCheck(
                new ResourceName(config.Name),
                config.ExpirationPeriod,
                requestSettings);
        });
        // Act
        var exception = Record.Exception(() =>
            _ = new HealthChecksState(null!, resourceHealthCheckFactory));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(HealthChecksState)} cant be created without resource factory")]
    [Trait("Category", "Unit")]
    public void CantBeCreatedWithoutResourceFactory()
    {
        // Arrange
        var options = new Mock<IOptions<HealthChecksStateConfiguration>>(MockBehavior.Strict);
        options.Setup(x => x.Value)
            .Returns(new HealthChecksStateConfiguration
            {
                Resources = new List<ResourceConfiguration>
                {
                    new()
                    {
                        Name = "Resource1",
                        ExpirationPeriod = TimeSpan.FromMinutes(5),
                        Url = new Uri("http://example.com"),
                        CheckInterval = TimeSpan.FromSeconds(10),
                        Timeout = TimeSpan.FromSeconds(5)
                    }
                }
            });
        // Act
        var exception = Record.Exception(() =>
            _ = new HealthChecksState(options.Object, null!));
        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"{nameof(HealthChecksState)} cant be created with empty resources collection in config")]
    [Trait("Category", "Unit")]
    public void CantBeCreatedWithEmptyResourcesCollectionInConfig()
    {
        // Arrange
        var options = new Mock<IOptions<HealthChecksStateConfiguration>>(MockBehavior.Strict);
        options.Setup(x => x.Value)
            .Returns(new HealthChecksStateConfiguration
            {
                Resources = new List<ResourceConfiguration>()
            });
        var resourceHealthCheckFactory = new Func<ResourceConfiguration, ResourceHealthCheck>(config =>
        {
            var requestSettings = new ResourceRequestSettings(
                config.Url,
                config.CheckInterval,
                config.Timeout);
            return new ResourceHealthCheck(
                new ResourceName(config.Name),
                config.ExpirationPeriod,
                requestSettings);
        });

        // Act
        var exception = Record.Exception(() =>
            _ = new HealthChecksState(options.Object, resourceHealthCheckFactory));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = $"{nameof(HealthChecksState)} can return health checks")]
    [Trait("Category", "Unit")]
    public void CanReturnHealthChecks()
    {
        // Arrange
        var hcConfig1 = new ResourceConfiguration()
        {
            Name = "Resource1",
            ExpirationPeriod = TimeSpan.FromMinutes(5),
            Url = new Uri("http://example.com"),
            CheckInterval = TimeSpan.FromSeconds(10),
            Timeout = TimeSpan.FromSeconds(5)
        };
        var hcConfig2 = new ResourceConfiguration()
        {
            Name = "Resource2",
            ExpirationPeriod = TimeSpan.FromMinutes(10),
            Url = new Uri("http://example2.com"),
            CheckInterval = TimeSpan.FromSeconds(15),
            Timeout = TimeSpan.FromSeconds(7)
        };

        var options = new Mock<IOptions<HealthChecksStateConfiguration>>(MockBehavior.Strict);
        options.Setup(x => x.Value)
            .Returns(new HealthChecksStateConfiguration
            {
                Resources = new List<ResourceConfiguration>
                {
                    hcConfig1,
                    hcConfig2
                }
            });
        var resourceHealthCheckFactory = new Func<ResourceConfiguration, ResourceHealthCheck>(config =>
        {
            var requestSettings = new ResourceRequestSettings(
                config.Url,
                config.CheckInterval,
                config.Timeout);
            return new ResourceHealthCheck(
                new ResourceName(config.Name),
                config.ExpirationPeriod,
                requestSettings);
        });
        var healthChecksState = new HealthChecksState(options.Object, resourceHealthCheckFactory);

        // Act
        var healthChecks = healthChecksState.HealthChecks;

        // Assert
        healthChecks.Should().NotBeNull();
        healthChecks.Should().HaveCount(2);
        var hc1 = healthChecks.First();
        hc1.ResourceName.Value.Should().Be(hcConfig1.Name);
        hc1.ExpirationPeriod.Should().Be(hcConfig1.ExpirationPeriod);
        hc1.RequestSettings.Url.Should().Be(hcConfig1.Url);
        hc1.RequestSettings.CheckInterval.Should().Be(hcConfig1.CheckInterval);
        hc1.RequestSettings.Timeout.Should().Be(hcConfig1.Timeout);
        var hc2 = healthChecks.Last();
        hc2.ResourceName.Value.Should().Be(hcConfig2.Name);
        hc2.ExpirationPeriod.Should().Be(hcConfig2.ExpirationPeriod);
        hc2.RequestSettings.Url.Should().Be(hcConfig2.Url);
        hc2.RequestSettings.CheckInterval.Should().Be(hcConfig2.CheckInterval);
        hc2.RequestSettings.Timeout.Should().Be(hcConfig2.Timeout);
    }

    [Fact(DisplayName = $"{nameof(HealthChecksState)} can build report")]
    [Trait("Category", "Unit")]
    public void CanBuildReport()
    {
        // Arrange
        var hcConfig1 = new ResourceConfiguration()
        {
            Name = "Resource1",
            ExpirationPeriod = TimeSpan.FromMinutes(5),
            Url = new Uri("http://example.com"),
            CheckInterval = TimeSpan.FromSeconds(10),
            Timeout = TimeSpan.FromSeconds(5)
        };
        var hcConfig2 = new ResourceConfiguration()
        {
            Name = "Resource2",
            ExpirationPeriod = TimeSpan.FromMinutes(10),
            Url = new Uri("http://example2.com"),
            CheckInterval = TimeSpan.FromSeconds(15),
            Timeout = TimeSpan.FromSeconds(7)
        };

        var options = new Mock<IOptions<HealthChecksStateConfiguration>>(MockBehavior.Strict);
        options.Setup(x => x.Value)
            .Returns(new HealthChecksStateConfiguration
            {
                Resources = new List<ResourceConfiguration>
                {
                    hcConfig1,
                    hcConfig2
                }
            });
        var resourceHealthCheckFactory = new Func<ResourceConfiguration, ResourceHealthCheck>(config =>
        {
            var requestSettings = new ResourceRequestSettings(
                config.Url,
                config.CheckInterval,
                config.Timeout);
            return new ResourceHealthCheck(
                new ResourceName(config.Name),
                config.ExpirationPeriod,
                requestSettings);
        });
        var healthChecksState = new HealthChecksState(options.Object, resourceHealthCheckFactory);

        // Act
        var report = healthChecksState.BuildReport();

        // Assert
        report.Should().NotBeNull();
        report.ReportItems.Should().HaveCount(2);
        var hc1 = report.ReportItems.First();
        hc1.ResourceName.Value.Should().Be(hcConfig1.Name);
        var hc2 = report.ReportItems.Last();
        hc2.ResourceName.Value.Should().Be(hcConfig2.Name);
    }
}