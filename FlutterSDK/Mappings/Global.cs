using FlutterSDK.animation.animation;
using System;
using System.Threading.Tasks;
using FlutterSDK.Animation;

namespace FlutterSDK
{
    // Instead of changing up code, here I have implemented a number of common things, to tie their approach to .NET

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
        //public void timeSync<T>(string label, Action task, _TaskEntry<T> flow)
        //{

        //}
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

    public class Future<T> //TODO
    {
        // public TickerFuture complete() { return new TickerFuture(); }

        public Stream<T> asStream() { return new Stream<T>(); }
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

    public delegate void AnimationStatusListener(AnimationStatus status);

    public delegate Task<bool> WillPopCallback(); 

    public class Paint
    {
        public Color color { get; set; }
    }

    public class Canvas
    {
        public void drawRRect(RRect rect)
        {

        }
    }

    public class RRect
    {

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


    public class ui
    {

        public class Window
        {
            public object onBeginFrame { get; set; }
            public object onDrawFrame { get; set; }
        }

        public static Window window { get; set; }


        public class Picture
        {

        }

        public class PictureRecorder //TODO
        {
            public Picture endRecording() { return null; }
        }

        public class PointerData //TODO
        {
            public double? physicalX { get; set; }
            public double? physicalY { get; set; }
        }


        public static double? lerpDouble(num a, num b, double? t)
        {
            if (a == null && b == null)
                return null;
            a = a ?? 0.0;
            b = b ?? 0.0;
            return a + (b - a) * t;
        }
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

    public static class math
    {
        public static double? sin(double? one)
        {
            return System.Math.Sin(one.Value);
        }
        public static double? cos(double? one)
        {
            return System.Math.Cos(one.Value);
        }
        public static double? max(double? first, double? second) => Math.Max(first.Value, second.Value);
        public static double? min(double? first, double? second) => Math.Min(first.Value, second.Value);
        public static double? sqrt(double? value) => Math.Sqrt(value.Value);

        public static double pi => 3.1415926535897932;
        public static double e => 2.718281828459045;
        public static double? pow(double? one, double? two)
        {
            return System.Math.Pow(one.Value, two.Value);
        }
        public static double? log(double? d)
        {
            return System.Math.Log(d.Value);
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
        public static string runtimeType = "runtimeType";

        public static string describeIdentity(object something)
        {
            return "describeIdentity";
        }

        public static void assert(object obj)
        {
            // We do nothing at the moment
        }

        public static double? toDouble(this int i) => Convert.ToDouble(i);

        public static double? truncateToDouble(this double? d) => System.Math.Truncate(d.Value);

        public static string toStringAsFixed(this double? d, int value) => d.Value.ToString($"N{value}");

        public static double? clamp(this double? d, double? lower, double? upper)
        {
            if (d < lower)
                return lower;

            if (d > upper)
                return upper;

            return d;
        }

        public static bool isFinite(this double? d)
        {
            return !double.IsInfinity(d.Value);
        }

        public static double? abs(this double? d)
        {
            return (double)Math.Abs(Convert.ToDecimal(d));
        }
    }
}