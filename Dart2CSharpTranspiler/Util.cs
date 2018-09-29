using System;
using System.Collections.Generic;
using System.Text;

namespace Transpiler
{
    public static class Util
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="openChar"></param>
        /// <param name="endChar"></param>
        /// <param name="ignoreInString">By switching this to true, it doesnt end the container if the endChar is inside a string.</param>
        /// <returns></returns>
        public static string GetContainerCode(string value, char openChar, char endChar, bool ignoreInString = false)
        {
            var container = "";
            var opened = false;
            var openCount = 0;
            var insideString = false;
            var previousChar = ' ';
            var stringMarker = ' ';

            foreach (var c in value)
            {
                if ((c == '"' || c == '\'') && previousChar != '\\')
                {
                    if (!insideString)
                    {
                        insideString = true;
                        stringMarker = c;
                    }
                    else if (c == stringMarker)
                    {
                        insideString = false;
                        stringMarker = ' ';
                    }
                }

                if (c == openChar && !opened)
                {
                    opened = true;
                    openCount++;
                    continue;
                }
                else if (opened && (!insideString || !ignoreInString))
                {
                    if (c == openChar)
                        openCount++;
                    else if (c == endChar)
                    {
                        openCount--;
                    }

                    if (openCount == 0)
                        break;
                }

                if (opened)
                    container += c;

                previousChar = c;
            }

            return container;
        }

        public static string[] SplitViaCharacter(this string value, char separator)
        {
            var list = new List<string>();

            var ready = true;
            var tmp = "";
            char openCharacter = '*';
            int openCount = 0;
            foreach (var c in value.Trim())
            {
                if ((c == '\'' || c == '<' || c == '(') && ready == true)
                {
                    openCharacter = c;
                    ready = false;
                    tmp += c;
                }
                else if ((c == '\'' || c == '<' || c == '(') && ready == false)
                {
                    openCount++;
                }
                else if ((c == '\'' || (c == '>' && openCharacter == '<') || (c == ')' && openCharacter == '(')) && ready == false && openCount > 0)
                {
                    openCount--;
                }
                else if ((c == '\'' || (c == '>' && openCharacter == '<') || (c == ')' && openCharacter == '(')) && ready == false)
                {
                    ready = true;
                    tmp += c;
                }
                else if (c == separator && ready)
                {
                    list.Add(tmp);
                    tmp = "";
                }
                else
                    tmp += c;
            }

            list.Add(tmp);

            return list.ToArray();
        }


    }
}
