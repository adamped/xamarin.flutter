using System;
using System.Collections.Generic;
using static Dart2CSharpTranspiler.Dart.Constants;

namespace Dart2CSharpTranspiler.Dart
{
    public class Keyword : TokenType
    {
        public static Keyword ABSTRACT = new Keyword("abstract", "ABSTRACT", isBuiltIn: true, isModifier: true);

        public static Keyword AS = new Keyword("as", "AS", precedence: RELATIONAL_PRECEDENCE, isBuiltIn: true);

        public static Keyword ASSERT = new Keyword("assert", "ASSERT");

        public static Keyword ASYNC = new Keyword("async", "ASYNC", isPseudo: true);

        public static Keyword AWAIT = new Keyword("await", "AWAIT", isPseudo: true);

        public static Keyword BREAK = new Keyword("break", "BREAK");

        public static Keyword CASE = new Keyword("case", "CASE");

        public static Keyword CATCH = new Keyword("catch", "CATCH");

        public static Keyword CLASS = new Keyword("class", "CLASS", isTopLevelKeyword: true);

        public static Keyword CONST = new Keyword("const", "CONST", isModifier: true);

        public static Keyword CONTINUE = new Keyword("continue", "CONTINUE");

        public static Keyword COVARIANT = new Keyword("covariant", "COVARIANT", isBuiltIn: true, isModifier: true);

        public static Keyword DEFAULT = new Keyword("default", "DEFAULT");

        public static Keyword DEFERRED =
          new Keyword("deferred", "DEFERRED", isBuiltIn: true);

        public static Keyword DO = new Keyword("do", "DO");

        public static Keyword DYNAMIC =
          new Keyword("dynamic", "DYNAMIC", isBuiltIn: true);

        public static Keyword ELSE = new Keyword("else", "ELSE");

        public static Keyword ENUM =
          new Keyword("enum", "ENUM", isTopLevelKeyword: true);

        public static Keyword EXPORT = new Keyword("export", "EXPORT",
            isBuiltIn: true, isTopLevelKeyword: true);

        public static Keyword EXTENDS = new Keyword("extends", "EXTENDS");

        public static Keyword EXTERNAL =
          new Keyword("external", "EXTERNAL", isBuiltIn: true, isModifier: true);

        public static Keyword FACTORY =
          new Keyword("factory", "FACTORY", isBuiltIn: true);

        public static Keyword FALSE = new Keyword("false", "FALSE");

        public static Keyword FINAL = new Keyword("final", "FINAL", isModifier: true);

        public static Keyword FINALLY = new Keyword("finally", "FINALLY");

        public static Keyword FOR = new Keyword("for", "FOR");

        public static Keyword FUNCTION = new Keyword("Function", "FUNCTION", isPseudo: true);

        public static Keyword GET = new Keyword("get", "GET", isBuiltIn: true);

        public static Keyword HIDE = new Keyword("hide", "HIDE", isPseudo: true);

        public static Keyword IF = new Keyword("if", "IF");

        public static Keyword IMPLEMENTS = new Keyword("implements", "IMPLEMENTS", isBuiltIn: true);

        public static Keyword IMPORT = new Keyword("import", "IMPORT", isBuiltIn: true, isTopLevelKeyword: true);

        public static Keyword IN = new Keyword("in", "IN");

        public static Keyword INTERFACE = new Keyword("interface", "INTERFACE", isBuiltIn: true);

        public static Keyword IS = new Keyword("is", "IS", precedence: RELATIONAL_PRECEDENCE);

        public static Keyword LIBRARY = new Keyword("library", "LIBRARY", isBuiltIn: true, isTopLevelKeyword: true);

        public static Keyword MIXIN = new Keyword("mixin", "MIXIN", isBuiltIn: true, isTopLevelKeyword: true);

        public static Keyword NATIVE = new Keyword("native", "NATIVE", isPseudo: true);

        public static Keyword NEW = new Keyword("new", "NEW");

        public static Keyword NULL = new Keyword("null", "NULL");

        public static Keyword OF = new Keyword("of", "OF", isPseudo: true);

        public static Keyword ON = new Keyword("on", "ON", isPseudo: true);

        public static Keyword OPERATOR = new Keyword("operator", "OPERATOR", isBuiltIn: true);

        public static Keyword PART = new Keyword("part", "PART", isBuiltIn: true, isTopLevelKeyword: true);

        public static Keyword PATCH = new Keyword("patch", "PATCH", isPseudo: true);

        public static Keyword RETHROW = new Keyword("rethrow", "RETHROW");

        public static Keyword RETURN = new Keyword("return", "RETURN");

        public static Keyword SET = new Keyword("set", "SET", isBuiltIn: true);

        public static Keyword SHOW = new Keyword("show", "SHOW", isPseudo: true);

        public static Keyword SOURCE = new Keyword("source", "SOURCE", isPseudo: true);

        public static Keyword STATIC = new Keyword("static", "STATIC", isBuiltIn: true, isModifier: true);

        public static Keyword SUPER = new Keyword("super", "SUPER");

        public static Keyword SWITCH = new Keyword("switch", "SWITCH");

        public static Keyword SYNC = new Keyword("sync", "SYNC", isPseudo: true);

        public static Keyword THIS = new Keyword("this", "THIS");

        public static Keyword THROW = new Keyword("throw", "THROW");

        public static Keyword TRUE = new Keyword("true", "TRUE");

        public static Keyword TRY = new Keyword("try", "TRY");

        public static Keyword TYPEDEF = new Keyword("typedef", "TYPEDEF", isBuiltIn: true, isTopLevelKeyword: true);

        public static Keyword VAR = new Keyword("var", "VAR", isModifier: true);

        public static Keyword VOID = new Keyword("void", "VOID");

        public static Keyword WHILE = new Keyword("while", "WHILE");

        public static Keyword WITH = new Keyword("with", "WITH");

        public static Keyword YIELD = new Keyword("yield", "YIELD", isPseudo: true);

        public static List<Keyword> values = new List<Keyword>{
          ABSTRACT,
          AS,
          ASSERT,
          ASYNC,
          AWAIT,
          BREAK,
          CASE,
          CATCH,
          CLASS,
          CONST,
          CONTINUE,
          COVARIANT,
          DEFAULT,
          DEFERRED,
          DO,
          DYNAMIC,
          ELSE,
          ENUM,
          EXPORT,
          EXTENDS,
          EXTERNAL,
          FACTORY,
          FALSE,
          FINAL,
          FINALLY,
          FOR,
          FUNCTION,
          GET,
          HIDE,
          IF,
          IMPLEMENTS,
          IMPORT,
          IN,
          INTERFACE,
          IS,
          LIBRARY,
          MIXIN,
          NATIVE,
          NEW,
          NULL,
          OF,
          ON,
          OPERATOR,
          PART,
          PATCH,
          RETHROW,
          RETURN,
          SET,
          SHOW,
          SOURCE,
          STATIC,
          SUPER,
          SWITCH,
          SYNC,
          THIS,
          THROW,
          TRUE,
          TRY,
          TYPEDEF,
          VAR,
          VOID,
          WHILE,
          WITH,
          YIELD,
        };

        /**
         * A table mapping the lexemes of keywords to the corresponding keyword.
         */
        public static readonly Dictionary<String, Keyword> keywords = _createKeywordMap();

        /**
         * Initialize a newly created keyword.
         */
        public Keyword(String lexeme,
                       String name,
                       bool isBuiltIn = false,
                       bool isModifier = false,
                       bool isPseudo = false,
                       bool isTopLevelKeyword = false,
                       int precedence = NO_PRECEDENCE)
              : base(lexeme, name, precedence, KEYWORD_TOKEN,
                    isModifier: isModifier, isTopLevelKeyword: isTopLevelKeyword)
        {
        }

        public bool isBuiltInOrPseudo => isBuiltIn || isPseudo;
                 

        /**
         * Create a table mapping the lexemes of keywords to the corresponding keyword
         * and return the table that was created.
         */
        static Dictionary<String, Keyword> _createKeywordMap()
        {
            Dictionary<String, Keyword> result =
                new Dictionary<String, Keyword>();
            foreach (Keyword keyword in values)
            {
                result[keyword.lexeme] = keyword;
            }
            return result;
        }
    }
}
