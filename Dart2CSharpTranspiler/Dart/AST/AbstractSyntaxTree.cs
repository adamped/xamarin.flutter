using System.Collections.Generic;

namespace Dart2CSharpTranspiler.Dart
{

    public class SimpleToken
    {
        public SimpleTokenType type;
        public bool isKeyword;
        public string lexeme;
        public bool hasFailed; // This is a temporary hack. If you get here it means the Dart AST generation failed.
    }

    public class SimpleTokenType
    {
        public string name;
        public bool isKeyword;
    }

    public class SimpleCompilationUnit
    {
        public IEnumerable<SimpleToken> beginToken;
    }

    public class Token
    {
        public TokenType type { get; set; }
        public int offset { get; set; }
        public Token previous { get; set; }
        public Token next { get; set; }
        public int charCount { get; set; }
        public int charOffset { get; set; }
        public int charEnd { get; set; }
        public object beforeSynthetic { get; set; }
        public int end { get; set; }
        public object endGroup { get; set; }
        public bool isEof { get; set; }
        public bool isIdentifier { get; set; }
        public bool isKeyword { get; set; }
        public bool isKeywordOrIdentifier { get; set; }
        public bool isModifier { get; set; }
        public bool isOperator { get; set; }
        public bool isSynthetic { get; set; }
        public bool isTopLevelKeyword { get; set; }
        public bool isUserDefinableOperator { get; set; }
        public string keyword { get; set; }
        public int kind { get; set; }
        public int length { get; set; }
        public string lexeme { get; set; }
        public Token precedingComments { get; set; }
        public string stringValue { get; set; }
    }

    public class LineInfo
    {
        public List<int> lineStarts { get; set; }
        public int lineCount { get; set; }
    }
    
    public class BestType
    {
        public string name { get; set; }
        public string displayName { get; set; }
        public bool hasTypeParameterReferenceInBound { get; set; }
        public bool isBottom { get; set; }
        public bool isDartAsyncFuture { get; set; }
        public bool isDartAsyncFutureOr { get; set; }
        public bool isDartCoreFunction { get; set; }
        public bool isDartCoreNull { get; set; }
        public bool isDynamic { get; set; }
        public bool isObject { get; set; }
        public bool isUndefined { get; set; }
        public bool isVoid { get; set; }
    }

    public class ChildEntity
    {
        public int end { get; set; }
        public bool isSynthetic { get; set; }
        public int length { get; set; }
        public int offset { get; set; }
        public string parent { get; set; }
        public string root { get; set; }
        public object staticType { get; set; }
        public object bestParameterElement { get; set; }
        public BestType bestType { get; set; }
        public bool inConstantContext { get; set; }
        public bool isAssignable { get; set; }
        public object propagatedParameterElement { get; set; }
        public object propagatedType { get; set; }
        public object staticParameterElement { get; set; }
        public string unParenthesized { get; set; }
        public string period { get; set; }
        public string beginToken { get; set; }
        public object bestElement { get; set; }
        public List<object> childEntities { get; set; }
        public Token endToken { get; set; }
        public string identifier { get; set; }
        public bool isDeferred { get; set; }
        public string name { get; set; }
        public int precedence { get; set; }
        public string prefix { get; set; }
        public object propagatedElement { get; set; }
        public object staticElement { get; set; }
    }

    public class Reference
    {
        public int end { get; set; }
        public bool isSynthetic { get; set; }
        public int length { get; set; }
        public int offset { get; set; }
        public string parent { get; set; }
        public string root { get; set; }
        public object newKeyword { get; set; }
        public Token beginToken { get; set; }
        public List<ChildEntity> childEntities { get; set; }
        public Token endToken { get; set; }
        public string identifier { get; set; }
    }

    public class DocumentationComment
    {
        public int end { get; set; }
        public bool isSynthetic { get; set; }
        public int length { get; set; }
        public int offset { get; set; }
        public string parent { get; set; }
        public string root { get; set; }
        public List<string> tokens { get; set; }
        public string beginToken { get; set; }
        public List<string> childEntities { get; set; }
        public Token endToken { get; set; }
        public bool isBlock { get; set; }
        public bool isDocumentation { get; set; }
        public bool isEndOfLine { get; set; }
        public List<Reference> references { get; set; }
    }

    public class Name
    {
        public int end { get; set; }
        public bool isSynthetic { get; set; }
        public int length { get; set; }
        public int offset { get; set; }
        public string parent { get; set; }
        public string root { get; set; }
        public object staticType { get; set; }
        public object bestParameterElement { get; set; }
        public string bestType { get; set; }
        public bool inConstantContext { get; set; }
        public bool isAssignable { get; set; }
        public object propagatedParameterElement { get; set; }
        public object propagatedType { get; set; }
        public object staticParameterElement { get; set; }
        public string unParenthesized { get; set; }
        public string token { get; set; }
        public object auxiliaryElements { get; set; }
        public string beginToken { get; set; }
        public object bestElement { get; set; }
        public List<string> childEntities { get; set; }
        public Token endToken { get; set; }
        public bool isQualified { get; set; }
        public string name { get; set; }
        public int precedence { get; set; }
        public object propagatedElement { get; set; }
        public object staticElement { get; set; }
    }

    public class Parent
    {
        public int end { get; set; }
        public bool isSynthetic { get; set; }
        public int length { get; set; }
        public int offset { get; set; }
        public Parent parent { get; set; }
        public string root { get; set; }
        public string leftParenthesis { get; set; }
        public object leftDelimiter { get; set; }
        public object rightDelimiter { get; set; }
        public string rightParenthesis { get; set; }
        public string beginToken { get; set; }
        public List<string> childEntities { get; set; }
        public Token endToken { get; set; }
        public List<object> parameterElements { get; set; }
        public List<string> parameters { get; set; }
    }

    public class Identifier
    {
        public int end { get; set; }
        public bool isSynthetic { get; set; }
        public int length { get; set; }
        public int offset { get; set; }
        public string parent { get; set; }
        public string root { get; set; }
        public object staticType { get; set; }
        public object bestParameterElement { get; set; }
        public string bestType { get; set; }
        public bool inConstantContext { get; set; }
        public bool isAssignable { get; set; }
        public object propagatedParameterElement { get; set; }
        public object propagatedType { get; set; }
        public object staticParameterElement { get; set; }
        public string unParenthesized { get; set; }
        public string token { get; set; }
        public object auxiliaryElements { get; set; }
        public string beginToken { get; set; }
        public object bestElement { get; set; }
        public List<string> childEntities { get; set; }
        public Token endToken { get; set; }
        public bool isQualified { get; set; }
        public string name { get; set; }
        public int precedence { get; set; }
        public object propagatedElement { get; set; }
        public object staticElement { get; set; }
    }

    public class Kind
    {
        public string name { get; set; }
        public int ordinal { get; set; }
        public bool isOptional { get; set; }
    }
    
    public class ImplementsClause
    {
        public int end { get; set; }
        public bool isSynthetic { get; set; }
        public int length { get; set; }
        public int offset { get; set; }
        public string parent { get; set; }
        public string root { get; set; }
        public string implementsKeyword { get; set; }
        public string beginToken { get; set; }
        public List<object> childEntities { get; set; }
        public Token endToken { get; set; }
        public List<string> interfaces { get; set; }
    }
    
    public class CompilationUnit
    {
        public int end { get; set; }
        public bool isSynthetic { get; set; }
        public int length { get; set; }
        public int offset { get; set; }
        public object parent { get; set; }
        public object root { get; set; }
        public Token beginToken { get; set; }
        public Token endToken { get; set; }
        public object declaredElement { get; set; }
        public LineInfo lineInfo { get; set; }
        //public LocalDeclarations localDeclarations { get; set; }
        public List<object> childEntities { get; set; }
        public List<object> declarations { get; set; }
        public List<object> directives { get; set; }
        public object element { get; set; }
        public object scriptTag { get; set; }
        public List<object> sortedDirectivesAndDeclarations { get; set; }
    }

}
