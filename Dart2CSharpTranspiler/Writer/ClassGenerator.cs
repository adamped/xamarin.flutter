using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Transpiler;

namespace Dart2CSharpTranspiler.Writer
{
    /// <summary>
    /// Generates the content of a class.
    /// </summary>
    public static class ClassGenerator
    {
        /// <summary>
        /// Adds all classes of a <see cref="DartFile"/> to an existing <see cref="NamespaceDeclarationSyntax"/>.
        /// </summary>
        public static NamespaceDeclarationSyntax AddClasses(DartFile file, NamespaceDeclarationSyntax namespaceDeclartion, Dictionary<string, string> availableMixins)
        {
            foreach (var classModel in file.Classes)
            {
                var classDeclaration = GenerateClass(classModel, availableMixins);
                // Add class to namespace
                namespaceDeclartion = namespaceDeclartion.AddMembers(classDeclaration);
            }

            return namespaceDeclartion;
        }

        /// <summary>
        /// Generates an interface that abstracts a dart mixin.
        /// </summary>
        /// <param name="classModel"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static InterfaceDeclarationSyntax GenerateMixinInterface(DartClass classModel, string name)
        {
            var interfaceDeclaration = SyntaxFactory.InterfaceDeclaration(name);
            interfaceDeclaration = interfaceDeclaration.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
            interfaceDeclaration = (InterfaceDeclarationSyntax)AddConstraints(classModel, interfaceDeclaration);
            return interfaceDeclaration;
        }

        /// <summary>
        /// Generates a class base on a <see cref="DartClass"/> <paramref name="classModel"/>.
        /// </summary>
        /// <param name="classModel">Model the classed will be based on.</param>
        /// <param name="availableMixins">All known mixins of the library.</param>
        /// <returns></returns>
        private static ClassDeclarationSyntax GenerateClass(DartClass classModel, Dictionary<string, string> availableMixins)
        {
            var className = classModel.Name;
            var classDeclaration = SyntaxFactory.ClassDeclaration(NormalizationHelper.NormalizeTypeName(className));
            classDeclaration = (ClassDeclarationSyntax)AddConstraints(classModel, classDeclaration);
            classDeclaration = classDeclaration.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
            classDeclaration = AddBaseTypes(classModel, className, classDeclaration, availableMixins);
            classDeclaration = ClassSectionGenerator.AddSections(classModel, classDeclaration);
            return classDeclaration;
        }

        /// <summary>
        /// Adds base types to a class.
        /// </summary>
        /// <param name="classModel">The model for the class.</param>
        /// <param name="className">The name for the class.</param>
        /// <param name="classDeclaration">The <see cref="ClassDeclarationSyntax"/> the base types will be added to.</param>
        /// <param name="availableMixins">All known mixins of the library.</param> 
        private static ClassDeclarationSyntax AddBaseTypes(DartClass classModel, string className, ClassDeclarationSyntax classDeclaration, Dictionary<string, string> availableMixins)
        {
            if (classModel.Extends != null)
            {
                // Add base classes
                if (!string.IsNullOrEmpty(classModel.Extends.Name))
                {
                    var name = NormalizationHelper.NormalizeTypeName(classModel.Extends.Name);
                    var type = SyntaxFactory.ParseTypeName(name);

                    classDeclaration = classDeclaration.AddBaseListTypes(
                        SyntaxFactory.SimpleBaseType(type));
                }

                // Add mixin interfaces
                if (classModel.Extends.Mixins != null)
                {
                    foreach (var mixin in classModel.Extends.Mixins)
                    {
                        var mixingInterface = availableMixins[mixin.ToLower()];
                        // Mixins are abstracted as interfaces and the method implemenation is later added using composition
                        classDeclaration = classDeclaration.AddBaseListTypes(
                            SyntaxFactory.SimpleBaseType(
                                SyntaxFactory.ParseTypeName(mixingInterface)));
                    }
                }
            }

            // If this class is a mixing itself, the interface must be addded for the mixin
            if (availableMixins.ContainsKey(className))
            {
                var mixinInterfaceName = availableMixins[className];
                classDeclaration = classDeclaration.AddBaseListTypes(
                    SyntaxFactory.SimpleBaseType(
                        SyntaxFactory.ParseTypeName(mixinInterfaceName)));
            }

            return classDeclaration;
        }

        /// <summary>
        /// Adds constraints to a <see cref="TypeDeclarationSyntax"/>.
        /// </summary>
        public static TypeDeclarationSyntax AddConstraints(DartClass classModel, TypeDeclarationSyntax classDeclaration)
        {
            foreach (var constraint in classModel.GenericConstraints)
            {
                classDeclaration = classDeclaration.AddTypeParameterListParameters(SyntaxFactory.TypeParameter(constraint.Key)); 
                if (constraint.Value != null)
                {
                    //TODO Add constraint
                }
            }

            return classDeclaration;
        }


        /// <summary>
        /// Adds the interface for a mixin to a <see cref="NamespaceDeclarationSyntax"/>.
        /// </summary>
        public static NamespaceDeclarationSyntax AddMixinInterfaces(DartFile file, NamespaceDeclarationSyntax namespaceDeclartion, Dictionary<string, string> availableMixins)
        {
            foreach (var mixinClassModel in file.Classes.Where(x => availableMixins.Any(mixin => IsSameClassSignature(mixin.Key, x.RawName))))
            {
                var mixinName = availableMixins.First(mixin => IsSameClassSignature(mixin.Key, mixinClassModel.RawName)).Value;
                mixinName = RegexProvider.GenericParameterRegex.Replace(mixinName, "");
                var interfaceDeclaration = ClassGenerator.GenerateMixinInterface(mixinClassModel, mixinName);
                // Add class to namespace
                namespaceDeclartion = namespaceDeclartion.AddMembers(interfaceDeclaration);
            }

            return namespaceDeclartion;
        }


        /// <summary>
        /// Checks if two classes have the same signature.
        /// </summary>
        /// <param name="firstClass">First class to compare.</param>
        /// <param name="secondClass">Second class to compare.</param>
        /// <returns></returns>
        private static bool IsSameClassSignature(string firstClass, string secondClass)
        {
            var c1GenericParameters = RegexProvider.GenericParameterRegex.Match(firstClass);
            var c2GenericParameters = RegexProvider.GenericParameterRegex.Match(secondClass);
            var c1Normalized = RegexProvider.GenericParameterRegex.Replace(firstClass, "").ToLower();
            var c2Normalized = RegexProvider.GenericParameterRegex.Replace(secondClass, "").ToLower();

            // Check if name without generic parameters differ
            if (c1Normalized != c2Normalized)
                return false;

            //Check if generic parameters exist 
            if (!c1GenericParameters.Success && !c2GenericParameters.Success)
            {
                return true;
            }

            // Check if both classes contain generic parameters
            if(c1GenericParameters.Success && c2GenericParameters.Success)
            {
                //Check if generic parameter count is equal
                var c1GenericParameterCount = c1GenericParameters.Groups.Last().Value.Split(',').Length;
                var c2GenericParameterCount = c2GenericParameters.Groups.Last().Value.Split(',').Length;
                if (c1GenericParameterCount == c2GenericParameterCount)
                    return true;
            }

            return false;
        }
    }
}