using static FlutterBinding.Flow.Helper;

namespace FlutterBinding.Flow
{

	public partial class CounterValues
	{
//C++ TO C# CONVERTER WARNING: The original C++ declaration of the following method implementation was not found:
		public void Add(ulong value)
		{
		  current_sample_ = (current_sample_ + 1) % GlobalMembers.kMaxSamples;
		  values_[current_sample_] = value;
		}
	}
}