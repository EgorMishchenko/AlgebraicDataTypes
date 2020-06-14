using System;

namespace AlgebraicDataTypes
{
  /// <summary>
  /// Represent Maybe monad which can contains some value or none.
  /// We can create Optional without value using new, because struct initialize with default values with new operator.
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
    public void Case(Action<T> some, Action none)
    {
      if (HasValue)
      {
        some(_value);
      }
      else
      {
        none();
      }

    }

    public U Case<U>(Func<T, U> some, U none)
    {
      if (HasValue)
      {
        return some(_value);
      }
      else
      {
        return none;
      }
    }

    public U Case<U>(Func<T, U> some, Func<U> none)
    {
      if (HasValue)
      {
        return some(_value);
      }
      else
      {
        return none();
      }
    }

    public Optional<U> Select<U>(Func<T, U> map)
    {
      if (HasValue)
      {
        return Optional.CreateOptionalWithValue<U>(map(_value));
      }
      else
      {
        return (Optional<U>) Optional.CreateNone;
      }
    }

    public Optional<U> Bind<U>(Func<T, Optional<U>> bind)
    {
      if (HasValue)
      {
        return bind(_value);
      }
      else
      {
        return Optional.CreateNone;
      }
    }

    public T Or(Func<T> none)
    {
      if (HasValue)
      {
        return _value;
      }
      else
      {
        return none();
      }
    }

    public Optional<T> Or(Func<Optional<T>> none)
    {
      if (HasValue)
      {
        return this;
      }
      else
      {
        return none();
      }
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
