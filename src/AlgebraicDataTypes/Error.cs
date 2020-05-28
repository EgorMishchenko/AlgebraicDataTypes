using System;
using System.Collections.Generic;
using System.Text;

namespace AlgebraicDataTypes
{
  public class Error
  {
    public Error(string message)
    {
      if (string.IsNullOrWhiteSpace(message))
        throw new ArgumentOutOfRangeException(nameof(message), "Errors must always contain an error message");
      this.Message = message;
    }

    public string Message { get; }

    public override string ToString()
    {
      return this.Message;
    }
  }
}
