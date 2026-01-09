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
    /// <exception cref="ArgumentNullException">Throws if name is null or empty.</exception>
    /// <exception cref="ArgumentException">Throws if name is whitespace.</exception>
    public ResourceName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name), "Name cannot be null or empty.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be whitespace.", nameof(name));
        }

        Value = name;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current ResourceName instance.
    /// </summary>
    /// <param name="obj">The object to compare with the current ResourceName instance.</param>
    /// <returns>true if the specified object is a ResourceName and its value is equal to the value of the current instance;
    /// otherwise, false.</returns>
    public override bool Equals(object? obj)
        => obj is ResourceName name &&
           Value == name.Value;

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <remarks>Use this method when inserting instances of this type into hash-based collections such as
    /// <see cref="Dictionary{TKey, TValue}"/> or <see
    /// cref="HashSet{T}"/>. The hash code is based on the value of the underlying
    /// <c>Value</c> property.</remarks>
    /// <returns>A 32-bit signed integer hash code for the current object.</returns>
#pragma warning disable CA1307 // Specify StringComparison for clarity
    public override int GetHashCode() => Value.GetHashCode();
#pragma warning restore CA1307 // Specify StringComparison for clarity

    /// <summary>
    /// Returns the string representation of the current object.
    /// </summary>
    /// <returns>A string that represents the value of the current object.</returns>
    public override string ToString() => Value;
}
