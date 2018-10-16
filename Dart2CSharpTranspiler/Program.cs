using Dart2CSharpTranspiler.Dart;
using Dart2CSharpTranspiler.Writer;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dart2CSharpTranspiler
{
    class Program
    {
        static void Main(string[] args)
        {  
            string source = @"..\..\..\..\flutter\lib\src";
            string destination = @"..\..\..\..\FlutterSDK\src";
            string modelStorage = @"..\..\..\..\model.json";

            DartModel model = null;

            if (File.Exists(modelStorage))
                model = JsonConvert.DeserializeObject<DartModel>(File.ReadAllText(modelStorage));

            if (model == null)
            {
                model = BuildDartModel(source);

                File.WriteAllText(modelStorage, JsonConvert.SerializeObject(model));
            }

            CreateCSharpFiles(model, destination);

            Console.ReadLine();
        }

        private static Dictionary<string, DartClass> ClassList { get; set; }

        public static DartClass GetDartClass(string name)
        {
            if (name.Contains("<"))
                name = name.Substring(0, name.IndexOf("<")) + "<>";
            var baseClass = ClassList.SingleOrDefault(x => x.Key.EndsWith($".{name}")).Value;

            if (baseClass == null)
            {
                // Attempt a Generic incase it was an @optionalTypeArgs
                baseClass = ClassList.Single(x => x.Key.EndsWith($".{name}<>")).Value;
            }

            return baseClass;
        }

        private static DartModel BuildDartModel(string source)
        {
            var model = new DartModel();

            foreach (var sourceDirectory in Directory.GetDirectories(source))
            {
                var folder = sourceDirectory.Replace(source, "").TrimStart('\\');

                foreach (var sourceFilePath in Directory.GetFiles(sourceDirectory).Where(x=>x.EndsWith(".sm.json")))
                {
                    // Create Model from Dart File
                    var sourceFileName = sourceFilePath.Replace(sourceDirectory, "").TrimStart('\\');
                    var unit = JsonConvert.DeserializeObject<CompilationUnit>(File.ReadAllText(sourceFilePath));

                    if (model.ContainsKey(folder))
                        model[folder].Add(unit);
                    else
                    {
                        var list = new List<CompilationUnit>();
                        model.Add(folder, list);
                        list.Add(unit);
                    }
                }
            }

            return model;
        }

        private static void CreateCSharpFiles(DartModel model, string destination)
        {
            if (Directory.Exists(destination))
            {
                Directory.Delete(destination, true);
            }

            var writer = new CSharpWriter(model, "FlutterSDK");

            foreach (var folder in model)
            {
                var destinationFolder = Path.Combine(destination, NormalizationHelper.CamelCase(folder.Key));

                // Check Folder
                if (!Directory.Exists(destinationFolder))
                    Directory.CreateDirectory(destinationFolder);

                foreach (var file in folder.Value)
                {

                  //  Create CSharp File
                   var namespaceDeclaration = writer.GenerateFileSyntaxTree(file);
                    var code = namespaceDeclaration
                        .NormalizeWhitespace()
                        .ToFullString();
                    var destinationFileName = $"{NormalizationHelper.NormalizeTypeName(file.Name)}.cs";

                    var destinationPath = Path.Combine(destinationFolder, destinationFileName);


                    if (!File.Exists(destinationFileName))
                        File.Create(destinationPath).Close();

                    File.WriteAllText(destinationPath, code);
                }
            }
        }
    }
}