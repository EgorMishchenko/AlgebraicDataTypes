using System.Runtime.InteropServices;

namespace AlgebraicDataTypes
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public readonly struct ProtoNone
  {
    public Optional<T> Of<T>()
    {
      return new Optional<T>();
    }
  }
}
