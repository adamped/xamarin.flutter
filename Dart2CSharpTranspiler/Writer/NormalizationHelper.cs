using System.Linq;
using System.Text.RegularExpressions;

namespace Dart2CSharpTranspiler.Writer
{
    /// <summary>
    /// Helper class to normalize names to c# conventions.
    /// </summary>
    public static class NormalizationHelper
    {
        /// <summary>
        /// Formats the input text to camel case.
        /// </summary>
        public static string CamelCase(string input)
        {
            if (input.Length == 0)
                return input;
            var output = input;
            output = NormalizeGenericVariables(output);
            output = output[0].ToString().ToUpper() + output.Remove(0, 1);
            return output;
        }

        public static string PrivateField(string name)
        {
            var output = NormalizeTypeName(name);
            return "_" + output[0].ToString().ToLower() + output.Remove(0, 1);
        }

        /// <summary>
        /// Applies camel case, removes special characters.
        /// </summary>
        public static string NormalizeTypeName(string input)
        {
            // Apply lower camel case for primitive types
            if (PrimitiveTypeHelper.IsPrimitiveType(input))
                return input.ToLower();

            input = input.TrimStart('.', '_', '-');
            var parts = input.Split('_', '.', '-');
            var output = string.Empty;
            foreach (var part in parts)
            {
                if(part == "dart")
                    continue;
                output += CamelCase(part);
            }

            output = NormalizeGenericVariables(output);

            return output;
        }

        /// <summary>
        /// Formats and replaces generic parameters with c# equivalents in c# code conventions.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string NormalizeGenericVariables(string input)
        {
            var generics = Regex.Match(input, "<(.+)>");
            if (!generics.Success)
                return input;

            var genericParamters = generics.Groups.Last().Value.Split(',');

            // remove the old generics
            input = input.Replace(generics.Value, "");

            // Add them again, format each one
            input += "<";
            for (var index = 0; index < genericParamters.Length; index++)
            {
                var genericParamter = genericParamters[index];
                input += NormalizeTypeName(genericParamter);
                if (index + 1 != genericParamters.Length)
                    input += ","; 
            }
            input += ">";
            return input;
        }
    }
}