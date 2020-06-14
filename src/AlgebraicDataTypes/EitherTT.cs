using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AlgebraicDataTypes
{
  [DebuggerDisplay("{Is<T1>() ? \"Left\" : \"Right\",nq} {_ref}")]
  public readonly struct Either<T1, T2>
  {
    private static readonly StorageStrategy Strategy;
    private readonly object _ref;

    public Either(T1 value)
    {
      _ref = value;
    }

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
      _ref = @ref;
    }

    /// <summary>
    /// Fill left.
    /// </summary>
    /// <param name="value">The value that put to the left.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Either<T1, T2>(T1 value)
    {
      if (Strategy == StorageStrategy.ReferenceConversion)
      {
        return new Either<T1, T2>(value as object ?? Either.LeftNullSentinel);
      }
      else
      {
        if (value == null)
        {
          return new Either<T1, T2>(Either.LeftNullSentinel);
        }
        else
        {
          return new Either<T1, T2>(new Either.LeftBox<T1>(value));
        }
      }
    }

    /// <summary>
    /// Fill right.
    /// </summary>
    /// <param name="value">The value that put to the left.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Either<T1, T2>(T2 value)
    {
      if (Strategy == StorageStrategy.ReferenceConversion)
      {
        return new Either<T1, T2>(value as object ?? Either.RightNullSentinel);
      }
      else
      {
        if (value == null)
        {
          return new Either<T1, T2>(Either.RightNullSentinel);
        }
        else
        {
          return new Either<T1, T2>(new Either.RightBox<T2>(value));
        }
      }
    }

    /// <summary>
    /// Execute <see cref="caseT1"/> if left or execute <see cref="caseT2"/> if right.
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
    /// Check if <see cref="T"/> 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Is<T>()
    {
      if (typeof(T) != typeof(T1) && typeof(T) != typeof(T2))
      {
        throw new ArgumentException(typeof(T).Name + " is not one of the possible types of this Either");
      }
        
      if (Strategy == StorageStrategy.ReferenceConversion)
      {
        if (_ref is T)
          return true;
      }
      else
      {
        bool flag;
        if (typeof(T) == typeof(T1))
        {
          flag = _ref is Either.LeftBox<T>;
        }
        else
        {
          flag = _ref is Either.RightBox<T>;
        }

        if (flag)
        {
          return true;
        }
      }

      if (typeof(T) != typeof(T1))
      {
        return _ref == Either.RightNullSentinel;
      }
      else
      {
        return _ref == Either.LeftNullSentinel;
      }
    }

    /// <summary>
    /// Check if left is of <see cref="T1"/> 
    /// </summary>
    /// <param name="left"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsLeftOfType(out T1 left)
    {
      if (Strategy == StorageStrategy.ReferenceConversion)
      {
        if (_ref is T1 obj)
        {
          left = obj;
          return true;
        }
      }
      else if (_ref is Either.LeftBox<T1> leftBox)
      {
        left = leftBox.Value;
        return true;
      }
      left = default(T1);
      return _ref == Either.LeftNullSentinel;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="right"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsRightOfType(out T2 right)
    {
      if (Strategy == StorageStrategy.ReferenceConversion)
      {
        if (_ref is T2 obj)
        {
          right = obj;
          return true;
        }
      }
      else if (_ref is Either.RightBox<T2> rightBox)
      {
        right = rightBox.Value;
        return true;
      }
      right = default(T2);
      return _ref == Either.RightNullSentinel;
    }

    /// <summary>
    /// Cast Either to <see cref="T"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Cast<T>()
    {
      if (typeof(T) != typeof(T1) && typeof(T) != typeof(T2))
      {
        throw new ArgumentException("Type argument must be either " + typeof(T1).Name + " or " + typeof(T2).Name);
      }

      if (Strategy == StorageStrategy.ReferenceConversion)
      {
        if (_ref is T obj)
          return obj;
      }
      else if (typeof(T) == typeof(T1))
      {
        if (this._ref is Either.LeftBox<T> leftBox)
          return leftBox.Value;
      }
      else if (this._ref is Either.RightBox<T> rightBox)
      {
        return rightBox.Value;
      }

      if (_ref != (typeof(T) == typeof(T1) ? Either.LeftNullSentinel : Either.RightNullSentinel))
      {
        throw new InvalidCastException();
      }

      return default(T);
    }

    /// <summary>
    /// Map left to right.
    /// </summary>
    /// <typeparam name="U1"></typeparam>
    /// <param name="mapLeft"></param>
    /// <returns></returns>
    public Either<U1, T2> Select<U1>(Func<T1, U1> mapLeft)
    {
      if (IsLeftOfType(out T1 left))
      {
        return mapLeft(left);
      }
      else
      {
        return Cast<T2>();
      }
    }

    /// <summary>
    /// Map right to left.
    /// </summary>
    /// <typeparam name="U2"></typeparam>
    /// <param name="mapRight"></param>
    /// <returns></returns>
    public Either<T1, U2> Select<U2>(Func<T2, U2> mapRight)
    {
      if (IsRightOfType(out T2 right))
      {
        return mapRight(right);
      }
      else
      {
        return Cast<T1>();
      }
    }

    public async Task<Either<U1, T2>> SelectAsync<U1>(Func<T1, Task<U1>> mapLeft)
    {
      Either<U1, T2> either;
      if (IsLeftOfType(out T1 left))
      {
        either = await mapLeft(left).ConfigureAwait(false);
      }
      else
      {
        either = Cast<T2>();
      }
      
      return either;
    }

    public async Task<Either<T1, U2>> SelectAsync<U2>(Func<T2, Task<U2>> mapRight)
    {
      Either<T1, U2> either;
      if (IsRightOfType(out T2 right))
      {
        either = await mapRight(right).ConfigureAwait(false);
      }
      else
      {
        either = Cast<T1>();
      }

      return either;
    }

    public static Either<T1, T2> Return(T1 left)
    {
      return (Either<T1, T2>)left;
    }

    public static Either<T1, T2> Return(T2 right)
    {
      return (Either<T1, T2>)right;
    }

    public Either<U1, T2> Bind<U1>(Func<T1, Either<U1, T2>> bindLeft)
    {
      T1 left;
      return !this.IsLeftOfType(out left) ? (Either<U1, T2>)this.Cast<T2>() : bindLeft(left);
    }

    public Either<T1, U2> Bind<U2>(Func<T2, Either<T1, U2>> bindRight)
    {
      T2 right;
      return !this.IsRightOfType(out right) ? (Either<T1, U2>)this.Cast<T1>() : bindRight(right);
    }

    public Either<U1, U2> Bind<U1, U2>(
      Func<T1, Either<U1, U2>> bindLeft,
      Func<T2, Either<U1, U2>> bindRight)
    {
      T1 left;
      return !this.IsLeftOfType(out left) ? bindRight(this.Cast<T2>()) : bindLeft(left);
    }

    private enum StorageStrategy
    {
      ReferenceConversion,
      Box
    }
  }
}
