using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AlgebraicDataTypes.Tests
{
  public sealed class OptionalTests
  {
    [Fact]
    public void Optional_CreateOptionalWithValue_Int()
    {
      var optionalWithValue = Optional.CreateOptionalWithValue(100);

      Assert.Equal(100, optionalWithValue.GetValueOrDefault(0));
      Assert.True(optionalWithValue.Equals(100));
    }

    [Fact]
    public void Optional_CreateOptionalWithValue_NullString()
    {
      var optionalWithNullValue = Optional.CreateOptionalWithValue<string>(null);

      Assert.Null(optionalWithNullValue.GetValueOrDefault("default_string"));
      Assert.True(optionalWithNullValue.Equals(null));
    }

    [Fact]
    public void Optional_CreateNone()
    {
      //var none = Optional.CreateNone.Of<Optional<string>>();

      var none = new Optional<string>();
      Assert.False(none.HasValue);
    }
  }
}
