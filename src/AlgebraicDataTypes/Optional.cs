using System;
using System.Collections.Generic;
using System.Text;

namespace AlgebraicDataTypes
{
  public static class Optional
  {
    /// <summary>
    /// Create new instance of <see cref="Optional{T}"/> with value.
    /// </summary>
    /// <typeparam name="T">Type of value contained in optional.</typeparam>
    /// <param name="value">Value contained in optional.</param>
    /// <returns>New instance of optional with specified value.</returns>
    public static Optional<T> CreateOptionalWithValue<T>(T value)
    {
      return new Optional<T>(value);
    }
  }
}
