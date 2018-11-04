namespace minikin
{

	public class Hyphenator
	{
//C++ TO C# CONVERTER WARNING: The original C++ declaration of the following method implementation was not found:
		public void hyphenate(vector<HyphenationType> result, UInt16 word, int len, icu.Locale locale)
		{
		  result.clear();
		  result.resize(len);
		  int paddedLen = len + 2; // start and stop code each count for 1
		  if (patternData != null && len >= minPrefix + minSuffix && paddedLen <= MAX_HYPHENATED_SIZE)
		  {
			UInt16[] alpha_codes = Arrays.InitializeWithDefaultInstances<UInt16>(MAX_HYPHENATED_SIZE);
			HyphenationType hyphenValue = alphabetLookup(alpha_codes, word, len);
			if (hyphenValue != HyphenationType.DONT_BREAK)
			{
			  hyphenateFromCodes(result.data(), alpha_codes, paddedLen, hyphenValue);
			  return;
			}
			// TODO: try NFC normalization
			// TODO: handle non-BMP Unicode (requires remapping of offsets)
		  }
		  // Note that we will always get here if the word contains a hyphen or a soft
		  // hyphen, because the alphabet is not expected to contain a hyphen or a soft
		  // hyphen character, so alphabetLookup would return DONT_BREAK.
		  hyphenateWithNoPatterns(result.data(), word, len, locale);
		}
	}
}