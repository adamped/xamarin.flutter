using static Dart2CSharpTranspiler.DartCharacters;

namespace Dart2CSharpTranspiler
{
    public static class DartTokenConstants
    {
        public const int EOF_TOKEN = 0;

        public const int KEYWORD_TOKEN = _k;
        public const int IDENTIFIER_TOKEN = _a;
        public const int SCRIPT_TOKEN = _b;
        public const int BAD_INPUT_TOKEN = _X;
        public const int DOUBLE_TOKEN = _d;
        public const int INT_TOKEN = _i;
        public const int RECOVERY_TOKEN = _r;
        public const int HEXADECIMAL_TOKEN = _x;
        public const int STRING_TOKEN = _SQ;

        public const int AMPERSAND_TOKEN = _AMPERSAND;
        public const int BACKPING_TOKEN = _BACKPING;
        public const int BACKSLASH_TOKEN = _BACKSLASH;
        public const int BANG_TOKEN = _BANG;
        public const int BAR_TOKEN = _BAR;
        public const int COLON_TOKEN = _COLON;
        public const int COMMA_TOKEN = _COMMA;
        public const int EQ_TOKEN = _EQ;
        public const int GT_TOKEN = _GT;
        public const int HASH_TOKEN = _HASH;
        public const int OPEN_CURLY_BRACKET_TOKEN = _OPEN_CURLY_BRACKET;
        public const int OPEN_SQUARE_BRACKET_TOKEN = _OPEN_SQUARE_BRACKET;
        public const int OPEN_PAREN_TOKEN = _OPEN_PAREN;
        public const int LT_TOKEN = _LT;
        public const int MINUS_TOKEN = _MINUS;
        public const int PERIOD_TOKEN = _PERIOD;
        public const int PLUS_TOKEN = _PLUS;
        public const int QUESTION_TOKEN = _QUESTION;
        public const int AT_TOKEN = _AT;
        public const int CLOSE_CURLY_BRACKET_TOKEN = _CLOSE_CURLY_BRACKET;
        public const int CLOSE_SQUARE_BRACKET_TOKEN = _CLOSE_SQUARE_BRACKET;
        public const int CLOSE_PAREN_TOKEN = _CLOSE_PAREN;
        public const int SEMICOLON_TOKEN = _SEMICOLON;
        public const int SLASH_TOKEN = _SLASH;
        public const int TILDE_TOKEN = _TILDE;
        public const int STAR_TOKEN = _STAR;
        public const int PERCENT_TOKEN = _PERCENT;
        public const int CARET_TOKEN = _CARET;

        public const int STRING_INTERPOLATION_TOKEN = 128;
        public const int LT_EQ_TOKEN = STRING_INTERPOLATION_TOKEN + 1;
        public const int FUNCTION_TOKEN = LT_EQ_TOKEN + 1;
        public const int SLASH_EQ_TOKEN = FUNCTION_TOKEN + 1;
        public const int PERIOD_PERIOD_PERIOD_TOKEN = SLASH_EQ_TOKEN + 1;
        public const int PERIOD_PERIOD_TOKEN = PERIOD_PERIOD_PERIOD_TOKEN + 1;
        public const int EQ_EQ_EQ_TOKEN = PERIOD_PERIOD_TOKEN + 1;
        public const int EQ_EQ_TOKEN = EQ_EQ_EQ_TOKEN + 1;
        public const int LT_LT_EQ_TOKEN = EQ_EQ_TOKEN + 1;
        public const int LT_LT_TOKEN = LT_LT_EQ_TOKEN + 1;
        public const int GT_EQ_TOKEN = LT_LT_TOKEN + 1;
        public const int GT_GT_EQ_TOKEN = GT_EQ_TOKEN + 1;
        public const int INDEX_EQ_TOKEN = GT_GT_EQ_TOKEN + 1;
        public const int INDEX_TOKEN = INDEX_EQ_TOKEN + 1;
        public const int BANG_EQ_EQ_TOKEN = INDEX_TOKEN + 1;
        public const int BANG_EQ_TOKEN = BANG_EQ_EQ_TOKEN + 1;
        public const int AMPERSAND_AMPERSAND_TOKEN = BANG_EQ_TOKEN + 1;
        public const int AMPERSAND_AMPERSAND_EQ_TOKEN = AMPERSAND_AMPERSAND_TOKEN + 1;
        public const int AMPERSAND_EQ_TOKEN = AMPERSAND_AMPERSAND_EQ_TOKEN + 1;
        public const int BAR_BAR_TOKEN = AMPERSAND_EQ_TOKEN + 1;
        public const int BAR_BAR_EQ_TOKEN = BAR_BAR_TOKEN + 1;
        public const int BAR_EQ_TOKEN = BAR_BAR_EQ_TOKEN + 1;
        public const int STAR_EQ_TOKEN = BAR_EQ_TOKEN + 1;
        public const int PLUS_PLUS_TOKEN = STAR_EQ_TOKEN + 1;
        public const int PLUS_EQ_TOKEN = PLUS_PLUS_TOKEN + 1;
        public const int MINUS_MINUS_TOKEN = PLUS_EQ_TOKEN + 1;
        public const int MINUS_EQ_TOKEN = MINUS_MINUS_TOKEN + 1;
        public const int TILDE_SLASH_EQ_TOKEN = MINUS_EQ_TOKEN + 1;
        public const int TILDE_SLASH_TOKEN = TILDE_SLASH_EQ_TOKEN + 1;
        public const int PERCENT_EQ_TOKEN = TILDE_SLASH_TOKEN + 1;
        public const int GT_GT_TOKEN = PERCENT_EQ_TOKEN + 1;
        public const int CARET_EQ_TOKEN = GT_GT_TOKEN + 1;
        public const int COMMENT_TOKEN = CARET_EQ_TOKEN + 1;
        public const int STRING_INTERPOLATION_IDENTIFIER_TOKEN = COMMENT_TOKEN + 1;
        public const int QUESTION_PERIOD_TOKEN = STRING_INTERPOLATION_IDENTIFIER_TOKEN + 1;
        public const int QUESTION_QUESTION_TOKEN = QUESTION_PERIOD_TOKEN + 1;
        public const int QUESTION_QUESTION_EQ_TOKEN = QUESTION_QUESTION_TOKEN + 1;
        public const int GENERIC_METHOD_TYPE_ASSIGN_TOKEN = QUESTION_QUESTION_EQ_TOKEN + 1;
        public const int GENERIC_METHOD_TYPE_LIST_TOKEN = GENERIC_METHOD_TYPE_ASSIGN_TOKEN + 1;

    }
}