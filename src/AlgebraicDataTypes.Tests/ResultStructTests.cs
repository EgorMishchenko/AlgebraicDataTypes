using System;
using System.Threading.Tasks;
using Xunit;

namespace AlgebraicDataTypes.Tests
{
  public class ResultStructTests
  {
    [Fact]
    public void Result_SelectSuccess()
    {
      var successResult = GetSuccessResult(10);

      var successString = successResult.Select<string>(ok => "success", e => "error");

      Assert.Equal("success", successString);
    }

    [Fact]
    public void Result_SelectFailure()
    {
      var failureResult = GetFailureResult();

      var fail = failureResult.Select<string>(ok => "success", e => "error");

      Assert.Equal("error", fail);
    }

    [Fact]
    public async Task Result_SelectAsyncSuccess()
    {
      var successResult = GetSuccessResult(10);

      var successString = await successResult.SelectAsync<string>(ok => Task.FromResult("success"), e => Task.FromResult("error"));

      Assert.Equal("success", successString);
    }

    [Fact]
    public async Task Result_SelectAsyncFailure()
    {
      var failureResult = GetFailureResult();

      var failureString = await failureResult.SelectAsync<string>(ok => Task.FromResult("success"), e => Task.FromResult("error"));

      Assert.Equal("error", failureString);
    }

    [Fact]
    public void Result_HandleSuccess_InputDelegateReturnsResult_Success()
    {
      var successResult = GetSuccessResult(10);
      var result = successResult.HandleSuccess(v => Result.CreateSuccess(666));
        
      Assert.True(result.IsSuccess);
      Assert.Equal(Result.CreateSuccess(666), result);
    }

    [Fact]
    public void Result_HandleSuccess_InputDelegateReturnsResult_Failure()
    {
      var failureResult = GetFailureResult();
      var result = failureResult.HandleSuccess(v => Result.CreateSuccess(666));

      Assert.True(result.IsError);
      // TODO: Try to get error.
      Assert.Equal("Error (error message)", result.ToString());
    }

    [Fact]
    public void Result_HandleSuccess_InputDelegateReturnsInt_Success()
    {
      var successResult = GetSuccessResult(10);

      var resultWithSuccessString = successResult.HandleSuccess(v => 555);

      Assert.True(resultWithSuccessString.IsSuccess);
      Assert.Equal(Result.CreateSuccess(555), resultWithSuccessString);

      var value = resultWithSuccessString.Select(x => x, e => 0);
      Assert.Equal(555, value);
    }

    [Fact]
    public void Result_HandleFailure_InputDelegateReturnsResult()
    {
      var failureResult = GetFailureResult();

      var result = failureResult.HandleFailure(x => Result.CreateSuccess(999));

      Assert.True(result.IsSuccess);
      Assert.Equal(999, result.Select(x => x, e => 0));
    }

    [Fact]
    public void Result_HandleFailure_InputDelegateReturnsInt()
    {
      var failureResult = GetFailureResult();

      var newResult = failureResult.HandleFailure(x => 999);

      Assert.True(newResult.IsSuccess);
      Assert.Equal(999, newResult.Select(x => x, e => 0));
    }

    private Result<int, Error> GetSuccessResult(int successNumber)
    {
      return Result.CreateSuccess(successNumber);
    }

    private Result<int, Error> GetFailureResult()
    {
      return Result.CreateFailure<int, Error>(new Error("error message"));
    }
  }
}
