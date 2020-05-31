using Xunit;

namespace AlgebraicDataTypes.Tests
{
  public class EitherStructTests
  {
    [Fact]
    public void Either_Case()
    {
      var either = (Either<int, string>) 10;
      either = "test";

      var result = either.Case(c1 => "case_1", c2 => "case_2");
      Assert.Equal("case_2", result);
    }
  }
}
