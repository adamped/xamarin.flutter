using FlutterBinding.UI;
using System;

namespace FlutterBinding.Mapping
{
    public static class Types
    {

        public class Duration
        {
            public Duration(long milliseconds = 0)
            {

            }

            public static Duration zero = new Duration(); //TODO: make an actual zero
        }

        public class ByteData
        {
            public ByteData() { }
            public ByteData(int value) { }

            public int getInt32(int first, int second) => 0; // TODO:
            public void setInt32(int first, int second, int third) { }
        }

        public class Zone
        {
            public static Zone current = new Zone();
            public void runUnaryGuarded(PlatformMessageResponseCallback callback, ByteData data)
            {
                // TODO:
            }
        }

        public class StateError : Exception {
            public StateError(string message) : base(message) { }
        }


    }
}
