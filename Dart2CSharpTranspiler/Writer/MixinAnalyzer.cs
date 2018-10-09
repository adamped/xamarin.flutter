using System.Collections.Generic;
using System.Linq;
using Transpiler;

namespace Dart2CSharpTranspiler.Writer
{
    /// <summary>
    /// Analyzer to find mixins in a <see cref="DartModel"/>
    /// </summary>
    public static class MixinAnalyzer
    {
        /// <summary>
        /// Finds all mixins in the DartModel.
        /// </summary>
        public static Dictionary<string, string> FindMixins(DartModel model)
        {
            var mixins = new Dictionary<string, string>();
            foreach (var modelClass in model.Values.SelectMany(files => files.SelectMany(file => file.Classes)))
            {
                if (modelClass.Extends == null || 
                    modelClass.Extends.Mixins == null) continue;

                foreach (var mixin in modelClass.Extends.Mixins)
                {
                    if (mixins.ContainsKey(mixin.ToLower()))
                        continue;
                    var normalizedName = NormalizationHelper.NormalizeTypeName(mixin);
                    var mixinInterfaceName = "I" + normalizedName;
                    mixins.Add(mixin.ToLower(), mixinInterfaceName);
                }
            }

            return mixins;
        }
    }
}