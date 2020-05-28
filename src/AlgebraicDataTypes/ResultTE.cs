using System;
using System.Threading.Tasks;

namespace AlgebraicDataTypes
{
  public readonly struct Result<T,E> where E : Error
  {
    private readonly T _value;
    private readonly E _error;

    internal Result(T value)
    {
      _value = value;
      _error = default(E);
    }

    internal Result(E error)
    {
      _value = default(T);
      _error = error;
    }

    public bool IsError => _error != null;

    public bool IsSuccess => _error == null;

    /// <summary>
    /// Executes success delegate if result is successful or execute failed delegate if result is a failure.
    /// </summary>
    /// <typeparam name="V">The type of value that will be returned if success.</typeparam>
    /// <param name="onSuccess">Delegate to execute in case of success.</param>
    /// <param name="onFail">Delegate to execute in case of failure.</param>
    /// <returns>The result of execution success or failed delegate.</returns>
    public V Select<V>(Func<T, V> onSuccess, Func<E, V> onFail) // maybe call this method "Execute"
    {
      return IsSuccess ? onSuccess(_value) : onFail(_error);
    }

    /// <summary>
    /// Asynchronously executes the delegate if result is successful or execute failed delegate if result is a failure.
    /// </summary>
    /// <typeparam name="V">The type of value that will be returned if success.</typeparam>
    /// <param name="success">Delegate to execute in case of success.</param>
    /// <param name="error">Delegate to execute in case of failure</param>
    /// <returns>Task with value from success or with value.</returns>
    public Task<V> SelectAsync<V>(Func<T, Task<V>> success, Func<E, Task<V>> error)
    {
      return IsSuccess ? success(_value) : error(_error);
    }

    /// <summary>
    /// Execute delegate that returns result if success or get failure result.
    /// </summary>
    /// <typeparam name="V">The type of value that will be returned if success.</typeparam>
    /// <param name="fn">Delegate to execute in success case that returns result.</param>
    /// <returns>Successful or failed result.</returns>
    public Result<V, E> HandleSuccess<V>(Func<T, Result<V, E>> fn) // former "Then"
    {
      return IsSuccess ? fn(_value) : new Result<V, E>(_error);
    }

    /// <summary>
    /// Execute delegate if success and put it into result or get failure result.
    /// </summary>
    /// <typeparam name="V">The type of value that will be returned if success.</typeparam>
    /// <param name="fn">Delegate to execute in success case</param>
    /// <returns>Successful or failed result.</returns>
    public Result<V, E> HandleSuccess<V>(Func<T, V> fn)
    {
      return IsSuccess ? new Result<V, E>(fn(_value)) : new Result<V, E>(_error);
    }

    public Task<Result<V, E>> HandleSuccessAsync<V>(Func<T, Task<Result<V, E>>> fn)
    {
      return IsSuccess ? fn(_value) : Task.FromResult<Result<V, E>>(new Result<V, E>(_error));
    }

    /// <summary>
    /// Execute delegate that returns result if failure or this result. Former "Handle"
    /// </summary>
    /// <param name="fn">Delegate to execute in failure case.</param>
    /// <returns></returns>
    public Result<T, E> HandleFailure(Func<E, Result<T, E>> fn) // former "Handle"
    {
      return IsError ? fn(_error) : this;
    }

    /// <summary>
    /// Execute delegate if failure and put it into result or get this result.
    /// </summary>
    /// <param name="fn">Delegate to execute in failure case.</param>
    /// <returns></returns>
    public Result<T, E> HandleFailure(Func<E, T> fn)
    {
      return IsError ? new Result<T, E>(fn(_error)) : this;
    }

    public Task<Result<T, E>> HandleFailureAsync(Func<E, Task<Result<T, E>>> fn)
    {
      return IsError ? fn(_error) : Task.FromResult<Result<T, E>>(this) ;
    }

    public override string ToString()
    {
      if (IsSuccess)
      {
        return "Ok (" + ((object) _value == null ? "NULL" : _value.ToString()) + ")";
      }

      return "Error (" + this._error.ToString() + ")";
    }
  }
}
