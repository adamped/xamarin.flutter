using System;

namespace Dart2CSharpTranspiler
{
    public static class DartCharacters
    {
        public const int _EOF = 0;
        public const int _STX = 2;
        public const int _BS = 8;
        public const int _TAB = 9;
        public const int _LF = 10;
        public const int _VTAB = 11;
        public const int _FF = 12;
        public const int _CR = 13;
        public const int _SPACE = 32;
        public const int _BANG = 33;
        public const int _DQ = 34;
        public const int _HASH = 35;
        public const int _DOLLAR = 36;
        public const int _PERCENT = 37;
        public const int _AMPERSAND = 38;
        public const int _SQ = 39;
        public const int _OPEN_PAREN = 40;
        public const int _CLOSE_PAREN = 41;
        public const int _STAR = 42;
        public const int _PLUS = 43;
        public const int _COMMA = 44;
        public const int _MINUS = 45;
        public const int _PERIOD = 46;
        public const int _SLASH = 47;
        public const int _0 = 48;
        public const int _1 = 49;
        public const int _2 = 50;
        public const int _3 = 51;
        public const int _4 = 52;
        public const int _5 = 53;
        public const int _6 = 54;
        public const int _7 = 55;
        public const int _8 = 56;
        public const int _9 = 57;
        public const int _COLON = 58;
        public const int _SEMICOLON = 59;
        public const int _LT = 60;
        public const int _EQ = 61;
        public const int _GT = 62;
        public const int _QUESTION = 63;
        public const int _AT = 64;
        public const int _A = 65;
        public const int _B = 66;
        public const int _C = 67;
        public const int _D = 68;
        public const int _E = 69;
        public const int _F = 70;
        public const int _G = 71;
        public const int _H = 72;
        public const int _I = 73;
        public const int _J = 74;
        public const int _K = 75;
        public const int _L = 76;
        public const int _M = 77;
        public const int _N = 78;
        public const int _O = 79;
        public const int _P = 80;
        public const int _Q = 81;
        public const int _R = 82;
        public const int _S = 83;
        public const int _T = 84;
        public const int _U = 85;
        public const int _V = 86;
        public const int _W = 87;
        public const int _X = 88;
        public const int _Y = 89;
        public const int _Z = 90;
        public const int _OPEN_SQUARE_BRACKET = 91;
        public const int _BACKSLASH = 92;
        public const int _CLOSE_SQUARE_BRACKET = 93;
        public const int _CARET = 94;
        public const int __ = 95;
        public const int _BACKPING = 96;
        public const int _a = 97;
        public const int _b = 98;
        public const int _c = 99;
        public const int _d = 100;
        public const int _e = 101;
        public const int _f = 102;
        public const int _g = 103;
        public const int _h = 104;
        public const int _i = 105;
        public const int _j = 106;
        public const int _k = 107;
        public const int _l = 108;
        public const int _m = 109;
        public const int _n = 110;
        public const int _o = 111;
        public const int _p = 112;
        public const int _q = 113;
        public const int _r = 114;
        public const int _s = 115;
        public const int _t = 116;
        public const int _u = 117;
        public const int _v = 118;
        public const int _w = 119;
        public const int _x = 120;
        public const int _y = 121;
        public const int _z = 122;
        public const int _OPEN_CURLY_BRACKET = 123;
        public const int _BAR = 124;
        public const int _CLOSE_CURLY_BRACKET = 125;
        public const int _TILDE = 126;
        public const int _DEL = 127;
        public const int _NBSP = 160;
        public const int _LS = 0x2028;
        public const int _PS = 0x2029;

        public const int _FIRST_SURROGATE = 0xd800;
        public const int _LAST_SURROGATE = 0xdfff;
        public const int _LAST_CODE_POINT = 0x10ffff;

        public static bool isHexDigit(int characterCode)
        {
            if (characterCode <= _9) return _0 <= characterCode;
            characterCode |= _a ^ _A;
            return (_a <= characterCode && characterCode <= _f);
        }

        public static int hexDigitValue(int hexDigit)
        {
            if (!isHexDigit(hexDigit))
                throw new Exception("Not hex digit value");

            // hexDigit is one of '0'..'9', 'A'..'F' and 'a'..'f'.
            if (hexDigit <= _9) return hexDigit - _0;
            return (hexDigit | (_a ^ _A)) - (_a - 10);
        }

        public static bool isUnicodeScalarValue(int value)
        {
            return value < _FIRST_SURROGATE ||
                (value > _LAST_SURROGATE && value <= _LAST_CODE_POINT);
        }

        public static bool isUtf16LeadSurrogate(int value)
        {
            return value >= 0xd800 && value <= 0xdbff;
        }

        public static bool isUtf16TrailSurrogate(int value)
        {
            return value >= 0xdc00 && value <= 0xdfff;
        }

    }
}