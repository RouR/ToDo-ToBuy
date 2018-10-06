using System;
using System.IO;
using System.Linq;
using Nuke.Common;
using TypeLite;

partial class Build : NukeBuild
{
    string OutputPathForTypescriptDto()
    {
        var outputPath = Path.Combine(MySolutionDirectory.ToString(), "_tsModels");
        return outputPath;
    }

    Target TsClean => _ => _
        //.DependsOn(Clean) //After this target, will impossible run Clean, see https://github.com/dotnet/corefx/issues/14724
        .Executes(() =>
        {
            var outputPath = OutputPathForTypescriptDto();

            if (Directory.Exists(outputPath))
                Directory.Delete(outputPath, true);
        });

    Target TsGen => _ => _
        .DependsOn(TsClean)
        .DependsOn(CompileDotNet)
        .Executes(() =>
        {
            Console.WriteLine("Run custom target - TsGen!");

            var outputPath = MyTsModelsDirectory;

            Directory.CreateDirectory(outputPath);
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
            File.WriteAllText(Path.Combine(outputPath, "enums.ts"), tsEnumDefinitions);
            //Generate interface definitions for all classes
            var tsClassDefinitions =
                generator.Generate(TsGeneratorOutput.Properties | TsGeneratorOutput.Fields);
            File.WriteAllText(Path.Combine(outputPath, "classes.d.ts"), tsClassDefinitions);
        });
}