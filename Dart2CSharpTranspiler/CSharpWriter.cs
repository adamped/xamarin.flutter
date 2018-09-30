using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using System;
using System.Globalization;

namespace Transpiler
{
    public static class CSharpWriter
    {
        public static IList<string> Enums = new List<string>();

        public static string CreateFile(DartFile dart, DartModel model)
        {
            var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(dart.Folder.ToPascalCasing()))
                                          .NormalizeWhitespace();

            foreach (var @class in dart.Classes)
            {
                var classDeclaration = SyntaxFactory.ClassDeclaration(@class.Name.ToPascalCasing());

                classDeclaration = classDeclaration.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

                // Add class to namespace
                @namespace = @namespace.AddMembers(classDeclaration);
            }

            // Output code
            var code = @namespace
               .NormalizeWhitespace()
               .ToFullString();

            return code;
        }

        static string ToPascalCasing(this string value)
        {
            var resultBuilder = new System.Text.StringBuilder();
            
            // Replace anything, but letters and digits, with space
            foreach (char c in value)
            {
                if (!Char.IsLetterOrDigit(c))
                {
                    resultBuilder.Append(" ");
                }
                else
                {
                    resultBuilder.Append(c);
                }
            }

            string result = resultBuilder.ToString();

            result = result.ToLower();

            var textInfo = new CultureInfo("en-US", false).TextInfo;
            return textInfo.ToTitleCase(result).Replace(" ", string.Empty);            
        }
    }
}