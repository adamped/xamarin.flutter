/*
 * Copyright (C) 2012 The Android Open Source Project
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */




// ---------------------------------------------------------------------------

namespace minikin
{

// This is an implementation of a set of integers. It is optimized for
// values that are somewhat sparse, in the ballpark of a maximum value
// of thousands to millions. It is particularly efficient when there are
// large gaps. The motivating example is Unicode coverage of a font, but
// the abstraction itself is fully general.
public class SparseBitSet
{
	private bool InstanceFieldsInitialized = false;

	private void InitializeInstanceFields()
	{
		kElMask = (1 << kLogBitsPerEl) - 1;
		kElFirst = ((uint)1) << kElMask;
	}

  // Create an empty bit set.
  public SparseBitSet()
  {
	  if (!InstanceFieldsInitialized)
	  {
		  InitializeInstanceFields();
		  InstanceFieldsInitialized = true;
	  }
	  this.mMaxVal = 0;
  }

  // Initialize the set to a new value, represented by ranges. For
  // simplicity, these ranges are arranged as pairs of values,
  // inclusive of start, exclusive of end, laid out in a uint32 array.
  public SparseBitSet(uint ranges, int nRanges) : this()
  {
	  if (!InstanceFieldsInitialized)
	  {
		  InitializeInstanceFields();
		  InstanceFieldsInitialized = true;
	  }
	initFromRanges(ranges, nRanges);
  }

//C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = default':
//  SparseBitSet(SparseBitSet&&) = default;
//C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = default':
//  SparseBitSet& operator =(SparseBitSet&&) = default;

  // Determine whether the value is included in the set
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool get(uint ch) const
  public bool get(uint ch)
  {
	if (ch >= mMaxVal)
	{
	  return false;
	}
	uint[] bitmap = mBitmaps[mIndices[ch >> kLogValuesPerPage]];
	uint index = ch & kPageMask;
	return (bitmap[index >> kLogBitsPerEl] & (kElFirst >> (index & kElMask))) != 0;
  }

  // One more than the maximum value in the set, or zero if empty
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: uint length() const
  public uint length()
  {
	  return mMaxVal;
  }

  // The next set bit starting at fromIndex, inclusive, or kNotFound
  // if none exists.
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: uint nextSetBit(uint fromIndex) const;
	public uint nextSetBit(uint fromIndex)
	{
	  if (fromIndex >= mMaxVal)
	  {
		return GlobalMembers.kNotFound;
	  }
	  uint fromPage = fromIndex >> kLogValuesPerPage;
	  element[] bitmap = &mBitmaps[mIndices[fromPage]];
	  uint offset = (fromIndex & kPageMask) >> kLogBitsPerEl;
	  element e = bitmap[offset] & (kElAllOnes >> (fromIndex & kElMask));
	  if (e != 0)
	  {
		return (fromIndex & ~kElMask) + CountLeadingZeros(e);
	  }
	  for (uint j = offset + 1; j < (1 << (kLogValuesPerPage - kLogBitsPerEl)); j++)
	  {
	//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
	//ORIGINAL LINE: e = bitmap[j];
		e.CopyFrom(bitmap[j]);
		if (e != 0)
		{
		  return (fromIndex & ~kPageMask) + (j << kLogBitsPerEl) + CountLeadingZeros(e);
		}
	  }
	  uint maxPage = (mMaxVal + kPageMask) >> kLogValuesPerPage;
	  for (uint page = fromPage + 1; page < maxPage; page++)
	  {
		UInt16 index = mIndices[page];
		if (index == mZeroPageIndex)
		{
		  continue;
		}
		bitmap = &mBitmaps[index];
		for (uint j = 0; j < (1 << (kLogValuesPerPage - kLogBitsPerEl)); j++)
		{
	//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
	//ORIGINAL LINE: e = bitmap[j];
		  e.CopyFrom(bitmap[j]);
		  if (e != 0)
		  {
			return (page << kLogValuesPerPage) + (j << kLogBitsPerEl) + CountLeadingZeros(e);
		  }
		}
	  }
	  return GlobalMembers.kNotFound;
	}

  public uint kNotFound = ~0u;

	public void initFromRanges(uint[] ranges, int nRanges)
	{
	  if (nRanges == 0)
	  {
		return;
	  }
	  uint maxVal = ranges[nRanges * 2 - 1];
	  if (maxVal >= kMaximumCapacity)
	  {
		return;
	  }
	  mMaxVal = maxVal;
	  mIndices.reset(Arrays.InitializeWithDefaultInstances<UInt16>((mMaxVal + kPageMask) >> kLogValuesPerPage));
	  uint nPages = calcNumPages(ranges, nRanges);
	  mBitmaps.reset(Arrays.InitializeWithDefaultInstances<element>(nPages << (kLogValuesPerPage - kLogBitsPerEl)));
	  mZeroPageIndex = noZeroPage;
	  uint nonzeroPageEnd = 0;
	  uint currentPage = 0;
	  for (int i = 0; i < nRanges; i++)
	  {
		uint start = ranges[i * 2];
		uint end = ranges[i * 2 + 1];
		LOG_ALWAYS_FATAL_IF(end < start); // make sure range size is nonnegative
		uint startPage = start >> kLogValuesPerPage;
		uint endPage = (end - 1) >> kLogValuesPerPage;
		if (startPage >= nonzeroPageEnd)
		{
		  if (startPage > nonzeroPageEnd)
		  {
			if (mZeroPageIndex == noZeroPage)
			{
			  mZeroPageIndex = (currentPage++) << (kLogValuesPerPage - kLogBitsPerEl);
			}
			for (uint j = nonzeroPageEnd; j < startPage; j++)
			{
			  mIndices[j] = mZeroPageIndex;
			}
		  }
		  mIndices[startPage] = (currentPage++) << (kLogValuesPerPage - kLogBitsPerEl);
		}
    
		int index = ((currentPage - 1) << (kLogValuesPerPage - kLogBitsPerEl)) + ((start & kPageMask) >> kLogBitsPerEl);
		int nElements = (end - (start & ~kElMask) + kElMask) >> kLogBitsPerEl;
		if (nElements == 1)
		{
		  mBitmaps[index] |= (kElAllOnes >> (start & kElMask)) & (kElAllOnes << ((~end + 1) & kElMask));
		}
		else
		{
		  mBitmaps[index] |= kElAllOnes >> (start & kElMask);
		  for (int j = 1; j < nElements - 1; j++)
		  {
			mBitmaps[index + j] = kElAllOnes;
		  }
		  mBitmaps[index + nElements - 1] |= kElAllOnes << ((~end + 1) & kElMask);
		}
		for (int j = startPage + 1; j < endPage + 1; j++)
		{
		  mIndices[j] = (currentPage++) << (kLogValuesPerPage - kLogBitsPerEl);
		}
	//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
	//ORIGINAL LINE: nonzeroPageEnd = endPage + 1;
		nonzeroPageEnd.CopyFrom(endPage + 1);
	  }
	}

  private const uint kMaximumCapacity = 0xFFFFFF;
  private const int kLogValuesPerPage = 8;
  private int kPageMask = (1 << kLogValuesPerPage) - 1;
  private const int kLogBytesPerEl = 2;
  private int kLogBitsPerEl = kLogBytesPerEl + 3;
  private int kElMask;
  // invariant: sizeof(element) == (1 << kLogBytesPerEl)
  private uint kElAllOnes = ~((uint)0);
  private uint kElFirst;
  private const UInt16 noZeroPage = 0xFFFF;

	public uint calcNumPages(uint[] ranges, int nRanges)
	{
	  bool haveZeroPage = false;
	  uint nonzeroPageEnd = 0;
	  uint nPages = 0;
	  for (int i = 0; i < nRanges; i++)
	  {
		uint start = ranges[i * 2];
		uint end = ranges[i * 2 + 1];
		uint startPage = start >> kLogValuesPerPage;
		uint endPage = (end - 1) >> kLogValuesPerPage;
		if (startPage >= nonzeroPageEnd)
		{
		  if (startPage > nonzeroPageEnd)
		  {
			if (!haveZeroPage)
			{
			  haveZeroPage = true;
			  nPages++;
			}
		  }
		  nPages++;
		}
		nPages += endPage - startPage;
	//C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
	//ORIGINAL LINE: nonzeroPageEnd = endPage + 1;
		nonzeroPageEnd.CopyFrom(endPage + 1);
	  }
	  return nPages;
	}
	public int CountLeadingZeros(element x)
	{
	  return sizeof(element) <= sizeof(int) ? clz_win(x) : clzl_win(x);
	}

  private uint mMaxVal = new uint();

  private std::unique_ptr<UInt16[]> mIndices = new std::unique_ptr<UInt16[]>();
  private std::unique_ptr<uint[]> mBitmaps = new std::unique_ptr<uint[]>();
  private UInt16 mZeroPageIndex = new UInt16();

  // Forbid copy and assign.
//C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
//  SparseBitSet(const SparseBitSet&) = delete;
//C++ TO C# CONVERTER TODO TASK: C# has no equivalent to ' = delete':
//  void operator =(const SparseBitSet&) = delete;
}

} // namespace minikin

