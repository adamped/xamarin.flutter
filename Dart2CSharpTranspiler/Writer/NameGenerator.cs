using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Dart2CSharpTranspiler.Writer
{
    /// <summary>
    /// Generates names that follow the C# code style.
    /// </summary>
    public static class NameGenerator
    {
        /// <summary>
        /// Generates a the name for a namespace.
        /// </summary>
        public static NameSyntax GenerateNamespaceName(string text, string namespaceRoot = null)
        {
            var name = NormalizationHelper.CamelCase(text);
            if (namespaceRoot != null)
                name = namespaceRoot + "." + name;
            return SyntaxFactory.ParseName(name);
        }
    }
}