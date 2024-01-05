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
}