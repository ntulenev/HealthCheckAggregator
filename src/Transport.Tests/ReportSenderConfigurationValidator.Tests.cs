using Microsoft.Extensions.Options;

using Transport.Configuration;
using Transport.Configuration.Validation;

namespace Transport.Tests;

public class ReportSenderConfigurationValidatorTests
{
    [Fact(DisplayName = $"{nameof(ReportSenderConfigurationValidator)} could be created with valid params")]
    [Trait("Category", "Unit")]
    public void ReportSenderConfigurationValidatorCanBeCreatedWithValidParams()
    {
        // Arrange
        var options = new ReportSenderConfiguration
        {
            Url = new Uri("https://example.com"),
            Timeout = TimeSpan.FromSeconds(10)
        };
        var validator = new ReportSenderConfigurationValidator();
        // Act
        
        var result = validator.Validate(null!, options);
        
        // Assert
        result.Should().Be(ValidateOptionsResult.Success);
    }
}