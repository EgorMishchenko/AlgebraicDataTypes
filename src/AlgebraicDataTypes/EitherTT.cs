using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AlgebraicDataTypes
{
  [DebuggerDisplay("{Is<T1>() ? \"Left\" : \"Right\",nq} {_ref}")]
  public readonly struct Either<T1, T2>
  {
    private static readonly StorageStrategy Strategy;
    private readonly object _ref;

    static Either()
    {
      if (Either.IsMutuallyNotConvertable(typeof(T1), typeof(T2)))
      {
        Strategy = StorageStrategy.ReferenceConversion;
      }
      else
      {
        Strategy = StorageStrategy.Box;
      }
    }

    private Either(object @ref)
    {
      this._ref = @ref;
    }

    /// <summary>
    /// Fill left.
    /// </summary>
    /// <param name="value"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Either<T1, T2>(T1 value)
    {
      if (Strategy == StorageStrategy.ReferenceConversion)
      {
        return new Either<T1, T2>((object) value ?? Either.LeftNullSentinel);
      }
      else
      {
        if ((object)value == null)
        {
          return new Either<T1, T2>(Either.LeftNullSentinel);
        }
        else
        {
          return new Either<T1, T2>((object) new Either.LeftBox<T1>(value));
        }
      }
    }

    /// <summary>
    /// Fill right.
    /// </summary>
    /// <param name="value"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Either<T1, T2>(T2 value)
    {
      if (Strategy == StorageStrategy.ReferenceConversion)
      {
        return new Either<T1, T2>((object) value ?? Either.RightNullSentinel);
      }
      else
      {
        if ((object)value == null)
        {
          return new Either<T1, T2>(Either.RightNullSentinel);
        }
        else
        {
          return new Either<T1, T2>((object) new Either.RightBox<T2>(value));
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="caseT1"></param>
    /// <param name="caseT2"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult Case<TResult>(Func<T1, TResult> caseT1, Func<T2, TResult> caseT2)
    {
      if (Strategy == StorageStrategy.ReferenceConversion)
      {
        if (_ref is T1)
        {
          var obj = (T1)_ref;
          return caseT1(obj);
        }
        else
        {
          if (_ref is T2)
          {
            var obj = (T2) _ref;
            return caseT2(obj);
          }
          else
          {
            return CheckSentinels(_ref, caseT1, caseT2);
          }
        }
      }

      if (_ref is Either.LeftBox<T1> leftBox)
      {
        return caseT1(leftBox.Value);
      }

      if (!(_ref is Either.RightBox<T2> rightBox))
      {
        return CheckSentinels(_ref, caseT1, caseT2);
      }
      else
      {
        return caseT2(rightBox.Value);
      }

      TResult CheckSentinels(object @ref, Func<T1, TResult> case1, Func<T2, TResult> case2)
      {
        if (@ref == Either.LeftNullSentinel)
          return case1(default(T1));
        if (@ref != Either.RightNullSentinel)
          throw new InvalidOperationException("Either in invalid state (possibly uninitialized)");
        return case2(default(T2));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Is<T>()
    {
      if ((object)typeof(T) != (object)typeof(T1) && (object)typeof(T) != (object)typeof(T2))
        throw new ArgumentException(typeof(T).Name + " is not one of the possible types of this Either");
      if (Either<T1, T2>.Strategy == Either<T1, T2>.StorageStrategy.ReferenceConversion)
      {
        if (this._ref is T)
          return true;
      }
      else
      {
        bool flag = (object)typeof(T) == (object)typeof(T1) ? this._ref is Either.LeftBox<T> : this._ref is Either.RightBox<T>;
        if (flag)
          return flag;
      }
      return (object)typeof(T) != (object)typeof(T1) ? this._ref == Either.RightNullSentinel : this._ref == Either.LeftNullSentinel;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Is(out T1 left)
    {
      if (Either<T1, T2>.Strategy == Either<T1, T2>.StorageStrategy.ReferenceConversion)
      {
        if (this._ref is T1 obj)
        {
          left = obj;
          return true;
        }
      }
      else if (this._ref is Either.LeftBox<T1> leftBox)
      {
        left = leftBox.Value;
        return true;
      }
      left = default(T1);
      return this._ref == Either.LeftNullSentinel;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="right"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Is(out T2 right)
    {
      if (Either<T1, T2>.Strategy == Either<T1, T2>.StorageStrategy.ReferenceConversion)
      {
        if (this._ref is T2 obj)
        {
          right = obj;
          return true;
        }
      }
      else if (this._ref is Either.RightBox<T2> rightBox)
      {
        right = rightBox.Value;
        return true;
      }
      right = default(T2);
      return this._ref == Either.RightNullSentinel;
    }

    private enum StorageStrategy
    {
      ReferenceConversion,
      Box,
    }
  }
}
