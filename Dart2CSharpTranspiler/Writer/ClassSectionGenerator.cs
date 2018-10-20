using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Transpiler;

namespace Dart2CSharpTranspiler.Writer
{
    /// <summary>
    /// Generator for sections of a class.
    /// </summary>
    public static class ClassSectionGenerator
    {
        /// <summary>
        /// Adds all sections from the <paramref name="classModel"/> to the <paramref name="classDeclaration"/>.
        /// </summary>
        /// <param name="classModel">Model of the class.</param>
        /// <param name="classDeclaration">Declaration of the class the sections will get added to.</param> 
        public static ClassDeclarationSyntax AddSections(DartClass classModel, ClassDeclarationSyntax classDeclaration, NamespaceDeclarationSyntax namespaceDeclaration)
        {
            var declarations = new List<MemberDeclarationSyntax>();
            foreach (var section in classModel.Sections)
            {
                var returnType = section.ReturnType == null ? string.Empty : NormalizationHelper.NormalizeTypeName(section.ReturnType);

                var rewriteRule = RewriteRuleProvider.FindRuleForClass(returnType);
                if (rewriteRule != null)
                {
                    returnType = rewriteRule.ReplacementClassName;
                    namespaceDeclaration = rewriteRule.ApplyToNamespace(namespaceDeclaration);
                }

                switch (section.Type)
                {
                    case SectionType.PropertyGet:
                        break;
                    case SectionType.PropertySet:
                        break;
                    case SectionType.Method:
                        AddMethod(declarations, section, returnType);
                        break;
                    case SectionType.Constructor:
                        break;
                    case SectionType.Field:
                        AddField(declarations, section, returnType);
                        break;
                    case SectionType.Operator:
                        break;
                    case SectionType.Typedef:
                        break;
                    case SectionType.Enum:
                        break;
                    case SectionType.Export:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }
            classDeclaration = classDeclaration.AddMembers(declarations.ToArray());
            return classDeclaration;
        }

        private static void AddMethod(List<MemberDeclarationSyntax> declarations, Section section, string returnType)
        { 
            var name = NormalizationHelper.NormalizeTypeName(section.Name);
            var syntax = SyntaxFactory.ParseStatement("throw new NotImplementedException();");
            var methodDeclaration = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName(returnType), name)
                .AddModifiers(GetModifiers(section))
                .WithBody(SyntaxFactory.Block(syntax));
            declarations.Add(methodDeclaration);
        }

        private static void AddField(List<MemberDeclarationSyntax> declarations, Section section, string returnType)
        {
            var name = NormalizationHelper.PrivateField(section.Name);
            var variableDeclaration = SyntaxFactory.VariableDeclaration(SyntaxFactory.ParseTypeName(returnType))
                .AddVariables(SyntaxFactory.VariableDeclarator(name));
            var fieldDeclaration = SyntaxFactory.FieldDeclaration(variableDeclaration)
                .AddModifiers(GetModifiers(section));

            declarations.Add(fieldDeclaration);
        }

        private static SyntaxToken[] GetModifiers(Section section)
        {
            var modifiers = new List<SyntaxKind>();
            switch (section.VisibilityType)
            {
                case VisibilityType.Public:
                    modifiers.Add(SyntaxKind.PublicKeyword);
                    break;
                case VisibilityType.Private:
                    modifiers.Add(SyntaxKind.PrivateKeyword);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (section.IsAbstract)
                modifiers.Add(SyntaxKind.AbstractKeyword);
            if (section.IsConstant)
                modifiers.Add(SyntaxKind.ConstKeyword);
            if (section.IsFinal)
            {
                //TODO Handle final keyword
            }
            if(section.IsOverride)
                modifiers.Add(SyntaxKind.OverrideKeyword);
            if(section.IsStatic)
                modifiers.Add(SyntaxKind.StaticKeyword);
            if(section.IsVirtual)
                modifiers.Add(SyntaxKind.VirtualKeyword);

            return modifiers.Select(SyntaxFactory.Token).ToArray();
        } 
    }
}