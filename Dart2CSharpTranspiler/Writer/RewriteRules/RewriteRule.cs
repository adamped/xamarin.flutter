
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Dart2CSharpTranspiler.Writer
{
    public abstract class RewriteRule
    {
        // Class name settings
        public abstract string ReplacementClassName { get; }
        public abstract string DartClassName { get; }
        
        // Adding using settings
        public bool RequiresUsing => !string.IsNullOrEmpty(RequiredUsing);
        public abstract string RequiredUsing { get; }  

        // Remove import settings
        public bool RemovesImport => !string.IsNullOrEmpty(RemovedImport);
        public abstract string RemovedImport { get; }

        public NamespaceDeclarationSyntax ApplyToNamespace(NamespaceDeclarationSyntax namespaceDeclaration)
        {
            // Check if the rewrite rule requires an using and the using is not already there
            if (RequiresUsing
                && !namespaceDeclaration.Usings.Any(x => x.Name.ToString().Equals(RequiredUsing)))
            {
                namespaceDeclaration = namespaceDeclaration.AddUsings(SyntaxFactory.UsingDirective(NameGenerator.GenerateNamespaceName(RequiredUsing)));
            }

            return namespaceDeclaration;
        }
    }
}