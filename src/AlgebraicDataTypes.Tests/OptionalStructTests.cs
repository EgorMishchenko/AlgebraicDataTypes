using Xunit;

namespace AlgebraicDataTypes.Tests
{
  public class OptionalStructTests
  {
    #region Creating Optional
    // Int
    [Fact]
    public void Optional_CreateOptionalInt_WithoutValue()
    {
      var optionalWithoutValue = new Optional<int>();

      Assert.False(optionalWithoutValue.HasValue);
      Assert.Equal(0, optionalWithoutValue.GetValueOrDefault(0));
    }

    [Fact]
    public void Optional_CreateOptionalInt_WithValue()
    {
      Optional<int> optionalWithValue = 100;

      Assert.True(optionalWithValue.HasValue);
      Assert.Equal(100, optionalWithValue.GetValueOrDefault(0));
    }

    // String
    [Fact]
    public void Optional_CreateOptionalString_WithoutValue()
    {
      var optionalWithoutValue = new Optional<string>();

      Assert.False(optionalWithoutValue.HasValue);
      Assert.Equal("default_text", optionalWithoutValue.GetValueOrDefault("default_text"));
    }

    [Fact]
    public void Optional_CreateOptionalString_WithValue()
    {
      Optional<string> optionalWithValue = "text";

      Assert.True(optionalWithValue.HasValue);
      Assert.Equal("text", optionalWithValue.GetValueOrDefault("default_text"));
    }

    [Fact]
    public void Optional_CreateOptionalString_StringEmpty()
    {
      Optional<string> optionalWithValue = string.Empty;

      Assert.True(optionalWithValue.HasValue);
      Assert.Equal(string.Empty, optionalWithValue.GetValueOrDefault("default_text"));
    }

    [Fact]
    public void Optional_CreateOptionalString_NullValue()
    {
      Optional<string> optional = null;

      Assert.False(optional.HasValue);
      Assert.Equal("default_text", optional.GetValueOrDefault("default_text"));
    }

    // Class
    [Fact]
    public void Optional_CreateOptionalClass_WithoutValue()
    {
      var optionalWithoutValue = new Optional<TestClass>();
      
      var testClass = new TestClass { TestProperty = 0 };

      Assert.False(optionalWithoutValue.HasValue);
      Assert.Equal(testClass, optionalWithoutValue.GetValueOrDefault(testClass));
    }
    
    #endregion

    [Fact]
    public void Optional_Select_WithValue()
    {
      Optional<string> optionalWithValue = "string_value";

      var newOptional = optionalWithValue.Select(x => "new_string_value");

      Assert.Equal("new_string_value", newOptional.GetValueOrDefault(string.Empty));
    }

    [Fact]
    public void Optional_Select_WithNone()
    {
      var optionalNone = new Optional<string>();

      Assert.False(optionalNone.HasValue);
      var newOptional = optionalNone.Select(x => "on_none_case_string");

      Assert.Equal("", newOptional.GetValueOrDefault(string.Empty));
    }

    private class TestClass
    {
      public int TestProperty { get; set; }
    }
  }
}
