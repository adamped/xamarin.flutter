namespace Dart2CSharpTranspiler.Writer.RewriteRules
{
    public class DurationRewriteRule : RewriteRule
    {
        public override string ReplacementClassName { get; } = "TimeSpan";
        public override string DartClassName { get; } = "Duration";
         
        public override string RequiredUsing { get; } = "System";
         
        public override string RemovedImport { get; } 
    }
    
}