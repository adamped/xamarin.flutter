using System;
using System.Collections.Generic;
using System.Linq;
using Dart2CSharpTranspiler.Dart;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Transpiler;

namespace Dart2CSharpTranspiler.Writer
{
    /// <summary>
    /// Generates the content of a class.
    /// </summary>
    public class FileProcessor
    {
        private readonly DartFile _file;
        private NamespaceDeclarationSyntax _namespaceDeclartion;
        private readonly Dictionary<string, string> _availableMixins;

        public FileProcessor(DartFile file, NamespaceDeclarationSyntax namespaceDeclartion, Dictionary<string, string> availableMixins)
        {
            _file = file;
            _namespaceDeclartion = namespaceDeclartion;
            _availableMixins = availableMixins;
        }

        /// <summary>
        /// Adds all classes of a <see cref="DartFile"/> to the <see cref="NamespaceDeclarationSyntax"/>.
        /// </summary>
        public NamespaceDeclarationSyntax AddFileInfoToNamespace()
        {
            // Add classes
            foreach (var classModel in _file.Classes)
            {
                var classDeclaration = GenerateClass(classModel, _availableMixins); 
                _namespaceDeclartion = _namespaceDeclartion.AddMembers(classDeclaration);
            }

            // Add enums
            foreach (var section in _file.Sections.Where(x => x.Type == SectionType.Enum))
            {
                _namespaceDeclartion = _namespaceDeclartion.AddMembers(GenerateEnum(section));
            } 

            AddMixinInterfaces();

            return _namespaceDeclartion;
        }

        private EnumDeclarationSyntax GenerateEnum(Section section)
        {
            // Find members
            var values = section.RawCode.Split(',', StringSplitOptions.RemoveEmptyEntries);
            var members = new List<EnumMemberDeclarationSyntax>();
            foreach (var value in values)
            {
                var member = NormalizationHelper.CamelCase(value.Trim());
                members.Add(SyntaxFactory.EnumMemberDeclaration(member));
            }

            // Create normalized name 
            var name = NormalizationHelper.NormalizeTypeName(section.Name);
             
            // Create enum
            return SyntaxFactory.EnumDeclaration(name)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddMembers(members.ToArray());
        }

        /// <summary>
        /// Generates an interface that abstracts a dart mixin.
        /// </summary>
        /// <param name="classModel"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private InterfaceDeclarationSyntax GenerateMixinInterface(DartClass classModel, string name)
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
        private ClassDeclarationSyntax GenerateClass(DartClass classModel, Dictionary<string, string> availableMixins)
        {
            var className = classModel.Name;

            // find modifiers
            var modifiers = new List<SyntaxKind>
            {
                SyntaxKind.PublicKeyword
            };
            if (classModel.IsAbstract)
                modifiers.Add(SyntaxKind.AbstractKeyword); 
            if (classModel.IsImmutable)
                modifiers.Add(SyntaxKind.AbstractKeyword); 
            
            var classDeclaration = SyntaxFactory.ClassDeclaration(NormalizationHelper.NormalizeTypeName(className))
                .AddModifiers(modifiers.Select(SyntaxFactory.Token).ToArray()); 

            classDeclaration = (ClassDeclarationSyntax)AddConstraints(classModel, classDeclaration); 
            classDeclaration = AddBaseTypes(classModel, className, classDeclaration, availableMixins);
            classDeclaration = ClassSectionGenerator.AddSections(classModel, classDeclaration, _namespaceDeclartion);
            return classDeclaration;
        }

        /// <summary>
        /// Adds base types to a class.
        /// </summary>
        /// <param name="classModel">The model for the class.</param>
        /// <param name="className">The name for the class.</param>
        /// <param name="classDeclaration">The <see cref="ClassDeclarationSyntax"/> the base types will be added to.</param>
        /// <param name="availableMixins">All known mixins of the library.</param> 
        private ClassDeclarationSyntax AddBaseTypes(DartClass classModel, string className, ClassDeclarationSyntax classDeclaration, Dictionary<string, string> availableMixins)
        {
            if (classModel.Extends != null)
            {
                // Add base classes
                if (!string.IsNullOrEmpty(classModel.Extends.Name))
                {
                    var name = NormalizationHelper.NormalizeTypeName(classModel.Extends.Name); 
                    name = RewriteBaseClass(name); 
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
        /// Rewrites a base class if a rewriting rule exists.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string RewriteBaseClass(string name)
        {
            var rewriteRule = RewriteRuleProvider.FindRuleForClass(name);

            if (rewriteRule != null)
            {
                _namespaceDeclartion = rewriteRule.ApplyToNamespace(_namespaceDeclartion); 
                // Overwrite name with rule
                name = rewriteRule.ReplacementClassName;
            }

            return name;
        }

        /// <summary>
        /// Adds constraints to a <see cref="TypeDeclarationSyntax"/>.
        /// </summary>
        public TypeDeclarationSyntax AddConstraints(DartClass classModel, TypeDeclarationSyntax classDeclaration)
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
        public NamespaceDeclarationSyntax AddMixinInterfaces()
        {
            foreach (var mixinClassModel in _file.Classes.Where(x =>_availableMixins.Any(mixin => IsSameClassSignature(mixin.Key, x.RawName))))
            {
                var mixinName = _availableMixins.First(mixin => IsSameClassSignature(mixin.Key, mixinClassModel.RawName)).Value;
                mixinName = RegexProvider.GenericParameterRegex.Replace(mixinName, "");
                var interfaceDeclaration = GenerateMixinInterface(mixinClassModel, mixinName);
                // Add class to namespace
                _namespaceDeclartion = _namespaceDeclartion.AddMembers(interfaceDeclaration);
            }

            return _namespaceDeclartion;
        }


        /// <summary>
        /// Checks if two classes have the same signature.
        /// </summary>
        /// <param name="firstClass">First class to compare.</param>
        /// <param name="secondClass">Second class to compare.</param>
        /// <returns></returns>
        private bool IsSameClassSignature(string firstClass, string secondClass)
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