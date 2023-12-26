using Microsoft.Extensions.Options;

using System.Diagnostics;

namespace Transport.Configuration.Validation;

/// <summary>
/// Validates <see cref="ReportSenderConfiguration"/>.
/// </summary>
public sealed class ReportSenderConfigurationValidator : IValidateOptions<ReportSenderConfiguration>
{
    /// <summary>
    /// Validates <see cref="ReportSenderConfiguration"/>.
    /// </summary>
    public ValidateOptionsResult Validate(string? name, ReportSenderConfiguration options)
    {
        Debug.Assert(options != null);

        if (options.Url is null)
        {
            return ValidateOptionsResult.Fail("Url cannot be null.");
        }

        if (options.Timeout <= TimeSpan.Zero)
        {
            return ValidateOptionsResult.Fail("Timeout must be positive.");
        }

        return ValidateOptionsResult.Success;
    }
}
