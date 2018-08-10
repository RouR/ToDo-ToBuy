using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using TypeLite;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

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

            var appDomain = AppDomain.CurrentDomain;
            var loadedAssembiles = new List<Assembly>(100);

            var projects = ProjectModelTasks.ParseSolution(SolutionFile).Projects;
            foreach (var project in projects.Where(x => x.Name.Contains("DTO")))
            {
                try
                {
                    DotNetBuild(s => DefaultDotNetBuild.SetProjectFile(project.Path));

                    //var dllFile = Path.Combine(project.Directory, "bin", DefaultDotNetBuild.Configuration, DefaultDotNetBuild.Framework ?? "", project.Name, ".dll");
                    //var dllFile = BuildAssemblyDirectory ;

                    var dllFiles = GlobFiles(SourceDirectory, "*.dll")
                        .Where(x => !x.Contains("obj") && x.StartsWith(project.Directory))
                        .Distinct()
                        .ToArray();

                    foreach (var file in dllFiles)
                    {
                        Console.WriteLine("Load assembly " + file);
                        var newFilePath = Path.Combine(appDomain.BaseDirectory, Path.GetFileName(file));
                        File.Copy(file, newFilePath, true);
                        var asm = AssemblyLoadContext.Default.LoadFromAssemblyPath(file);
                        loadedAssembiles.Add(asm);
                    }


                    //var assembly = appDomain.GetAssemblies().Single(x => x.FullName.StartsWith(project.Name + ","));
                    var assembly = loadedAssembiles.Single(x => x.FullName.StartsWith(project.Name + ","));

                    var models = assembly.GetTypes().Where(x => x.Namespace.StartsWith(project.Name + ".Public"));

                    var generator = new TypeScriptFluent().WithConvertor<Guid>(c => "string");


                    foreach (var model in models)
                    {
                        generator.ModelBuilder.Add(model);
                    }

                    //Generate enums
                    var tsEnumDefinitions = generator.Generate(TsGeneratorOutput.Enums);
                    File.WriteAllText(Path.Combine(outputPath, "enums.ts"), tsEnumDefinitions);
                    //Generate interface definitions for all classes
                    var tsClassDefinitions =
                        generator.Generate(TsGeneratorOutput.Properties | TsGeneratorOutput.Fields);
                    File.WriteAllText(Path.Combine(outputPath, "classes.d.ts"), tsClassDefinitions);
                }
                catch (ReflectionTypeLoadException ex)
                {
                    /*
                     To get NuGet assemblies in build folder add in csproj of your module
                        <PropertyGroup>
                            <CopyLocalLockFileAssemblies>true</CopyLocalLockFileAssemblies>
                        </PropertyGroup>
                     */
                    WriteError(ex.Message);
                    foreach (var loaderException in ex.LoaderExceptions)
                    {
                        WriteError("Can`t load " + loaderException.Message);
                    }
                }
                catch (Exception ex)
                {
                    WriteError(ex.Message);
                    //throw;
                }
            }

            foreach (var assembly in loadedAssembiles)
            {
            }
        });
}