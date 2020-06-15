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

    [Fact]
    public void Optional_CreateOptionalClass_WithValue()
    {
      var testClass = new TestClass { TestProperty = 111 };

      Optional<TestClass> optional = testClass;

      var defaultTestClass = new TestClass { TestProperty = 0 };
     
      Assert.True(optional.HasValue);
      Assert.Equal(testClass, optional.GetValueOrDefault(defaultTestClass));
    }

    #endregion

    #region Contains

    [Fact]
    public void Optional_ContainsString_True()
    {
      Optional<string> optional = "text";

      Assert.True(optional.HasValue);
      Assert.True(optional.Contains("text"));
    }

    [Fact]
    public void Optional_ContainsDifferentString_False()
    {
      Optional<string> optional = "text";

      Assert.True(optional.HasValue);
      Assert.False(optional.Contains("different_text"));
    }

    [Fact]
    public void Optional_WithNull_Contains_False()
    {
      Optional<string> optionalWithoutValue = null;

      Assert.False(optionalWithoutValue.HasValue);
      Assert.False(optionalWithoutValue.Contains("text"));
    }

    [Fact]
    public void Optional_WithNull_ContainsNull_False()
    {
      Optional<string> optionalWithoutValue = null;

      Assert.False(optionalWithoutValue.HasValue);
      Assert.False(optionalWithoutValue.Contains(null));
    }

    #endregion

    #region Case

    [Fact]
    public void OptionalString_Case_TwoActions_WithValue()
    {
      Optional<string> optional = "text";

      Assert.True(optional.HasValue);

      var testClass = new TestClass();
      optional.Case(some => { testClass.TestMessage = some + " passed"; }, () => { testClass.TestMessage = "none"; });

      Assert.Equal("text passed", testClass.TestMessage);
      Assert.Equal(0, testClass.TestProperty);
    }

    [Fact]
    public void OptionalInt_Case_TwoActions_WithValue()
    {
      Optional<int> optional = 10;

      Assert.True(optional.HasValue);

      var testClass = new TestClass();
      optional.Case(some => { testClass.TestProperty = some + 1; }, () => { testClass.TestMessage = "none"; });

      Assert.Null(testClass.TestMessage);
      Assert.Equal(11, testClass.TestProperty);
    }

    [Fact]
    public void OptionalInt_Case_TwoActions_WithoutValue()
    {
      Optional<int> optional = new Optional<int>();

      Assert.False(optional.HasValue);

      var testClass = new TestClass();
      optional.Case(some => { testClass.TestProperty = some + 1; }, () => { testClass.TestMessage = "none"; });

      Assert.Equal("none", testClass.TestMessage);
      Assert.Equal(0, testClass.TestProperty);
    }

    [Fact]
    public void OptionalString_Case_TwoActions_WithoutValue()
    {
      Optional<string> optional = new Optional<string>();

      Assert.False(optional.HasValue);

      var testClass = new TestClass();
      optional.Case(some => { testClass.TestMessage = "some"; }, () => { testClass.TestMessage = "none"; });

      Assert.Equal("none", testClass.TestMessage);
      Assert.Equal(0, testClass.TestProperty);
    }

    [Fact]
    public void OptionalString_Case_FuncTUAndU_WithValue_ReturnInt()
    {
      Optional<string> optional = "text";

      Assert.True(optional.HasValue);
      Assert.Equal(10, optional.Case<int>(some => 10, 666));
    }

    [Fact]
    public void OptionalString_Case_FuncTUAndU_WithoutValue_ReturnInt()
    {
      Optional<string> optional = new Optional<string>();

      Assert.True(optional.HasValue);
      Assert.Equal(666, optional.Case<int>(some => 10, 666));
    }

    [Fact]
    public void OptionalString_Case_FuncTUAndU_WithValue_ReturnBool()
    {
      Optional<string> optional = "text";

      Assert.True(optional.HasValue);
      Assert.True(optional.Case<bool>(some => true, false));
    }

    [Fact]
    public void OptionalString_Case_FuncTUAndU_WithoutValue_ReturnBool()
    {
      Optional<string> optional = new Optional<string>();

      Assert.False(optional.HasValue);
      Assert.False(optional.Case<bool>(some => true, false));
    }

    [Fact]
    public void OptionalString_Case_FuncTUAndU_WithValue_ReturnString()
    {
      Optional<string> optional = "text";

      Assert.True(optional.HasValue);
      Assert.Equal("text changed", optional.Case<string>(some => some + " changed", "none"));
    }

    [Fact]
    public void OptionalString_Case_FuncTUAndU_WithoutValue_ReturnString()
    {
      Optional<string> optional = new Optional<string>();

      Assert.False(optional.HasValue);
      Assert.Equal("none", optional.Case<string>(some => some + " changed", "none"));
    }

    [Fact]
    public void OptionalString_Case_FuncTUAndFuncU_WithValue_ReturnInt()
    {
      Optional<string> optional = "text";

      Assert.True(optional.HasValue);
      Assert.Equal(10, optional.Case<int>(some => 10, () => 666));
    }

    [Fact]
    public void OptionalString_Case_FuncTUAndFuncU_WithoutValue_ReturnInt()
    {
      Optional<string> optional = new Optional<string>();

      Assert.False(optional.HasValue);
      Assert.Equal(666, optional.Case<int>(some => 10, () => 666));
    }

    [Fact]
    public void OptionalString_Case_FuncTUAndFuncU_WithoutValue_ReturnInt2()
    {
      Optional<string> optional = new Optional<string>();

      Assert.False(optional.HasValue);
      Assert.Equal(20, optional.Case<int>(some => 10, () =>
      {
        var result = 10;
        for (int i = 0; i < 10; i++)
        {
          result++;
        }

        return result;
      }));
    }

    #endregion

    #region Select

    [Fact]
    public void Optional_Select_WithValue()
    {
      Optional<string> optional = "text";

      Assert.True(optional.HasValue);

      var newOptional = optional.Select(some => "new_" + some);

      Assert.Equal("new_text", newOptional.GetValueOrDefault(string.Empty));
    }

    [Fact]
    public void Optional_Select_WithoutValue()
    {
      var optional = new Optional<string>();

      Assert.False(optional.HasValue);

      var newOptional = optional.Select<string>(some => "new_" + some);

      Assert.Equal("none", newOptional.GetValueOrDefault("none"));
    }


    [Fact]
    public void Optional_Select_WithValue_ReturnInt()
    {
      Optional<string> optional = "text";

      Assert.True(optional.HasValue);

      var newOptional = optional.Select<int>(some => 100);

      Assert.Equal(100, newOptional.GetValueOrDefault(0));
    }

    [Fact]
    public void Optional_Select_WithClassValue()
    {
      Optional<TestClass> optional = new TestClass{ TestMessage = "text", TestProperty = 99 };

      Assert.True(optional.HasValue);

      var newOptional = optional.Select(some =>
      {
        some.TestMessage = "new_text";
        return some;
      });

      Assert.Equal("new_text", newOptional.GetValueOrDefault(null).TestMessage);
    }

    #endregion

    #region Bind

    [Fact]
    public void Optional_Bind_WithStringValue()
    {
      Optional<string> optional = "text";

      Assert.True(optional.HasValue);

      var newOptional = optional.Bind<string>(some => "new_" + some);

      Assert.Equal("new_text", newOptional.GetValueOrDefault(string.Empty));
    }

    [Fact]
    public void Optional_Bind_WithStringValue2()
    {
      Optional<string> optional = "text";

      Assert.True(optional.HasValue);

      var newOptional = optional.Bind<string>(some => "new_" + some).Bind<string>(some => "new_" + some);

      Assert.Equal("new_new_text", newOptional.GetValueOrDefault(string.Empty));
    }

    [Fact]
    public void Optional_Bind_WithoutStringValue()
    {
      var optional = new Optional<string>();

      Assert.False(optional.HasValue);

      var newOptional = optional.Bind<string>(some => "new_" + some);

      Assert.Equal(string.Empty, newOptional.GetValueOrDefault(string.Empty));
    }

    [Fact]
    public void Optional_Bind_WithClassValue()
    {
      Optional<TestClass> optional = new TestClass { TestMessage = "text", TestProperty = 100 };

      Assert.True(optional.HasValue);

      var newOptional = optional.Bind<TestClass>(some => new TestClass{ TestMessage = "new_" + some.TestMessage});

      Assert.Equal("new_text", newOptional.GetValueOrDefault(null).TestMessage);
    }

    [Fact]
    public void Optional_Bind_WithoutClassValue()
    {
      var optional = new Optional<TestClass>();

      Assert.False(optional.HasValue);

      var newOptional = optional.Bind<TestClass>(some => new TestClass { TestMessage = "new_" + some.TestMessage });

      Assert.Null(newOptional.GetValueOrDefault(null));
    }

    #endregion

    #region Or

    [Fact]
    public void Optional_Or_WithStringValue()
    {
      Optional<string> optional = "text";

      Assert.True(optional.HasValue);

      Assert.Equal("text", optional.Or(() => "none"));
    }

    [Fact]
    public void Optional_Or_WithoutStringValue()
    {
      var optional = new Optional<string>();

      Assert.False(optional.HasValue);

      Assert.Equal("none", optional.Or(() => "none"));
    }

    [Fact]
    public void Optional_Or_WithIntValue()
    {
      Optional<int> optional = 99;

      Assert.True(optional.HasValue);

      Assert.Equal(99, optional.Or(() => 100));
    }

    [Fact]
    public void Optional_OrOptional_WithStringValue()
    {
      Optional<string> optional = "text";

      Assert.True(optional.HasValue);

      Assert.Equal((Optional<string>)"text", optional.Or(() => (Optional<string>)"none"));
    }

    [Fact]
    public void Optional_OrOptional_WithoutStringValue()
    {
      var optional = new Optional<string>();

      Assert.False(optional.HasValue);

      Assert.Equal((Optional<string>)"none", optional.Or(() => (Optional<string>)"none"));
    }

    #endregion

    private class TestClass
    {
      public int TestProperty { get; set; }
      public string TestMessage { get; set; }
    }
  }
}
