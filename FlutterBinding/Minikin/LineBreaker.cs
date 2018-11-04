using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 The Android Open Source Project
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





// Ordinarily, this method measures the text in the range given. However, when
// paint is nullptr, it assumes the widths have already been calculated and
// stored in the width buffer. This method finds the candidate word breaks
// (using the ICU break iterator) and sends them to addCandidate.

// add a word break (possibly for a hyphenated fragment), and add desperate
// breaks if needed (ie when word exceeds current line width)

// Helper method for addCandidate()

// TODO performance: could avoid populating mCandidates if greedy only



// Get the width of a space. May return 0 if there are no spaces.
// Note: if there are multiple different widths for spaces (for example, because
// of mixing of fonts), it's only guaranteed to pick one.
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: float LineBreaker::getSpaceWidth() const

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: float LineBreaker::currentLineWidth() const


// Follow "prev" links in mCandidates array, and copy to result arrays.




} // namespace minikin
