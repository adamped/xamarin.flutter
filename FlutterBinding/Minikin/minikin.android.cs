namespace minikin
{

	public class android
	{
//C++ TO C# CONVERTER WARNING: The original C++ declaration of the following method implementation was not found:
		public android.hash_t FontStyle.hash()
		{
		  uint hash = android.JenkinsHashMix(0, bits);
		  hash = android.JenkinsHashMix(hash, mLanguageListId);
		  return android.JenkinsHashWhiten(hash);
		}
	}
}