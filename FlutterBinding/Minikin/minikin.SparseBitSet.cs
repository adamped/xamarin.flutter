namespace minikin
{

	public class SparseBitSet
	{
//C++ TO C# CONVERTER WARNING: The original C++ declaration of the following method implementation was not found:
		public int CountLeadingZeros(element x)
		{
		  // Note: GCC / clang builtin
		  return sizeof(element) <= sizeof(int) ? __builtin_clz(x) : __builtin_clzl(x);
		}
	}
}