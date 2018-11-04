/*
 * Copyright 2018 Google, Inc.
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

/*
 * Copyright 2018 Google Inc.
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


//C++ TO C# CONVERTER WARNING: The following #include directive was ignored:
//#include "third_party/skia/include/core/SkColor.h"
//C++ TO C# CONVERTER WARNING: The following #include directive was ignored:
//#include "third_party/skia/include/core/SKPoint.h"

using SkiaSharp;

namespace FlutterBinding.Txt
{

    public class TextShadow
    {
        public SKColor color = SKColor.Parse("#000");// SK_ColorBLACK;
        public SKPoint offset = new SKPoint();
        public double blur_radius = 0.0;

        public TextShadow()
        {
        }

        public TextShadow(SKColor color, SKPoint offset, double blur_radius)
        {
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: this.color = color;
            this.color = color;
            //C++ TO C# CONVERTER TODO TASK: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created:
            //ORIGINAL LINE: this.offset = offset;
            this.offset = offset;
            this.blur_radius = blur_radius;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: bool operator ==(const TextShadow& other) const
        //public static bool operator ==(TextShadow ImpliedObject, TextShadow other)
        //{
        //    if (ImpliedObject.color != other.color)
        //    {
        //        return false;
        //    }
        //    if (ImpliedObject.offset != other.offset)
        //    {
        //        return false;
        //    }
        //    if (ImpliedObject.blur_radius != other.blur_radius)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: bool operator !=(const TextShadow& other) const
        //public static bool operator !=(TextShadow ImpliedObject, TextShadow other)
        //{
        //    return !(*ImpliedObject == other);
        //}

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: bool hasShadow() const
        public bool hasShadow()
        {
            if (!offset.IsEmpty)
            {
                return true;
            }
            if (blur_radius != 0.0)
            {
                return true;
            }

            return false;
        }
    }

} // namespace FlutterBinding.Txt


//C++ TO C# CONVERTER WARNING: The following #include directive was ignored:
//#include "third_party/skia/include/core/SkColor.h"

