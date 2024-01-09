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
    
    [Fact(DisplayName = $"{nameof(ReportSenderConfigurationValidator)} fails when URL is null")]
    [Trait("Category", "Unit")]
    public void ReportSenderConfigurationValidatorFailsWhenUrlIsNull()
    {
        // Arrange
        var options = new ReportSenderConfiguration
        {
            Url = null!,
            Timeout = TimeSpan.FromSeconds(10)
        };
        var validator = new ReportSenderConfigurationValidator();

        // Act
        var result = validator.Validate(null!, options);

        // Assert
        result.Should().NotBe(ValidateOptionsResult.Success);
    }

    [Fact(DisplayName = $"{nameof(ReportSenderConfigurationValidator)} fails when Timeout is zero")]
    [Trait("Category", "Unit")]
    public void ReportSenderConfigurationValidatorFailsWhenTimeoutIsZero()
    {
        // Arrange
        var options = new ReportSenderConfiguration
        {
            Url = new Uri("https://example.com"),
            Timeout = TimeSpan.Zero
        };
        var validator = new ReportSenderConfigurationValidator();

        // Act
        var result = validator.Validate(null!, options);

        // Assert
        result.Should().NotBe(ValidateOptionsResult.Success);
    }
    
}