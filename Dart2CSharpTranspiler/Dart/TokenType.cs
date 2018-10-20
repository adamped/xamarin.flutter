using System;
using System.Collections.Generic;
using static Dart2CSharpTranspiler.Dart.Constants;

namespace Dart2CSharpTranspiler.Dart
{
    public class TokenType
    {
        public const int NO_PRECEDENCE = 0;
        public const int ASSIGNMENT_PRECEDENCE = 1;
        public const int CASCADE_PRECEDENCE = 2;
        public const int CONDITIONAL_PRECEDENCE = 3;
        public const int IF_NULL_PRECEDENCE = 4;
        public const int LOGICAL_OR_PRECEDENCE = 5;
        public const int LOGICAL_AND_PRECEDENCE = 6;
        public const int EQUALITY_PRECEDENCE = 7;
        public const int RELATIONAL_PRECEDENCE = 8;
        public const int BITWISE_OR_PRECEDENCE = 9;
        public const int BITWISE_XOR_PRECEDENCE = 10;
        public const int BITWISE_AND_PRECEDENCE = 11;
        public const int SHIFT_PRECEDENCE = 12;
        public const int ADDITIVE_PRECEDENCE = 13;
        public const int MULTIPLICATIVE_PRECEDENCE = 14;
        public const int PREFIX_PRECEDENCE = 15;
        public const int POSTFIX_PRECEDENCE = 16;
        public const int SELECTOR_PRECEDENCE = 17;



        /**
         * The type of the token that marks the start or end of the input.
         */
        public static TokenType EOF =
          new TokenType("", "EOF", NO_PRECEDENCE, EOF_TOKEN);

        public static TokenType DOUBLE = new TokenType(
      "double", "DOUBLE", NO_PRECEDENCE, DOUBLE_TOKEN,
            stringValue: null);

        public static TokenType HEXADECIMAL = new TokenType(
      "hexadecimal", "HEXADECIMAL", NO_PRECEDENCE, HEXADECIMAL_TOKEN,
            stringValue: null);

        public static TokenType IDENTIFIER = new TokenType(
      "identifier", "STRING_INT", NO_PRECEDENCE, IDENTIFIER_TOKEN,
            stringValue: null);

        public static TokenType INT = new TokenType(
      "int", "INT", NO_PRECEDENCE, INT_TOKEN,
            stringValue: null);

        public static TokenType MULTI_LINE_COMMENT = new TokenType(
      "comment", "MULTI_LINE_COMMENT", NO_PRECEDENCE, COMMENT_TOKEN,
            stringValue: null);

        public static TokenType SCRIPT_TAG =
          new TokenType("script", "SCRIPT_TAG", NO_PRECEDENCE, SCRIPT_TOKEN);

        public static TokenType SINGLE_LINE_COMMENT = new TokenType(
      "comment", "SINGLE_LINE_COMMENT", NO_PRECEDENCE, COMMENT_TOKEN,
            stringValue: null);

        public static TokenType STRING = new TokenType(
      "string", "STRING", NO_PRECEDENCE, STRING_TOKEN,
            stringValue: null);

        public static TokenType AMPERSAND = new TokenType(
      "&", "AMPERSAND", BITWISE_AND_PRECEDENCE, AMPERSAND_TOKEN,
            isOperator: true, isUserDefinableOperator: true);

        public static TokenType AMPERSAND_AMPERSAND = new TokenType("&&",
            "AMPERSAND_AMPERSAND", LOGICAL_AND_PRECEDENCE, AMPERSAND_AMPERSAND_TOKEN,
            isOperator: true);

        // This is not yet part of the language and not supported by fasta
        public static TokenType AMPERSAND_AMPERSAND_EQ = new TokenType(
      "&&=",
            "AMPERSAND_AMPERSAND_EQ",
            ASSIGNMENT_PRECEDENCE,
            AMPERSAND_AMPERSAND_EQ_TOKEN,
            isOperator: true);

        public static TokenType AMPERSAND_EQ = new TokenType(
      "&=", "AMPERSAND_EQ", ASSIGNMENT_PRECEDENCE, AMPERSAND_EQ_TOKEN,
            isOperator: true);

        public static TokenType AT =
          new TokenType("@", "AT", NO_PRECEDENCE, AT_TOKEN);

        public static TokenType BANG = new TokenType(
      "!", "BANG", PREFIX_PRECEDENCE, BANG_TOKEN,
            isOperator: true);

        public static TokenType BANG_EQ = new TokenType(
      "!=", "BANG_EQ", EQUALITY_PRECEDENCE, BANG_EQ_TOKEN,
            isOperator: true);

        public static TokenType BANG_EQ_EQ = new TokenType(
      "!==", "BANG_EQ_EQ", EQUALITY_PRECEDENCE, BANG_EQ_EQ_TOKEN);

        public static TokenType BAR = new TokenType(
      "|", "BAR", BITWISE_OR_PRECEDENCE, BAR_TOKEN,
            isOperator: true, isUserDefinableOperator: true);

        public static TokenType BAR_BAR = new TokenType(
      "||", "BAR_BAR", LOGICAL_OR_PRECEDENCE, BAR_BAR_TOKEN,
            isOperator: true);

        // This is not yet part of the language and not supported by fasta
        public static TokenType BAR_BAR_EQ = new TokenType(
      "||=", "BAR_BAR_EQ", ASSIGNMENT_PRECEDENCE, BAR_BAR_EQ_TOKEN,
            isOperator: true);

        public static TokenType BAR_EQ = new TokenType(
      "|=", "BAR_EQ", ASSIGNMENT_PRECEDENCE, BAR_EQ_TOKEN,
            isOperator: true);

        public static TokenType COLON =
          new TokenType(":", "COLON", NO_PRECEDENCE, COLON_TOKEN);

        public static TokenType COMMA =
          new TokenType(",", "COMMA", NO_PRECEDENCE, COMMA_TOKEN);

        public static TokenType CARET = new TokenType(
      "^", "CARET", BITWISE_XOR_PRECEDENCE, CARET_TOKEN,
            isOperator: true, isUserDefinableOperator: true);

        public static TokenType CARET_EQ = new TokenType(
      "^=", "CARET_EQ", ASSIGNMENT_PRECEDENCE, CARET_EQ_TOKEN,
            isOperator: true);

        public static TokenType CLOSE_CURLY_BRACKET = new TokenType(
      "}", "CLOSE_CURLY_BRACKET", NO_PRECEDENCE, CLOSE_CURLY_BRACKET_TOKEN);

        public static TokenType CLOSE_PAREN =
          new TokenType(")", "CLOSE_PAREN", NO_PRECEDENCE, CLOSE_PAREN_TOKEN);

        public static TokenType CLOSE_SQUARE_BRACKET = new TokenType(
      "]", "CLOSE_SQUARE_BRACKET", NO_PRECEDENCE, CLOSE_SQUARE_BRACKET_TOKEN);

        public static TokenType EQ = new TokenType(
      "=", "EQ", ASSIGNMENT_PRECEDENCE, EQ_TOKEN,
            isOperator: true);

        public static TokenType EQ_EQ = new TokenType(
      "==", "EQ_EQ", EQUALITY_PRECEDENCE, EQ_EQ_TOKEN,
            isOperator: true, isUserDefinableOperator: true);

        /// The `===` operator is not supported in the Dart language
        /// but is parsed as such by the scanner to support better recovery
        /// when a JavaScript code snippet is pasted into a Dart file.
        public static TokenType EQ_EQ_EQ =
          new TokenType("===", "EQ_EQ_EQ", EQUALITY_PRECEDENCE, EQ_EQ_EQ_TOKEN);

        public static TokenType FUNCTION =
          new TokenType("=>", "FUNCTION", NO_PRECEDENCE, FUNCTION_TOKEN);

        public static TokenType GT = new TokenType(
      ">", "GT", RELATIONAL_PRECEDENCE, GT_TOKEN,
            isOperator: true, isUserDefinableOperator: true);

        public static TokenType GT_EQ = new TokenType(
      ">=", "GT_EQ", RELATIONAL_PRECEDENCE, GT_EQ_TOKEN,
            isOperator: true, isUserDefinableOperator: true);

        public static TokenType GT_GT = new TokenType(
      ">>", "GT_GT", SHIFT_PRECEDENCE, GT_GT_TOKEN,
            isOperator: true, isUserDefinableOperator: true);

        public static TokenType GT_GT_EQ = new TokenType(
      ">>=", "GT_GT_EQ", ASSIGNMENT_PRECEDENCE, GT_GT_EQ_TOKEN,
            isOperator: true);

        public static TokenType HASH =
          new TokenType("#", "HASH", NO_PRECEDENCE, HASH_TOKEN);

        public static TokenType INDEX = new TokenType(
      "[]", "INDEX", SELECTOR_PRECEDENCE, INDEX_TOKEN,
            isOperator: true, isUserDefinableOperator: true);

        public static TokenType INDEX_EQ = new TokenType(
      "[]=", "INDEX_EQ", NO_PRECEDENCE, INDEX_EQ_TOKEN,
            isOperator: true, isUserDefinableOperator: true);

        public static TokenType LT = new TokenType(
      "<", "LT", RELATIONAL_PRECEDENCE, LT_TOKEN,
            isOperator: true, isUserDefinableOperator: true);

        public static TokenType LT_EQ = new TokenType(
      "<=", "LT_EQ", RELATIONAL_PRECEDENCE, LT_EQ_TOKEN,
            isOperator: true, isUserDefinableOperator: true);

        public static TokenType LT_LT = new TokenType(
      "<<", "LT_LT", SHIFT_PRECEDENCE, LT_LT_TOKEN,
            isOperator: true, isUserDefinableOperator: true);

        public static TokenType LT_LT_EQ = new TokenType(
      "<<=", "LT_LT_EQ", ASSIGNMENT_PRECEDENCE, LT_LT_EQ_TOKEN,
            isOperator: true);

        public static TokenType MINUS = new TokenType(
      "-", "MINUS", ADDITIVE_PRECEDENCE, MINUS_TOKEN,
            isOperator: true, isUserDefinableOperator: true);

        public static TokenType MINUS_EQ = new TokenType(
      "-=", "MINUS_EQ", ASSIGNMENT_PRECEDENCE, MINUS_EQ_TOKEN,
            isOperator: true);

        public static TokenType MINUS_MINUS = new TokenType(
      "--", "MINUS_MINUS", POSTFIX_PRECEDENCE, MINUS_MINUS_TOKEN,
            isOperator: true);

        public static TokenType OPEN_CURLY_BRACKET = new TokenType(
      "{", "OPEN_CURLY_BRACKET", NO_PRECEDENCE, OPEN_CURLY_BRACKET_TOKEN);

        public static TokenType OPEN_PAREN =
          new TokenType("(", "OPEN_PAREN", SELECTOR_PRECEDENCE, OPEN_PAREN_TOKEN);

        public static TokenType OPEN_SQUARE_BRACKET = new TokenType("[",
            "OPEN_SQUARE_BRACKET", SELECTOR_PRECEDENCE, OPEN_SQUARE_BRACKET_TOKEN);

        public static TokenType PERCENT = new TokenType(
      "%", "PERCENT", MULTIPLICATIVE_PRECEDENCE, PERCENT_TOKEN,
            isOperator: true, isUserDefinableOperator: true);

        public static TokenType PERCENT_EQ = new TokenType(
      "%=", "PERCENT_EQ", ASSIGNMENT_PRECEDENCE, PERCENT_EQ_TOKEN,
            isOperator: true);

        public static TokenType PERIOD =
          new TokenType(".", "PERIOD", SELECTOR_PRECEDENCE, PERIOD_TOKEN);

        public static TokenType PERIOD_PERIOD = new TokenType(
      "..", "PERIOD_PERIOD", CASCADE_PRECEDENCE, PERIOD_PERIOD_TOKEN,
            isOperator: true);

        public static TokenType PLUS = new TokenType(
      "+", "PLUS", ADDITIVE_PRECEDENCE, PLUS_TOKEN,
            isOperator: true, isUserDefinableOperator: true);

        public static TokenType PLUS_EQ = new TokenType(
      "+=", "PLUS_EQ", ASSIGNMENT_PRECEDENCE, PLUS_EQ_TOKEN,
            isOperator: true);

        public static TokenType PLUS_PLUS = new TokenType(
      "++", "PLUS_PLUS", POSTFIX_PRECEDENCE, PLUS_PLUS_TOKEN,
            isOperator: true);

        public static TokenType QUESTION = new TokenType(
      "?", "QUESTION", CONDITIONAL_PRECEDENCE, QUESTION_TOKEN,
            isOperator: true);

        public static TokenType QUESTION_PERIOD = new TokenType(
      "?.", "QUESTION_PERIOD", SELECTOR_PRECEDENCE, QUESTION_PERIOD_TOKEN,
            isOperator: true);

        public static TokenType QUESTION_QUESTION = new TokenType(
      "??", "QUESTION_QUESTION", IF_NULL_PRECEDENCE, QUESTION_QUESTION_TOKEN,
            isOperator: true);

        public static TokenType QUESTION_QUESTION_EQ = new TokenType("??=",
            "QUESTION_QUESTION_EQ", ASSIGNMENT_PRECEDENCE, QUESTION_QUESTION_EQ_TOKEN,
            isOperator: true);

        public static TokenType SEMICOLON =
          new TokenType(";", "SEMICOLON", NO_PRECEDENCE, SEMICOLON_TOKEN);

        public static TokenType SLASH = new TokenType(
      "/", "SLASH", MULTIPLICATIVE_PRECEDENCE, SLASH_TOKEN,
            isOperator: true, isUserDefinableOperator: true);

        public static TokenType SLASH_EQ = new TokenType(
      "/=", "SLASH_EQ", ASSIGNMENT_PRECEDENCE, SLASH_EQ_TOKEN,
            isOperator: true);

        public static TokenType STAR = new TokenType(
      "*", "STAR", MULTIPLICATIVE_PRECEDENCE, STAR_TOKEN,
            isOperator: true, isUserDefinableOperator: true);

        public static TokenType STAR_EQ = new TokenType(
      "*=", "STAR_EQ", ASSIGNMENT_PRECEDENCE, STAR_EQ_TOKEN,
            isOperator: true);

        public static TokenType STRING_INTERPOLATION_EXPRESSION = new TokenType(
      "${",
            "STRING_INTERPOLATION_EXPRESSION",
            NO_PRECEDENCE,
            STRING_INTERPOLATION_TOKEN);

        public static TokenType STRING_INTERPOLATION_IDENTIFIER = new TokenType(
      "$",
            "STRING_INTERPOLATION_IDENTIFIER",
            NO_PRECEDENCE,
            STRING_INTERPOLATION_IDENTIFIER_TOKEN);

        public static TokenType TILDE = new TokenType(
      "~", "TILDE", PREFIX_PRECEDENCE, TILDE_TOKEN,
            isOperator: true, isUserDefinableOperator: true);

        public static TokenType TILDE_SLASH = new TokenType(
      "~/", "TILDE_SLASH", MULTIPLICATIVE_PRECEDENCE, TILDE_SLASH_TOKEN,
            isOperator: true, isUserDefinableOperator: true);

        public static TokenType TILDE_SLASH_EQ = new TokenType(
      "~/=", "TILDE_SLASH_EQ", ASSIGNMENT_PRECEDENCE, TILDE_SLASH_EQ_TOKEN,
            isOperator: true);

        public static TokenType BACKPING =
          new TokenType("`", "BACKPING", NO_PRECEDENCE, BACKPING_TOKEN);

        public static TokenType BACKSLASH =
          new TokenType("\\", "BACKSLASH", NO_PRECEDENCE, BACKSLASH_TOKEN);

        public static TokenType PERIOD_PERIOD_PERIOD = new TokenType(
      "...", "PERIOD_PERIOD_PERIOD", NO_PRECEDENCE, PERIOD_PERIOD_PERIOD_TOKEN);

        public static TokenType AS = Keyword.AS;

        public static TokenType IS = Keyword.IS;

        /**
         * Token type used by error tokens.
         */
        public static TokenType BAD_INPUT = new TokenType(
      "malformed input", "BAD_INPUT", NO_PRECEDENCE, BAD_INPUT_TOKEN,
            stringValue: null);

        /**
         * Token type used by synthetic tokens that are created during parser
         * recovery (non-analyzer use case).
         */
        public static TokenType RECOVERY = new TokenType(
      "recovery", "RECOVERY", NO_PRECEDENCE, RECOVERY_TOKEN,
            stringValue: null);

        // TODO(danrubel): "all" is misleading
        // because this list does not include all TokenType instances.
        public static List<TokenType> all = new List<TokenType> {
                                  TokenType.EOF,
                                  TokenType.DOUBLE,
                                  TokenType.HEXADECIMAL,
                                  TokenType.IDENTIFIER,
                                  TokenType.INT,
                                  TokenType.MULTI_LINE_COMMENT,
                                  TokenType.SCRIPT_TAG,
                                  TokenType.SINGLE_LINE_COMMENT,
                                  TokenType.STRING,
                                  TokenType.AMPERSAND,
                                  TokenType.AMPERSAND_AMPERSAND,
                                  TokenType.AMPERSAND_EQ,
                                  TokenType.AT,
                                  TokenType.BANG,
                                  TokenType.BANG_EQ,
                                  TokenType.BAR,
                                  TokenType.BAR_BAR,
                                  TokenType.BAR_EQ,
                                  TokenType.COLON,
                                  TokenType.COMMA,
                                  TokenType.CARET,
                                  TokenType.CARET_EQ,
                                  TokenType.CLOSE_CURLY_BRACKET,
                                  TokenType.CLOSE_PAREN,
                                  TokenType.CLOSE_SQUARE_BRACKET,
                                  TokenType.EQ,
                                  TokenType.EQ_EQ,
                                  TokenType.FUNCTION,
                                  TokenType.GT,
                                  TokenType.GT_EQ,
                                  TokenType.GT_GT,
                                  TokenType.GT_GT_EQ,
                                  TokenType.HASH,
                                  TokenType.INDEX,
                                  TokenType.INDEX_EQ,
                                  TokenType.LT,
                                  TokenType.LT_EQ,
                                  TokenType.LT_LT,
                                  TokenType.LT_LT_EQ,
                                  TokenType.MINUS,
                                  TokenType.MINUS_EQ,
                                  TokenType.MINUS_MINUS,
                                  TokenType.OPEN_CURLY_BRACKET,
                                  TokenType.OPEN_PAREN,
                                  TokenType.OPEN_SQUARE_BRACKET,
                                  TokenType.PERCENT,
                                  TokenType.PERCENT_EQ,
                                  TokenType.PERIOD,
                                  TokenType.PERIOD_PERIOD,
                                  TokenType.PLUS,
                                  TokenType.PLUS_EQ,
                                  TokenType.PLUS_PLUS,
                                  TokenType.QUESTION,
                                  TokenType.QUESTION_PERIOD,
                                  TokenType.QUESTION_QUESTION,
                                  TokenType.QUESTION_QUESTION_EQ,
                                  TokenType.SEMICOLON,
                                  TokenType.SLASH,
                                  TokenType.SLASH_EQ,
                                  TokenType.STAR,
                                  TokenType.STAR_EQ,
                                  TokenType.STRING_INTERPOLATION_EXPRESSION,
                                  TokenType.STRING_INTERPOLATION_IDENTIFIER,
                                  TokenType.TILDE,
                                  TokenType.TILDE_SLASH,
                                  TokenType.TILDE_SLASH_EQ,
                                  TokenType.BACKPING,
                                  TokenType.BACKSLASH,
                                  TokenType.PERIOD_PERIOD_PERIOD };

        // TODO(danrubel): Should these be added to the "all" list?
        //TokenType.IS,
        //TokenType.AS,

        // These are not yet part of the language and not supported by fasta
        //TokenType.AMPERSAND_AMPERSAND_EQ,
        //TokenType.BAR_BAR_EQ,

        // Supported by fasta but not part of the language
        //TokenType.BANG_EQ_EQ,
        //TokenType.EQ_EQ_EQ,

        // Used by synthetic tokens generated during recovery
        //TokenType.BAD_INPUT,
        //TokenType.RECOVERY,

        public readonly int kind;

        /**
         * `true` if this token type represents a modifier
         * such as `abstract` or `const`.
         */
        public readonly bool isModifier;

        /**
         * `true` if this token type represents an operator.
         */
        public readonly bool isOperator;

        /**
         * `true` if this token type represents a keyword starting a top level
         * declaration such as `class`, `enum`, `import`, etc.
         */
        public readonly bool isTopLevelKeyword;

        /**
         * `true` if this token type represents an operator
         * that can be defined by users.
         */
        public readonly bool isUserDefinableOperator;

        /**
         * The lexeme that defines this type of token,
         * or `null` if there is more than one possible lexeme for this type of token.
         */
        public readonly String lexeme;

        /**
         * The name of the token type.
         */
        public readonly String name;

        /**
         * The precedence of this type of token,
         * or `0` if the token does not represent an operator.
         */
        public readonly int precedence;

        /**
         * See [Token.stringValue] for an explanation.
         */
        public readonly String stringValue;

        public TokenType(String lexeme,
                         String name,
                         int precedence,
                         int kind,
                         bool isModifier = false,
                         bool isOperator = false,
                         bool isTopLevelKeyword = false,
                         bool isUserDefinableOperator = false,
                         String stringValue = "unspecified")
        {
            this.lexeme = lexeme;
            this.name = name;
            this.precedence = precedence;
            this.kind = kind;
            this.isModifier = isModifier;
            this.isOperator = isOperator;
            this.isTopLevelKeyword = isTopLevelKeyword;
            this.isUserDefinableOperator = isUserDefinableOperator;

            this.stringValue = stringValue == "unspecified" ? lexeme : stringValue;
        }


        /**
         * Return `true` if this type of token represents an additive operator.
         */
        public bool isAdditiveOperator => precedence == ADDITIVE_PRECEDENCE;

        /**
         * Return `true` if this type of token represents an assignment operator.
         */
        public bool isAssignmentOperator => precedence == ASSIGNMENT_PRECEDENCE;

        /**
         * Return `true` if this type of token represents an associative operator. An
         * associative operator is an operator for which the following equality is
         * true: `(a * b) * c == a * (b * c)`. In other words, if the result of
         * applying the operator to multiple operands does not depend on the order in
         * which those applications occur.
         *
         * Note: This method considers the logical-and and logical-or operators to be
         * associative, even though the order in which the application of those
         * operators can have an effect because evaluation of the right-hand operand
         * is conditional.
         */
        public bool sAssociativeOperator =>
                      this == TokenType.AMPERSAND ||
                      this == TokenType.AMPERSAND_AMPERSAND ||
                      this == TokenType.BAR ||
                      this == TokenType.BAR_BAR ||
                      this == TokenType.CARET ||
                      this == TokenType.PLUS ||
                      this == TokenType.STAR;

        /**
         * A flag indicating whether the keyword is a "built-in" identifier.
         */
        public bool isBuiltIn => false;

        /**
         * Return `true` if this type of token represents an equality operator.
         */
        public bool isEqualityOperator =>
        this == TokenType.BANG_EQ || this == TokenType.EQ_EQ;

        /**
         * Return `true` if this type of token represents an increment operator.
         */
        public bool isIncrementOperator =>
                        this == TokenType.PLUS_PLUS || this == TokenType.MINUS_MINUS;

        /**
         * Return `true` if this type of token is a keyword.
         */
        public bool isKeyword => kind == KEYWORD_TOKEN;

        /**
         * A flag indicating whether the keyword can be used as an identifier
         * in some situations.
         */
        public bool isPseudo => false;

        /**
         * Return `true` if this type of token represents a multiplicative operator.
         */
        public bool isMultiplicativeOperator => precedence == MULTIPLICATIVE_PRECEDENCE;

        /**
         * Return `true` if this type of token represents a relational operator.
         */
        public bool isRelationalOperator =>
            this == TokenType.LT ||
            this == TokenType.LT_EQ ||
            this == TokenType.GT ||
            this == TokenType.GT_EQ;

        /**
         * Return `true` if this type of token represents a shift operator.
         */
        public bool isShiftOperator => precedence == SHIFT_PRECEDENCE;

        /**
         * Return `true` if this type of token represents a unary postfix operator.
         */
        public bool isUnaryPostfixOperator => precedence == POSTFIX_PRECEDENCE;

        /**
         * Return `true` if this type of token represents a unary prefix operator.
         */
        public bool isUnaryPrefixOperator =>
                      precedence == PREFIX_PRECEDENCE ||
                      this == TokenType.MINUS ||
                      this == TokenType.PLUS_PLUS ||
                      this == TokenType.MINUS_MINUS;

        /**
         * Return `true` if this type of token represents a selector operator
         * (starting token of a selector).
         */
        public bool isSelectorOperator => precedence == SELECTOR_PRECEDENCE;

        public String toString() => name;

        /**
         * Use [lexeme] instead of this method
         */
        public String value => lexeme;
    }
}
