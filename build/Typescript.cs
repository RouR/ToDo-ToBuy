using System;
using System.IO;
using System.Linq;
using Nuke.Common;
using Nuke.Common.Tooling;
using TypeLite;

partial class Build : NukeBuild
{
   

    Target TsClean => _ => _
        //.DependsOn(Clean) //After this target, will impossible run Clean, see https://github.com/dotnet/corefx/issues/14724
        .Executes(() =>
        {
            foreach (var myTsModelsDirectory in MyTsModelsDirectories)
            {
                var outputPath = myTsModelsDirectory;

                if (Directory.Exists(outputPath))
                    Directory.Delete(outputPath, true);
            }
          
        });

    Target TsGen => _ => _
        .DependsOn(TsClean)
        .DependsOn(CompileDotNet)
        .Executes(() =>
        {
            Console.WriteLine("Run custom target - TsGen!");

            var generator = new TypeScriptFluent().WithConvertor<Guid>(c => "string");

            var loadedAssemblies = LoadDtoAssemblies();
            foreach (var dtoAsm in loadedAssemblies.Dto)
            {
                var models = dtoAsm.Assembly.GetTypes()
                    .Where(x => x.Namespace.StartsWith(dtoAsm.ProjectName + ".Public"));

                foreach (var model in models)
                {
                    generator.ModelBuilder.Add(model);
                }
            }

            //Generate enums
            var tsEnumDefinitions = generator.Generate(TsGeneratorOutput.Enums);
            //Generate interface definitions for all classes
            var tsClassDefinitions = generator.Generate(TsGeneratorOutput.Properties | TsGeneratorOutput.Fields);

            foreach (var myTsModelsDirectory in MyTsModelsDirectories)
            {
                var outputPath = myTsModelsDirectory;

                Directory.CreateDirectory(outputPath);

                File.WriteAllText(Path.Combine(outputPath, "enums.ts"), tsEnumDefinitions);
                File.WriteAllText(Path.Combine(outputPath, "classes.d.ts"), tsClassDefinitions);
            }
           
        });


    Target Ng => _ => _
        .DependsOn(TsGen)
        .Executes(() =>
        {
            Console.WriteLine("Create Angular bundle");
            var task = ProcessTasks.StartProcess("ng", " build --verbose --aot", AngularSrcDir);
            task.WaitForExit();
            Console.Write(task.Output);
        });
}