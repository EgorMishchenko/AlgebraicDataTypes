using System;
using System.Collections.Generic;
using System.Text;

namespace AlgebraicDataTypes
{
  public class CaughtExceptionError : Error
  {
    public CaughtExceptionError(Exception exc)
      : base(exc.Message)
    {
      this.Exception = exc;
    }

    public Exception Exception { get; }
  }
}
