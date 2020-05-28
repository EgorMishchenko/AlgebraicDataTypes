using System.Threading.Tasks;
using Xunit;

namespace AlgebraicDataTypes.Tests
{
  public class ResultTests
  {
    [Fact]
    public void Result_GetSuccessResults()
    {
      var success = GetSuccessResult(10);

      Assert.True(success.IsSuccess);
      Assert.False(success.IsError);
    }

    [Fact]
    public async void Result_SelectAsync_Success()
    {
      var taskWithSuccessResult = Task.FromResult(GetSuccessResult(10));

      var result = await taskWithSuccessResult.SelectAsync(x => Task.FromResult(x), error => Task.FromResult(0));
      
      Assert.Equal(10, result);
    }

    [Fact]
    public async void Result_SelectAsync_Failure()
    {
      var taskWithSuccessResult = Task.FromResult(GetFailureResult());

      var result = await taskWithSuccessResult.SelectAsync(x => Task.FromResult(x), error => Task.FromResult(0));

      Assert.Equal(0, result);
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
