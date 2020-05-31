using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AlgebraicDataTypes
{
  public static class Either
  {
    internal static readonly object LeftNullSentinel = (object)new Either.NullSentinel(nameof(LeftNullSentinel));
    internal static readonly object RightNullSentinel = (object)new Either.NullSentinel(nameof(RightNullSentinel));

    internal static bool IsMutuallyNotConvertable(Type a, Type b)
    {
      return !HasPossibleConversion(a, b) && !HasPossibleConversion(b, a);
    }

    private static bool HasPossibleConversion(Type src, Type to)
    {
      TypeInfo srcType = src.GetTypeInfo();
      TypeInfo toType = to.GetTypeInfo();

      if (srcType.IsInterface && toType.IsClass && (toType.IsSealed &&
                                                    !toType.ImplementedInterfaces.Any(ti =>
                                                      ti == src || ti.GetTypeInfo().IsAssignableFrom(srcType)))
          || srcType.IsClass && srcType.IsSealed && (toType.IsInterface &&
                                                     !srcType.ImplementedInterfaces.Any(si =>
                                                       si == to || si.GetTypeInfo().IsAssignableFrom(toType))))
      {
        return false;
      }
      
      try
      {
        return (object)Expression.Convert(Expression.Parameter(src), to).Method == null;
      }
      catch (InvalidOperationException ex)
      {
        return false;
      }
    }

    [DebuggerDisplay("{Label}")]
    private sealed class NullSentinel
    {
      public NullSentinel([CallerMemberName] string label = null)
      {
        this.Label = label;
      }

      public string Label { get; }
    }

    internal sealed class LeftBox<T>
    {
      public T Value;

      public LeftBox(T value)
      {
        Value = value;
      }
    }

    internal sealed class RightBox<T>
    {
      public T Value;

      public RightBox(T value)
      {
        Value = value;
      }
    }
  }

}
