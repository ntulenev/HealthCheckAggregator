namespace Abstractions.Transport;

/// <summary>
/// Provides contract of serialization from <typeparamref name="TSource"/> to <typeparamref name="TTarget"/>.
/// </summary>
/// <typeparam name="TSource">Type that should be serialized</typeparam>
/// <typeparam name="TTarget">Target type of the serialization.</typeparam>
public interface ISerializer<TSource, TTarget>
{
    /// <summary>
    /// Serialize from <typeparamref name="TSource"/> to <typeparamref name="TTarget"/>
    /// </summary>
    /// <param name="source">Object that should be serialized.</param>
    /// <returns>Serialized result.</returns>
    TTarget Serialize(TSource source);
}