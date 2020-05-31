using System;

namespace AlgebraicDataTypes
{
  //
  public readonly struct Optional<T> : IEquatable<Optional<T>>, IEquatable<T>
  {
    private readonly T _value;

    public Optional(T value)
    {
      _value = value;
      HasValue = true;
    }

    public bool HasValue { get; }

    public T GetValueOrDefault(T @default)
    {
      return HasValue ? _value : @default;
    }

    /// <summary>
    /// Determines whether an element is in <see cref="Optional{T}"/>>.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <returns></returns>
    public bool Contains(T value)
    {

      if (HasValue)
      {
        return value == null ? _value == null : value.Equals(_value);
      }

      return false;
    }

    /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
    public bool Equals(Optional<T> other)
    {
      if (other.HasValue && Contains(other._value))
        return true;
      return !HasValue && !other.HasValue;
    }

    /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
    public bool Equals(T other)
    {
      return this.Contains(other);
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
