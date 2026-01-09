using AutoMapper;

using Microsoft.Extensions.Logging.Abstractions;

using Transport.Mapping;

namespace Transport.Tests;

public class HealthCheckMappingProfileTests
{
    [Fact(DisplayName = $"{nameof(HealthCheckMappingProfile)} is valid")]
    [Trait("Category", "Unit")]
    public void AutoMapperConfigurationIsValid()
    {
        // Arrange
        using var loggerFactory = new NullLoggerFactory();
        var configuration = new MapperConfiguration(
            cfg => cfg.AddProfile<HealthCheckMappingProfile>(), loggerFactory);

        // Act & Assert
        configuration.AssertConfigurationIsValid();
    }
}