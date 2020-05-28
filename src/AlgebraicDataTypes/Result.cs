using System;
using System.Threading.Tasks;

namespace AlgebraicDataTypes
{
  public static class Result
  {
    /// <summary>
    /// Create success result with specified value.
    /// </summary>
    /// <typeparam name="T">The type of success result value.</typeparam>
    /// <param name="value">The value of success result.</param>
    /// <returns>New success result with specified value.</returns>
    public static Result<T,Error> CreateSuccess<T>(T value)
    {
      return new Result<T, Error>(value);
    }


    /// <summary>
    /// Create success result with specified value and specified error type.
    /// </summary>
    /// <typeparam name="T">The type of success result value.</typeparam>
    /// <typeparam name="E">The type of error</typeparam>
    /// <param name="value">The value of success result.</param>
    /// <returns>New success result with specified value.</returns>
    public static Result<T, E> CreateSuccess<T, E>(T value) where E : Error 
    {
      return new Result<T, E>(value);
    }

    /// <summary>
    /// Create failure result with specific error.
    /// </summary>
    /// <typeparam name="T">The type of result value.</typeparam>
    /// <typeparam name="E">The type of error.</typeparam>
    /// <param name="error"></param>
    /// <returns>New failed result with specific error.</returns>
    public static Result<T, E> CreateFailure<T, E>(E error) where E : Error 
    {
      if (error == null)
        throw new ArgumentNullException(nameof(error), "An error object must always be specified in the error case.");
      return new Result<T, E>(error);
    }

    /// <summary>
    /// Asynchronously executes the delegate if result is successful or execute failed delegate if result is a failure.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <typeparam name="E"></typeparam>
    /// <param name="this"></param>
    /// <param name="success"></param>
    /// <param name="failure"></param>
    /// <returns></returns>
    public static async Task<V> SelectAsync<T, V, E>(this Task<Result<T, E>> @this, Func<T, Task<V>> success, Func<E, Task<V>> failure)
      where E : Error
    {
      return await (await @this.ConfigureAwait(true)).SelectAsync(success, failure).ConfigureAwait(false);
    }

    public static async Task<Result<V, E>> HandleSuccessAsync<T, V, E>(
      this Task<Result<T, E>> @this,
      Func<T, Task<Result<V, E>>> fn)
      where E : Error
    {
      object obj = (await @this.ConfigureAwait(true)).Select(fn, e => (object)e);
      return obj is E error ? new Result<V, E>(error) : await ((Task<Result<V, E>>)obj).ConfigureAwait(false);
    }

    public static async Task<Result<T, E>> HandleAsync<T, E>(
      this Task<Result<T, E>> @this,
      Func<E, Task<Result<T, E>>> fn)
      where E : Error
    {
      Result<T, E> result = await @this.ConfigureAwait(true);
      return result.Select(t => t, error => (object)error) is E e ? await fn(e).ConfigureAwait(false) : result;
    }
  }
}
