using Xunit;

namespace AlgebraicDataTypes.Tests
{
  public class EitherStructTests
  {
    [Fact]
    public void Either_Case_RightCase()
    {
      var either = (Either<int, string>)"test";

      var result = either.Case(c1 => "case_1", c2 => "case_2");
      Assert.Equal("case_2", result);
    }

    [Fact]
    public void Either_Case_LeftCase()
    {
      var either = (Either<int, string>)10;

      var result = either.Case(c1 => "case_1", c2 => "case_2");
      Assert.Equal("case_1", result);
    }

    [Fact]
    public void Either_Is_EitherIsString()
    {
      var either = (Either<int, string>)"test";
      var isString = either.Is<string>();

      Assert.True(isString);
    }

    [Fact]
    public void Either_Is_EitherIsInt()
    {
      var either = (Either<int, string>) 10;
      var isInt = either.Is<int>();

      Assert.True(isInt);
    }

    [Fact]
    public void Either_Is_EitherNull()
    {
      var either = (Either<int, string>) null;
      var isString = either.Is<string>();

      Assert.True(isString);
    }

    [Fact]
    public void Either_Is_EitherIsLeftString()
    {
      var either = (Either<string, int>)"test";

      var isLeftString = either.IsLeftOfType(out string left);

      Assert.True(isLeftString);
      Assert.Equal("test", left);
    }

    [Fact]
    public void Either_Is_EitherIsRightInt()
    {
      Either<string, int> either = 999;

      var isRightInt = either.IsRightOfType(out int right);

      Assert.True(isRightInt);
      Assert.Equal(999, right);
    }

    [Fact]
    public void Either_Select()
    {
      Either<int, string> either = "six hundreds"; // Either<T1, T2>  

      Either<bool, string> newStruct = either.Select<bool>((int x) => true);
    }

    private class TestClass
    {
      public int TestIntProp { get; set; }
    }
  }
}
