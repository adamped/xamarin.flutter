namespace flow
{

	public class CounterValues
	{
//C++ TO C# CONVERTER WARNING: The original C++ declaration of the following method implementation was not found:
		public void Add(long value)
		{
		  current_sample_ = (current_sample_ + 1) % kMaxSamples;
		  values_[current_sample_] = value;
		}
	}
}