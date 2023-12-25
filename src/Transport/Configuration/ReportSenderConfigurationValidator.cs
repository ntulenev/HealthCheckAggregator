using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace Transport.Configuration;

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

        return ValidateOptionsResult.Success;
    }
}
