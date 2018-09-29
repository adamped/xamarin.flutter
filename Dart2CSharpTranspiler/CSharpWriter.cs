using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Transpiler
{
    public static class CSharpWriter
    {
        public static IList<string> Enums = new List<string>();

        public static string CreateFile(DartFile dart, DartModel model)
        {
            return "The CSharp File";
        }
    }
}