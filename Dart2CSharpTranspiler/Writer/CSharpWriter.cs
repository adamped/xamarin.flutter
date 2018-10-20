using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Transpiler;

namespace Dart2CSharpTranspiler.Writer
{
    public class CSharpWriter
    {  
        private readonly DartModel _model;

        /// <summary>
        /// Dictionary of all mixins representing the original name and generated interface name.
        /// </summary>
        public Dictionary<string, string> Mixins { get; }
         
        public static IList<string> Enums = new List<string>();

        private string _namespaceRoot;

        public CSharpWriter(DartModel model, string namespaceRoot)
        {
            _model = model;
            _namespaceRoot = namespaceRoot;
            Mixins = MixinAnalyzer.FindMixins(model);
        }

        /// <summary>
        /// Generates the namespace syntax that represents a dart file.
        /// </summary>
        public NamespaceDeclarationSyntax GenerateFileSyntaxTree(DartFile file)
        {
            var namespaceName = NameGenerator.GenerateNamespaceName(file.Folder, _namespaceRoot);
            var namespaceDeclartion = SyntaxFactory.NamespaceDeclaration(namespaceName)
                      .NormalizeWhitespace();
            namespaceDeclartion = ImportProcessor.AddUsings(file, namespaceDeclartion);

            var classGenerator = new FileProcessor(file, namespaceDeclartion, Mixins);
            namespaceDeclartion = classGenerator.AddFileInfoToNamespace();   

            return namespaceDeclartion;
        }
    } 
}