using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dart2CSharpTranspiler.Writer.RewriteRules;

namespace Dart2CSharpTranspiler.Writer
{
    public static class RewriteRuleProvider
    {
        private static List<RewriteRule> _rules;

        private static List<RewriteRule> Rules()
        {
            if (_rules == null)
            {
                var assembly = Assembly.GetAssembly(typeof(RewriteRuleProvider));
                var rewriteRules = assembly.DefinedTypes.Where(x => x.BaseType == typeof(RewriteRule));
                _rules = rewriteRules.Select(x => (RewriteRule)Activator.CreateInstance(x)).ToList();
            }

            return _rules;
        }

        public static RewriteRule FindRuleForClass(string dartClassName)
        {
            return Rules().FirstOrDefault(x => string.Equals(x.DartClassName, dartClassName, StringComparison.InvariantCultureIgnoreCase));
        }

        public static bool ShouldImportGetRemoved(string dartImport)
        {
            return Rules().Any(x => string.Equals(x.RemovedImport, dartImport, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}