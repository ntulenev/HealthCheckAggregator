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

        if (string.IsNullOrEmpty(options.Url))
        {
            return ValidateOptionsResult.Fail("Url cannot be null or empty.");
        }

        if (!Uri.TryCreate(options.Url, UriKind.Absolute, out var uriResult) ||
                    uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps)
        {
            return ValidateOptionsResult.Fail($"Url '{options.Url}' is not a valid HTTP/HTTPS URL.");
        }

        return ValidateOptionsResult.Success;
    }
}
