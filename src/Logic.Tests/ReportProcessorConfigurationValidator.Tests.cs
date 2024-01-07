using Logic.Configuration;
using Logic.Configuration.Validation;

using Microsoft.Extensions.Options;

namespace Logic.Tests;

public class ReportProcessorConfigurationValidatorTests
{
    [Fact(DisplayName = $"Validator for {nameof(ReportProcessorConfigurationValidator)} " +
                        $"should succeed for valid configuration")]
    [Trait("Category", "Unit")]
    public void ValidatorShouldSucceedForValidConfiguration()
    {
        // Arrange
        var validator = new ReportProcessorConfigurationValidator();
        var options = new ReportProcessorConfiguration
        {
            SendInterval = TimeSpan.FromMinutes(5)
        };

        // Act
        var result = validator.Validate(null, options);

        // Assert
        result.Should().Be(ValidateOptionsResult.Success);
    }

    [Fact(DisplayName = $"Validator for {nameof(ReportProcessorConfigurationValidator)} " +
                        $"should fail for invalid configuration")]
    [Trait("Category", "Unit")]
    public void ValidatorShouldFailForInvalidConfiguration()
    {
        // Arrange
        var validator = new ReportProcessorConfigurationValidator();
        var options = new ReportProcessorConfiguration
        {
            SendInterval = TimeSpan.Zero
        };

        // Act
        var result = validator.Validate(null, options);

        // Assert
        result.Failed.Should().BeTrue();
    }
}