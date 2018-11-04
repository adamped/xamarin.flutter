/*
 * Copyright (C) 2013 The Android Open Source Project
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



namespace minikin
{

    public class CmapCoverage
    {
        public SparseBitSet getCoverage(byte cmap_data, int cmap_size, ref bool has_cmap_format14_subtable)
        {
            const int kHeaderSize = 4;
            const int kNumTablesOffset = 2;
            const int kTableSize = 8;
            const int kPlatformIdOffset = 0;
            const int kEncodingIdOffset = 2;
            const int kOffsetOffset = 4;
            const int kFormatOffset = 0;
            const uint kInvalidOffset = UINT32_MAX;

            if (kHeaderSize > cmap_size)
            {
                return SparseBitSet();
            }
            uint numTables = readU16(cmap_data, kNumTablesOffset);
            if (kHeaderSize + numTables * kTableSize > cmap_size != null)
            {
                return SparseBitSet();
            }

            uint bestTableOffset = new uint(kInvalidOffset);
            UInt16 bestTableFormat = 0;
            byte bestTablePriority = kLowestPriority;
            has_cmap_format14_subtable = false;
            for (uint i = 0; i < numTables; ++i)
            {
                uint tableHeadOffset = kHeaderSize + i * kTableSize;
                UInt16 platformId = readU16(cmap_data, tableHeadOffset + kPlatformIdOffset);
                UInt16 encodingId = readU16(cmap_data, tableHeadOffset + kEncodingIdOffset);
                uint offset = readU32(cmap_data, tableHeadOffset + kOffsetOffset);

                if (offset > cmap_size - 2)
                {
                    continue; // Invalid table: not enough space to read.
                }
                UInt16 format = readU16(cmap_data, offset + kFormatOffset);

                if (platformId == 0 && encodingId == 5)
                {
                    if (!has_cmap_format14_subtable && format == 14)
                    {
                        has_cmap_format14_subtable = true;
                    }
                    else
                    {
                        // Ignore the (0, 5) table if we have already seen another valid one or
                        // it's in a format we don't understand.
                    }
                }
                else
                {
                    uint length = new uint();
                    uint language = new uint();

                    if (format == 4)
                    {
                        const int lengthOffset = 2;
                        const int languageOffset = 4;
                        const int minTableSize = languageOffset + 2;
                        if (offset > cmap_size - minTableSize)
                        {
                            continue; // Invalid table: not enough space to read.
                        }
                        length = readU16(cmap_data, offset + lengthOffset);
                        language = readU16(cmap_data, offset + languageOffset);
                    }
                    else if (format == 12)
                    {
                        const int lengthOffset = 4;
                        const int languageOffset = 8;
                        const int minTableSize = languageOffset + 4;
                        if (offset > cmap_size - minTableSize)
                        {
                            continue; // Invalid table: not enough space to read.
                        }
                        length = readU32(cmap_data, offset + lengthOffset);
                        language = readU32(cmap_data, offset + languageOffset);
                    }
                    else
                    {
                        continue;
                    }

                    if (length > cmap_size - offset)
                    {
                        continue; // Invalid table: table length is larger than whole cmap data
                                  // size.
                    }
                    if (language != 0)
                    {
                        // Unsupported or invalid table: this is either a subtable for the
                        // Macintosh platform (which we don't support), or an invalid subtable
                        // since language field should be zero for non-Macintosh subtables.
                        continue;
                    }
                    byte priority = getTablePriority(new UInt16(platformId), new UInt16(encodingId));
                    if (priority < bestTablePriority)
                    {
                        //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
                        //ORIGINAL LINE: bestTableOffset = offset;
                        bestTableOffset.CopyFrom(offset);
                        //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
                        //ORIGINAL LINE: bestTablePriority = priority;
                        bestTablePriority.CopyFrom(priority);
                        //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
                        //ORIGINAL LINE: bestTableFormat = format;
                        bestTableFormat.CopyFrom(format);
                    }
                }
                if (has_cmap_format14_subtable && bestTablePriority == 0)
                {
                    // Already found the highest priority table and variation sequences table.
                    // No need to look at remaining tables.
                    break;
                }
            }
            if (bestTableOffset == kInvalidOffset)
            {
                return SparseBitSet();
            }
            byte tableData = cmap_data + bestTableOffset;
            int tableSize = cmap_size - bestTableOffset;
            vector<uint> coverageVec = new vector<uint>();
            bool success;
            if (bestTableFormat == 4)
            {
                success = getCoverageFormat4(coverageVec, tableData, tableSize);
            }
            else
            {
                success = getCoverageFormat12(coverageVec, tableData, tableSize);
            }
            if (success && (coverageVec.size() != 0))
            {
                return SparseBitSet(coverageVec.front(), coverageVec.size() >> 1);
            }
            else
            {
                return SparseBitSet();
            }
        }
    }

} // namespace minikin

