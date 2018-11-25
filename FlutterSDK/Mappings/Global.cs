﻿using FlutterSDK.Animation;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FlutterSDK.Widgets;

namespace FlutterSDK
{
    // Instead of changing up code, here I have implemented a number of common things, to tie their approach to .NET

    public delegate void AnimationStatusListener(AnimationStatus status);

    public delegate bool RoutePredicate(Route<object> route);

    public delegate Task<bool> WillPopCallback();

    public delegate void TickerCallback(TimeSpan elapsed);

    public delegate void DataColumnSortCallback(int columnIndex, bool ascending);

    public delegate void RegisterServiceExtensionCallback(string name, ServiceExtensionCallback callback);

    public delegate void ElementVisitor(Element element);

    /// <summary>
    /// Signature for service extensions.
    /// The returned map must not contain the keys "type" or "method", as
    /// they will be replaced before the value is sent to the client. The
    /// "type" key will be set to the string `_extensionType` to indicate
    /// that this is a return value from a service extension, and the
    /// "method" key will be set to the full name of the method.
    /// </summary>
    public delegate Task<Dictionary<string, object>> ServiceExtensionCallback(Dictionary<string, string> parameters);

    public enum TextDirection
    {
        ltr,
        rtl
    }

    public enum TextAlign
    {
        left
    }

    public class Matrix4 // TODO
    {

    }

    public class Function
    { }

    public class Radius
    { }


    public class Completer<T> // todo
    {
        public Future<T> future { get; set; }

        public void complete(T callback)
        {

        }
    }

    public class Stream<T> // todo
    {

    }

    public class Timeline
    {
       
    }

    public class Null // todo
    { }

    public class StackTrace // todo
    { }

    public class num
    {
        double value;

        public static implicit operator double(num x)
        {
            return x.value;
        }

        public static implicit operator num(double x)
        {
            return new num() { value = x };
        }
    }

    public class AssertionError : System.Exception
    {
        public AssertionError() { }
        public AssertionError(string message) { }
        public string message => this.Message;
    }

    public delegate void VoidCallback();

    public class Rect { }
    public class Color
    {
        public Color(uint? i)
        {

        }
    }
    public class Size
    {
        public Size(double? height)
        {
            this.height = height;
        }

        public Size(double? width, double? height)
        {
            this.width = width;
            this.height = height;
        }

        public double? width { get; set; }
        public double? height { get; set; }
    }


    public class Offset //TODO
    {
        public Offset() { }
        public Offset(double? one, double? two) { }

        public static Offset zero => new Offset();

        public double? dx { get; set; }
        public double? dy { get; set; }
    }

    public class ByteData //TODO
    { }

    public class Future: Task
    {
        public Future(Action action) : base(action) { }
    }

    public class Future<T>: Task<T>
    {
        public Future(Func<T> func) : base(func) { }
    }

    public class HashSet<T> : System.Collections.Generic.HashSet<T>
    {
        public HashSet<T> from(List<T> newList)
        {
            var h = new HashSet<T>();
            foreach (var item in newList)
                h.Add(item);

            return h;
        }
    }

    public class Timer
    {
        public static void run(Action task)
        {

        }
    }

    public class AppLifecycleState
    {
        public const AppLifecycleState suspending = null;
        public const AppLifecycleState paused = null;
        public const AppLifecycleState resumed = null;
        public const AppLifecycleState inactive = null;
    }


    public class StringBuffer // Similar to System.Text.StringBuilder
    {
        private string _value = "";
        public void write(string value)
        {
            _value += value;
        }

        public string toString()
        {
            return _value;
        }
    }

    public class Dictionary<T, V> : System.Collections.Generic.Dictionary<T, V>
    {
        public void remove(T element) => this.Remove(element);
    }

    public class List<T> : System.Collections.Generic.List<T>
    {
        public bool contains(object element)
        {
            if (element is T)
                if (this.Contains((T)element))
                    return true;

            return false;
        }

        public bool contains(T element)
        {
            if (this.Contains(element))
                return true;

            return false;
        }

        public int length()
        {
            return this.Count;
        }

        public List<T> from(List<T> newList)
        {
            return newList;
        }

        public bool isEmpty => this.Count == 0;

        public void add(T element) => this.Add(element);
    }

    public class Iterable<T> : List<T>
    {
    }

    public class Iterator<T> : Iterable<T>
    { }

    public static class Global
    {
        public static bool Identical(object main, object other)
         => main == other;

        public static string DescribeIdentity(object obj)
        {
            return $"{obj.GetType()}#{obj.GetHashCode()}";
        }

        public static string ShortHash(object obj)
        {
            return obj.GetHashCode().ToUnsigned(20).ToRadixString(16).PadLeft(5, '0');
        }

        public static int ToUnsigned(this int value, int bits)
        {
            return value >> bits;
        }

        public static string ToRadixString(this int value, int radix)
        {
            var digits = "0123456789abcdefghijklmnopqrstuvwxyz";

            radix = Math.Abs(radix);
            if (radix > digits.Length || radix < 2)
                throw new ArgumentOutOfRangeException("radix", radix, string.Format("Radix has to be > 2 and < {0}", digits.Length));

            string result = string.Empty;
            int quotient = Math.Abs(value);
            while (0 < quotient)
            {
                int temp = quotient % radix;
                result = digits[temp] + result;
                quotient /= radix;
            }
            return result;
        }

        public static double ToDouble(this int i) => Convert.ToDouble(i);
        public static double ToDouble(this double d) => d;

        public static string Join(this List<string> list, string separator) => string.Join(separator, list.ToArray());

        public static double TruncateToDouble(this double d) => Math.Truncate(d);

        public static string ToStringAsFixed(this double d, int value) => d.ToString($"N{value}");

        public static double InMicroseconds(this TimeSpan timespan)
        {
            return timespan.TotalMilliseconds * 1000;
        }

        public static double Clamp(this double d, double lower, double upper)
        {
            if (d < lower)
                return lower;

            if (d > upper)
                return upper;

            return d;
        }

        public static bool IsFinite(this double d)
        {
            return !double.IsInfinity(d);
        }

        public static double Abs(this double d)
        {
            return (double)Math.Abs(Convert.ToDecimal(d));
        }
    }
}