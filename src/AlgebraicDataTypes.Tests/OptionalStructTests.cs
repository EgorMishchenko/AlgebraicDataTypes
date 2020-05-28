using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AlgebraicDataTypes.Tests
{
  public class OptionalStructTests
  {
    [Fact]
    public void Optional_CreateOptionalWithoutValue()
    {
      var optionalWithoutValue = new Optional<int>();

      Assert.False(optionalWithoutValue.HasValue);
      Assert.Equal(0, optionalWithoutValue.GetValueOrDefault(0));
    }

    [Fact]
    public void Optional_CreateOptionalWithValue()
    {
      var optionalWithValue = new Optional<int>(10);

      Assert.True(optionalWithValue.HasValue);
      Assert.Equal(10, optionalWithValue.GetValueOrDefault(0));
    }


  }
}
