using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace Logic.Configuration;

/// <summary>
/// Validator for <see cref="ReportProcessorConfiguration"/>.
/// </summary>
public sealed class ReportProcessorConfigurationValidator : IValidateOptions<ReportProcessorConfiguration>
{
    /// <summary>
    /// Validates <see cref="ReportProcessorConfiguration"/>.
    /// </summary>
    public ValidateOptionsResult Validate(string? name, ReportProcessorConfiguration options)
    {
        Debug.Assert(options != null);

        if (options.SendInterval <= TimeSpan.Zero)
        {
            return ValidateOptionsResult.Fail("Send interval must be positive.");
        }

        return ValidateOptionsResult.Success;
    }
}
