using Abstractions;
using Transport.Models;

using Newtonsoft.Json;

using System.Globalization;
using System.Text;

namespace Transport;

/// <summary>
/// Json serializer for <see cref="HealthCheckReport"/>.
/// </summary>
public sealed class HealthCheckReportSerializer :
    ISerializer<HealthCheckReport, string>
{
    ///<inheritdoc/>
    ///<exception cref="ArgumentNullException">
    ///Throws if <paramref name="source"/> is null.
    ///</exception>
    public string Serialize(HealthCheckReport source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var sb = new StringBuilder();
        var sw = new StringWriter(sb, CultureInfo.InvariantCulture);
        using (var jsonWriter = new JsonTextWriter(sw))
        {
            _serializer.Serialize(jsonWriter, source);
        }

        return sw.ToString();
    }

    private static readonly JsonSerializer _serializer = JsonSerializer.CreateDefault();
}
