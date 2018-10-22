using FlutterBinding.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlutterBinding.Mapping
{
    public static class Types
    {

        public class Duration
        {
            public static Duration zero = new Duration(); //TODO: make an actual zero
        }

        public class ByteData
        { }

        public class Zone
        {
            public static Zone current = new Zone();
            public void runUnaryGuarded(PlatformMessageResponseCallback callback, ByteData data)
            {
                // TODO:
            }
        }


    }
}
