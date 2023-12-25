using Microsoft.Extensions.Options;

namespace Logic.Configuration.Validation;

/// <summary>
/// Validator for <see cref="HealthChecksStateConfiguration"/>.
/// </summary>
public class HealthChecksStateConfigurationValidator : IValidateOptions<HealthChecksStateConfiguration>
{
    /// <summary>
    /// Validates the <see cref="HealthChecksStateConfiguration"/> options.
    /// </summary>
    /// <param name="name">The name of the options.</param>
    /// <param name="options">The <see cref="HealthChecksStateConfiguration"/> to validate.</param>
    /// <returns>A <see cref="ValidateOptionsResult"/> indicating success or failure.</returns>
    public ValidateOptionsResult Validate(string? name, HealthChecksStateConfiguration options)
    {
        if (options.Resources == null || !options.Resources.Any())
        {
            return ValidateOptionsResult.Fail("At least one resource configuration must be provided.");
        }

        var resourceNames = new HashSet<string>();

        foreach (var resource in options.Resources)
        {
            // Validate individual resource configuration
            var resourceValidationResult = ValidateResourceConfiguration(resource);
            if (!resourceValidationResult.Succeeded)
            {
                return resourceValidationResult;
            }

            // Validate the uniqueness of resource names
            if (!resourceNames.Add(resource.Name))
            {
                return ValidateOptionsResult.Fail($"Resource name '{resource.Name}' is not unique.");
            }
        }

        return ValidateOptionsResult.Success;
    }

    private static ValidateOptionsResult ValidateResourceConfiguration(
        ResourceConfiguration resource)
    {

        if (string.IsNullOrEmpty(resource.Name))
        {
            return ValidateOptionsResult.Fail($"Resource name '{resource.Name}' is not set");
        }

        if (string.IsNullOrWhiteSpace(resource.Name))
        {
            return ValidateOptionsResult.Fail($"Resource name '{resource.Name}' is not valid");
        }

        if (resource.ExpirationPeriod <= TimeSpan.Zero)
        {
            return ValidateOptionsResult.Fail($"Expiration period for resource '{resource.Name}' must be positive.");
        }

        if (resource.Uri == null)
        {
            return ValidateOptionsResult.Fail($"URI for resource '{resource.Name}' is not set");
        }

        if (resource.CheckInterval <= TimeSpan.Zero)
        {
            return ValidateOptionsResult.Fail($"Check interval for resource '{resource.Name}' must be positive.");
        }

        if (resource.Timeout <= TimeSpan.Zero)
        {
            return ValidateOptionsResult.Fail($"Timeout for resource '{resource.Name}' must be positive.");
        }

        return ValidateOptionsResult.Success;
    }
}