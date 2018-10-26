using FlutterBinding.UI;
using System;

namespace FlutterBinding.Mapping
{
    public static class Types
    {
        public enum Endian
        {
            big,
            little
        }
        
        public class Duration
        {
            public Duration(long milliseconds = 0, long microseconds = 0)
            {

            }

            public static Duration zero = new Duration(); //TODO: make an actual zero
        }

        public class ByteData
        {
            public ByteData() { }
            public ByteData(int value) { }

            public int getInt32(int first, int second) => 0; // TODO:
            public int getInt64(int first, int second) => 0; // TODO:
            public double getFloat64(int first, int second) => 0; // TODO:
            public double getFloat32(int first, int second) => 0; // TODO:

            public void setInt32(int first, int second, int third) { }
            public void setFloat32(double first, double second, int third) { }

            public int lengthInBytes => 0; // TODO;
        }

        public class Zone
        {
            public static Zone current = new Zone();
            public void runUnaryGuarded(PlatformMessageResponseCallback callback, ByteData data)
            {
                // TODO:
            }

            public void runUnaryGuarded<A>(Action<A> callback, A data)
            {
                // TODO:
            }

            public void runGuarded(VoidCallback callback)
            {
                // TODO:
            }

            public void runBinaryGuarded<A1, A2>(Action<A1, A2> callback, A1 arg1, A2 arg2)
            {
                // TODO:
            }
        }

        public class StateError : Exception {
            public StateError(string message) : base(message) { }
        }


    }
}
