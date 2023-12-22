namespace Models;

/// <summary>
/// Name for a resource.
/// </summary>
public sealed class ResourceName
{
    /// <summary>
    /// Name.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Create resource name.
    /// </summary>
    /// <param name="name">Name.</param>
    /// <exception cref="ArgumentNullException">Throws if name is null or empty</exception>
    public ResourceName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name), "Name cannot be null or empty.");
        }

        Value = name;
    }

    public override bool Equals(object? obj)
        => obj is ResourceName name &&
           Value == name.Value;

    public override int GetHashCode() => Value.GetHashCode();
}
