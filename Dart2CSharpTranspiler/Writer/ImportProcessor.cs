﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Transpiler;

namespace Dart2CSharpTranspiler.Writer
{
    public static class ImportProcessor
    {
        /// <summary>
        /// Adds usings to a <see cref="NamespaceDeclarationSyntax"/>.
        /// </summary>
        public static NamespaceDeclarationSyntax AddUsings(DartFile file, NamespaceDeclarationSyntax namespaceDeclartion)
        {
            List<UsingDirectiveSyntax> usings = new List<UsingDirectiveSyntax>();
            usings.AddRange(GenerateDefaultUsings());
            usings.AddRange(GenerateLibraryUsings(file));

            namespaceDeclartion = namespaceDeclartion.AddUsings(usings.ToArray());
            return namespaceDeclartion;
        }

        private static List<UsingDirectiveSyntax> GenerateLibraryUsings(DartFile file)
        {
            var usings = new List<UsingDirectiveSyntax>();
            foreach (var import in file.Imports)
            {
                // Find imports of this library
                var packagedImport = Regex.Match(import.Name, "flutter\\/([a-z]+).dart");
                if (packagedImport.Success)
                {
                    var importName = packagedImport.Groups.Last().Value; 
                    if (!RewriteRuleProvider.ShouldImportGetRemoved(importName))
                    { 
                        usings.Add(SyntaxFactory.UsingDirective(NameGenerator.GenerateNamespaceName(importName)));
                    }
                }
            }

            return usings;
        }

        private static List<UsingDirectiveSyntax> GenerateDefaultUsings()
        {
            return new List<UsingDirectiveSyntax>
            {
                SyntaxFactory.UsingDirective(NameGenerator.GenerateNamespaceName("System"))
            };
        }
    }
}