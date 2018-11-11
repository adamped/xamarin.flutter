using FlutterBinding.UI;
using System;
using System.Collections.Generic;

namespace FlutterBinding.Mapping
{
    public static class Helper
    {
        public static bool identical(object first, object second) => first.Equals(second);

        public static int hashValues(object first, object second, object third = null, object fourth = null, object fifth = null, object sixth = null, object seventh = null, object eigth = null, object ninth = null) => 0; // TODO:

        public static int hashList(List<double> list) => 0; // TODO:
        public static int hashList(List<int> list) => 0; // TODO:

        public static string toStringAsFixed(this double value, int points)
        {
            return value.ToString($"N{points}");
        }

        public static string toRadixString(this uint value, int places) => value.ToString(); // TODO:

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

        public static double clamp(this double value, int lower, int upper)
        {
            if (value > upper)
                return upper;

            if (value < lower)
                return lower;

            return value;
        }

        public static bool isFinite(this double value) => !double.IsInfinity(value);

        public static double abs(this double value) => Math.Abs(value);
        
        public static Future<T> _futurize<T>(Action<_Callback<T>> callback)
        {
            // Question, why is this so complicated for running a new Task.
            // Could be a Dart -> C# translation issue

            var result = default(T);

            var resolve = new _Callback<T>((t) => { result = t; });

            return new Future<T>(() => { return result; });
        }

        public static Future _futurize(Action<_Callback> callback)
        {
            // Question, why is this so complicated for running a new Task.
            // Could be a Dart -> C# translation issue

            var resolve = new _Callback(()=> { });

            return new Future(() => {  });
        }

    }
}
