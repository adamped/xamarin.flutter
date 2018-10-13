using System.Text.RegularExpressions;

namespace Dart2CSharpTranspiler.Writer
{
    /// <summary>
    /// Provides common <see cref="Regex"/> instances.
    /// </summary>
    public static class RegexProvider
    {
        /// <summary>
        /// Regex for generic parameters.
        /// </summary>
        public static readonly Regex GenericParameterRegex = new Regex("<(.+)>");
    }
}