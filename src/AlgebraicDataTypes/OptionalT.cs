using System;

namespace AlgebraicDataTypes
{
  /// <summary>
  /// Represent Maybe monad which can contains some value or none.
  /// </summary>
  /// <typeparam name="T">Type of value that optional can contains.</typeparam>
  public readonly struct Optional<T> : IEquatable<Optional<T>>, IEquatable<T>
  {
    /// <summary>
    /// Container for the value.
    /// </summary>
    private readonly T _value;

    private Optional(T value)
    {
      _value = value;
      HasValue = true;
    }

    /// <summary>
    /// Indicates whether tbe current <see cref="Optional{T}"/> object has a value of its underlying type.
    /// </summary>
    public bool HasValue { get; }

    /// <summary>
    /// Get value contains in this Optional object or get default value
    /// </summary>
    /// <param name="default">Default value of <see cref="T"/> type.</param>
    /// <returns></returns>
    public T GetValueOrDefault(T @default)
    {
      return HasValue ? _value : @default;
    }

    public static implicit operator Optional<T>(T value)
    {
      if (value == null)
      {
        return new Optional<T>();
      }

      return new Optional<T>(value);
    }

    public static implicit operator Optional<T>(ProtoNone _)
    {
      return new Optional<T>();
    }

    /// <summary>
    /// Determines whether an element contains in optional.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <returns>True if contains, False if not.</returns>
    public bool Contains(T value)
    {
      if (HasValue)
      {
        return value.Equals(_value);
      }

      return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="some"></param>
    /// <param name="none"></param>
    public void Case(Action<T> some, Action none)
    {
      if (HasValue)
        some(_value);
      else
        none();
    }

    public U Case<U>(Func<T, U> some, U none)
    {
      return HasValue ? some(_value) : none;
    }

    public U Case<U>(Func<T, U> some, Func<U> none)
    {
      return HasValue ? some(_value) : none();
    }

    /// <summary>
    /// Execute func if Optional has value. (Type of returned value set implicitly)
    /// </summary>
    /// <typeparam name="U">Type of value returned from func if Optional has a value.</typeparam>
    /// <param name="map">Function thar execute if Optional has value.</param>
    /// <returns>Optional with value or None.</returns>
    public Optional<U> Select<U>(Func<T, U> map)
    {
      return HasValue ? Optional.CreateOptionalWithValue(map(_value)) : Optional.CreateNone;
    }

    /// <summary>
    /// Execute func if Optional has value. (Type of returned value should be set explicitly)
    /// </summary>
    /// <typeparam name="U">Type of value returned from func if Optional has a value.</typeparam>
    /// <param name="bind">Function thar execute if Optional has value.</param>
    /// <returns></returns>
    public Optional<U> Bind<U>(Func<T, Optional<U>> bind)
    {
      return HasValue ? bind(_value) : Optional.CreateNone;
    }

    /// <summary>
    /// Execute func if None or return value of Optional.
    /// </summary>
    /// <param name="none">Func for execution if Optional has no value.</param>
    /// <returns>Execute func if None or return value of Optional.</returns>
    public T Or(Func<T> none)
    {
      return HasValue ? _value : none();
    }

    /// <summary>
    /// Execute func returned Optional if None or return this Optional.
    /// </summary>
    /// <param name="none"></param>
    /// <returns></returns>
    public Optional<T> Or(Func<Optional<T>> none)
    {
      return HasValue ? this : none();
    }

    /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
    public bool Equals(Optional<T> other)
    {
      if (other.HasValue && Contains(other._value))
      {
        return true;
      }

      return !HasValue && !other.HasValue;
    }

    /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
    public bool Equals(T other)
    {
      return Contains(other);
    }

    public override bool Equals(object obj)
    {
      switch (obj)
      {
        case Optional<T> other when Equals(other):
          return true;
        case T specificValue:
          return Contains(specificValue);
        default:
          return false;
      }
    }

    public override int GetHashCode()
    {
      return HasValue ? _value.GetHashCode() : 0;
    }

    public override string ToString()
    {
      return !HasValue ? "None" : "Some " + _value.ToString();
    }

    public static bool operator ==(Optional<T> x, Optional<T> y)
    {
      return x.Equals(y);
    }

    public static bool operator !=(Optional<T> x, Optional<T> y)
    {
      return !x.Equals(y);
    }
  }
}
