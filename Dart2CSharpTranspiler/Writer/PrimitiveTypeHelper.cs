using System;
using System.Linq;

namespace Dart2CSharpTranspiler.Writer
{
    /// <summary>
    /// Converts dart to c# types.
    /// </summary>
    public static class PrimitiveTypeHelper
    { 
        public static bool IsPrimitiveType(string type)
        {
            var primitives = new string[] { "void", "string", "double", "int", "bool" };
            return primitives.Any(primitive =>  primitive.Equals(type, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}