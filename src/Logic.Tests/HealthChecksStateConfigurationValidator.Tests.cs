using Microsoft.Extensions.Options;

using Logic.Configuration.Validation;
using Logic.Configuration;

namespace Logic.Tests;

public class HealthChecksStateConfigurationValidatorTests
{
    [Fact(DisplayName = $"Validator for {nameof(HealthChecksStateConfiguration)} " +
        $"should succeed for valid configuration")]
    [Trait("Category", "Unit")]
    public void ValidateSuccessForValidConfiguration()
    {
        // Arrange
        var options = new HealthChecksStateConfiguration
        {
            Resources = new List<ResourceConfiguration>
            {
                new() {
                    Name = "Resource1",
                    ExpirationPeriod = TimeSpan.FromMinutes(5),
                    Url = new Uri("http://example.com"),
                    CheckInterval = TimeSpan.FromSeconds(30),
                    Timeout = TimeSpan.FromSeconds(10)
                },
                new() {
                    Name = "Resource2",
                    ExpirationPeriod = TimeSpan.FromMinutes(5),
                    Url = new Uri("http://example.com"),
                    CheckInterval = TimeSpan.FromSeconds(30),
                    Timeout = TimeSpan.FromSeconds(10)
                }
            }
        };
        var validator = new HealthChecksStateConfigurationValidator();

        // Act
        var result = validator.Validate(null, options);

        // Assert
        result.Should().Be(ValidateOptionsResult.Success);
    }

    [Fact(DisplayName = $"Validator for {nameof(HealthChecksStateConfiguration)} " +
        $"should fail if no resources are provided")]
    public void ValidateFailureForNoResources()
    {
        // Arrange
        var options = new HealthChecksStateConfiguration
        {
            Resources = new List<ResourceConfiguration>()
        };
        var validator = new HealthChecksStateConfigurationValidator();

        // Act
        var result = validator.Validate(null!, options);

        // Assert
        result.Failed.Should().BeTrue();
    }


    [Fact(DisplayName = $"Validator for {nameof(HealthChecksStateConfiguration)} " +
        $"should fail if resources collection is null")]
    public void ValidateFailureForNullResources()
    {
        // Arrange
        var options = new HealthChecksStateConfiguration
        {
            Resources = null!
        };
        var validator = new HealthChecksStateConfigurationValidator();

        // Act
        var result = validator.Validate(null!, options);

        // Assert
        result.Failed.Should().BeTrue();
    }

    [Fact(DisplayName = $"Validator for {nameof(HealthChecksStateConfiguration)} " +
             $"should fail if resources are duplicated")]
    [Trait("Category", "Unit")]
    public void ValidateFailureForDuplicatedResources()
    {
        // Arrange
        var options = new HealthChecksStateConfiguration
        {
            Resources = new List<ResourceConfiguration>
            {
                new()
                {
                    Name = "Resource1",
                    ExpirationPeriod = TimeSpan.FromMinutes(5),
                    Url = new Uri("http://example.com"),
                    CheckInterval = TimeSpan.FromSeconds(30),
                    Timeout = TimeSpan.FromSeconds(10)
                },
                new()
                {
                    Name = "Resource1",
                    ExpirationPeriod = TimeSpan.FromMinutes(15),
                    Url = new Uri("http://example.com"),
                    CheckInterval = TimeSpan.FromSeconds(35),
                    Timeout = TimeSpan.FromSeconds(15)
                }
            }
        };
        var validator = new HealthChecksStateConfigurationValidator();

        // Act
        var result = validator.Validate(null, options);

        // Assert
        result.Failed.Should().BeTrue();
    }

    [Theory(DisplayName = $"Validator for {nameof(HealthChecksStateConfiguration)} " +
         $"should fail if resources name is null or empty")]
    [Trait("Category", "Unit")]
    [InlineData(null!)]
    [InlineData("")]
    public void ValidateFailureForNullOrEmptyName(string name)
    {
        // Arrange
        var options = new HealthChecksStateConfiguration
        {
            Resources = new List<ResourceConfiguration>
            {
                new()
                {
                    Name = name,
                    ExpirationPeriod = TimeSpan.FromMinutes(5),
                    Url = new Uri("http://example.com"),
                    CheckInterval = TimeSpan.FromSeconds(30),
                    Timeout = TimeSpan.FromSeconds(10)
                }
            }
        };
        var validator = new HealthChecksStateConfigurationValidator();

        // Act
        var result = validator.Validate(null, options);

        // Assert
        result.Failed.Should().BeTrue();
    }

    [Fact(DisplayName = $"Validator for {nameof(HealthChecksStateConfiguration)} " +
     $"should fail if resources name is whitespace")]
    [Trait("Category", "Unit")]
    public void ValidateFailureForWhitespaceName()
    {
        // Arrange
        var options = new HealthChecksStateConfiguration
        {
            Resources = new List<ResourceConfiguration>
            {
                new()
                {
                    Name = "      ",
                    ExpirationPeriod = TimeSpan.FromMinutes(5),
                    Url = new Uri("http://example.com"),
                    CheckInterval = TimeSpan.FromSeconds(30),
                    Timeout = TimeSpan.FromSeconds(10)
                }
            }
        };
        var validator = new HealthChecksStateConfigurationValidator();

        // Act
        var result = validator.Validate(null, options);

        // Assert
        result.Failed.Should().BeTrue();
    }

    [Fact(DisplayName = $"Validator for {nameof(HealthChecksStateConfiguration)} " +
     $"should fail if ExpirationPeriod if is not positive")]
    [Trait("Category", "Unit")]
    public void ValidateFailureForExpirationPeriodIsNotPositive()
    {
        // Arrange
        var options = new HealthChecksStateConfiguration
        {
            Resources = new List<ResourceConfiguration>
            {
                new()
                {
                    Name = "Resource1",
                    ExpirationPeriod = TimeSpan.Zero,
                    Url = new Uri("http://example.com"),
                    CheckInterval = TimeSpan.FromSeconds(30),
                    Timeout = TimeSpan.FromSeconds(10)
                }
            }
        };
        var validator = new HealthChecksStateConfigurationValidator();

        // Act
        var result = validator.Validate(null, options);

        // Assert
        result.Failed.Should().BeTrue();
    }

    [Fact(DisplayName = $"Validator for {nameof(HealthChecksStateConfiguration)} " +
     $"should fail if CheckInterval if is not positive")]
    [Trait("Category", "Unit")]
    public void ValidateFailureForCheckIntervalIsNotPositive()
    {
        // Arrange
        var options = new HealthChecksStateConfiguration
        {
            Resources = new List<ResourceConfiguration>
            {
                new()
                {
                    Name = "Resource1",
                    ExpirationPeriod = TimeSpan.FromSeconds(30),
                    Url = new Uri("http://example.com"),
                    CheckInterval = TimeSpan.Zero,
                    Timeout = TimeSpan.FromSeconds(10)
                }
            }
        };
        var validator = new HealthChecksStateConfigurationValidator();

        // Act
        var result = validator.Validate(null, options);

        // Assert
        result.Failed.Should().BeTrue();
    }

    [Fact(DisplayName = $"Validator for {nameof(HealthChecksStateConfiguration)} " +
     $"should fail if Timeout if is not positive")]
    [Trait("Category", "Unit")]
    public void ValidateFailureForTimeoutIsNotPositive()
    {
        // Arrange
        var options = new HealthChecksStateConfiguration
        {
            Resources = new List<ResourceConfiguration>
            {
                new()
                {
                    Name = "Resource1",
                    ExpirationPeriod = TimeSpan.FromSeconds(30),
                    Url = new Uri("http://example.com"),
                    CheckInterval = TimeSpan.FromSeconds(30),
                    Timeout = TimeSpan.Zero
                }
            }
        };
        var validator = new HealthChecksStateConfigurationValidator();

        // Act
        var result = validator.Validate(null, options);

        // Assert
        result.Failed.Should().BeTrue();
    }

    [Fact(DisplayName = $"Validator for {nameof(HealthChecksStateConfiguration)} " +
      $"should fail if Url is not set")]
    [Trait("Category", "Unit")]
    public void ValidateFailureForUrlIsNotSet()
    {
        // Arrange
        var options = new HealthChecksStateConfiguration
        {
            Resources = new List<ResourceConfiguration>
            {
                new()
                {
                    Name = "Resource1",
                    ExpirationPeriod = TimeSpan.FromSeconds(30),
                    Url = null!,
                    CheckInterval = TimeSpan.FromSeconds(30),
                    Timeout = TimeSpan.FromSeconds(30)
                }
            }
        };
        var validator = new HealthChecksStateConfigurationValidator();

        // Act
        var result = validator.Validate(null, options);

        // Assert
        result.Failed.Should().BeTrue();
    }
}