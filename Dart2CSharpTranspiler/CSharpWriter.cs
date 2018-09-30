using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;

namespace Transpiler
{
    public static class CSharpWriter
    {
        public static IList<string> Enums = new List<string>();

        public static string CreateFile(DartFile dart, DartModel model)
        {
            var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(dart.Folder))
                                          .NormalizeWhitespace();

            foreach (var @class in dart.Classes)
            {
                var classDeclaration = SyntaxFactory.ClassDeclaration(@class.Name);
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
    }
}