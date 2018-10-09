using System;
using System.Collections.Generic;
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
        public static ClassDeclarationSyntax AddSections(DartClass classModel, ClassDeclarationSyntax classDeclaration)
        {
            var declarations = new List<MemberDeclarationSyntax>();
            foreach (var section in classModel.Sections)
            {
                var returnType = section.ReturnType == null ? string.Empty : NormalizationHelper.NormalizeTypeName(section.ReturnType);
                var name = NormalizationHelper.CamelCase(section.Name);

                switch (section.Type)
                {
                    case SectionType.PropertyGet:
                        break;
                    case SectionType.PropertySet:
                        break;
                    case SectionType.Method: 
                        var syntax = SyntaxFactory.ParseStatement("throw new NotImplementedException();");
                        var methodDeclaration = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName(returnType), name)
                            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                            .WithBody(SyntaxFactory.Block(syntax));
                        declarations.Add(methodDeclaration);
                        break;
                    case SectionType.Constructor:
                        break;
                    case SectionType.Field:
                        var variableDeclaration = SyntaxFactory.VariableDeclaration(SyntaxFactory.ParseTypeName(returnType))
                            .AddVariables(SyntaxFactory.VariableDeclarator(name));
                        var fieldDeclaration = SyntaxFactory.FieldDeclaration(variableDeclaration)
                            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));

                        declarations.Add(fieldDeclaration);
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
    }
}