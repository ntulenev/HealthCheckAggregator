using AutoMapper;

using Transport.Mapping;

namespace Transport.Tests;

public class HealthCheckMappingProfileTests
{
    [Fact(DisplayName = $"{nameof(HealthCheckMappingProfile)} is valid")]
    [Trait("Category", "Unit")]
    public void AutoMapperConfigurationIsValid()
    {
        // Arrange
        var configuration = new MapperConfiguration(
            cfg => { cfg.AddProfile<HealthCheckMappingProfile>(); });

        // Act & Assert
        configuration.AssertConfigurationIsValid();
    }
}