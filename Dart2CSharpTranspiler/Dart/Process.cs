using System.Collections.Generic;

namespace Dart2CSharpTranspiler.Dart
{
    public class Process
    {

        public static DartFile CreateFile(string name, string folder, CompilationUnit ast)
        {

            var file = new DartFile()
            {
                Name = name,
                Folder = folder,
                Classes = new List<DartClass>(),
                Imports = new List<DartImport>(),
                Sections = new List<Section>()
            };



            return file;

        }

    }
}