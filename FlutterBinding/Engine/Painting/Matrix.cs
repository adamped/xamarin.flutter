using SkiaSharp;
using System.Collections.Generic;

namespace FlutterBinding.Engine.Painting
{
    public class Matrix
    {
        // Mappings from SkMatrix-index to input-index.
        static int[] kSkMatrixIndexToMatrix4Index = new int[] {
                                                                    // clang-format off
                                                                    0, 4, 12,
                                                                    1, 5, 13,
                                                                    3, 7, 15,
                                                                    // clang-format on
                                                                };
        public static SKMatrix ToSkMatrix(List<double> matrix4)
        {
            SKMatrix sk_matrix = new SKMatrix();
            for (int i = 0; i < 9; ++i)
            {
                int matrix4_index = kSkMatrixIndexToMatrix4Index[i];
                if (matrix4_index < matrix4.Count)
                    sk_matrix.Values[i] = (float)matrix4[matrix4_index];
                else
                    sk_matrix.Values[i] = 0.0f;
            }
            return sk_matrix;
        }

    }
}
