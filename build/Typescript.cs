using System;
using System.IO;
using System.Linq;
using Nuke.Common;
using TypeLite;

partial class Build : NukeBuild
{
    string OutputPathForTypescriptDTO()
    {
        var outputPath = Path.Combine(SolutionDirectory.ToString(), "_tsModels");
        return outputPath;
    }

    Target TS_Clean => _ => _
        //.DependsOn(Clean) //After this target, will impossible run Clean, see https://github.com/dotnet/corefx/issues/14724
        .Executes(() =>
        {
            var outputPath = OutputPathForTypescriptDTO();

            if (Directory.Exists(outputPath))
                Directory.Delete(outputPath, true);
        });

    Target TS_Gen => _ => _
        .DependsOn(TS_Clean)
        .DependsOn(Compile_For_Custom)
        .Executes(() =>
        {
            Console.WriteLine("Run custom target - TS_Gen!");

            var outputPath = Path.Combine(SolutionDirectory.ToString(), "_tsModels");

            Directory.CreateDirectory(outputPath);
            var generator = new TypeScriptFluent().WithConvertor<Guid>(c => "string");

            var loadedAssembiles = LoadDtoAssemblies();
            foreach (var dtoAsm in loadedAssembiles.dto)
            {
                var models = dtoAsm.Assembly.GetTypes().Where(x => x.Namespace.StartsWith(dtoAsm.ProjectName + ".Public"));

                foreach (var model in models)
                {
                    generator.ModelBuilder.Add(model);
                }
            }

            //Generate enums
            var tsEnumDefinitions = generator.Generate(TsGeneratorOutput.Enums);
            File.WriteAllText(Path.Combine(outputPath, "enums.ts"), tsEnumDefinitions);
            //Generate interface definitions for all classes
            var tsClassDefinitions =
                generator.Generate(TsGeneratorOutput.Properties | TsGeneratorOutput.Fields);
            File.WriteAllText(Path.Combine(outputPath, "classes.d.ts"), tsClassDefinitions);
        });

 

    
}