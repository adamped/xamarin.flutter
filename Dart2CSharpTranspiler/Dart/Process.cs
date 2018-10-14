using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;

namespace Dart2CSharpTranspiler.Dart
{
    public static class Process
    {

        public static DartFile CreateFile(string name, string folder, SimpleCompilationUnit ast)
        {

            var file = new DartFile()
            {
                Name = name,
                Folder = folder,
                Classes = new List<DartClass>(),
                Imports = new List<DartImport>(),
                Sections = new List<Section>()
            };
                       
            DartImport import = null;
            DartClass @class = null;

            var raw = "";

            IList<SimpleToken> _previousTokens = new List<SimpleToken>();

            foreach (var token in ast.beginToken)
            {
                raw += $"{token.lexeme} ";
                if (token.type.isKeyword && import == null && @class == null)
                {
                    if (IsKeyword(token.type, Keyword.IMPORT))
                    {
                        import = new DartImport();
                    }
                    else if (IsKeyword(token.type, Keyword.CLASS))
                    {
                        @class = new DartClass();

                        if (_previousTokens.Count > 0)
                        {
                            foreach (var t in _previousTokens)
                                if (IsKeyword(token.type, Keyword.ABSTRACT))
                                    @class.IsAbstract = true;
                        }

                        _previousTokens.Clear();

                    }
                    else
                        _previousTokens.Add(token); // If we don't know what to do with them yet, just add them.


                }
                else
                {
                    // Currently building import
                    if (import != null)
                    {
                        if (IsType(token.type, TokenType.STRING, TokenType.IDENTIFIER))
                        {
                            if (import.HasScoped)
                            {
                                import.ScopedVariables.Add(token.lexeme.CleanEnclosingString());
                            }
                            else
                            {
                                import.Name = token.lexeme.CleanEnclosingString();
                                if (import.Name.StartsWith("package"))
                                    import.Type = ImportType.Package;
                                else if (import.Name.StartsWith("dart"))
                                    import.Type = ImportType.Dart;
                                else
                                    import.Type = ImportType.File;
                            }
                        }
                        else if (token.type.name == Keyword.SHOW.name)
                        {
                            import.HasScoped = true;
                        }
                        else if (token.type.name == TokenType.SEMICOLON.name)
                        {
                            import.Raw = raw;
                            raw = string.Empty;
                            file.Imports.Add(import);
                            import = null;
                        }
                    }

                    // Currently building class
                    if (@class != null)
                    {
                        if (IsType(token.type, TokenType.STRING, TokenType.IDENTIFIER))
                        {
                            @class.Name = token.lexeme;
                        }
                        else if (IsType(token.type, TokenType.LT))
                        {

                        }
                    }
                }
            }

            return file;

        }



        static bool IsKeyword(SimpleTokenType type, Keyword keyword)
        => type.name == keyword.name;

        static bool IsType(SimpleTokenType type, params TokenType[] keywords)
        {
            foreach (var keyword in keywords)
                if (type.name == keyword.name)
                    return true;

            return false;
        }

        static string CleanEnclosingString(this string value)
            => value.TrimStart('\'').TrimEnd('\'');

    }

}