using Models;

namespace Abstractions;

public interface IRawResourceChecker
{
    public Task<ResourceStatus> CheckAsync(TimeSpan timeout, Uri uri, CancellationToken ct);
}
