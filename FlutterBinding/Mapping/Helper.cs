using System;

namespace FlutterBinding.Mapping
{
    public static class Helper
    {
        public static bool identical(object first, object second) => first.Equals(second);

        public static string toStringAsFixed(this double value, int points)
        {
            return value.ToString($"N{points}");
        }

        public static double round(this double value) => Math.Round(value);

        public static int toInt(this double value) => Convert.ToInt32(value);

        public static int clamp(this int value, int lower, int upper)
        {
            if (value > upper)
                return upper;

            if (value < lower)
                return lower;

            return value;
        }

        public static bool isFinite(this double value) => !double.IsInfinity(value);

        public static double abs(this double value) => Math.Abs(value);
    }
}
