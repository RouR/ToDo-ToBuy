﻿using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using NuGet.Common;
using Nuke.Common;
using Nuke.Common.BuildServers;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using TypeLite;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

//https://github.com/nuke-build/nuke
class Build : NukeBuild
{
    //powershell -ExecutionPolicy ByPass -File ./build.ps1 -target clean
    //OR
    //just run (F5) this project

    /// <summary>
    ///  Console application entry point. Also defines the default target.
    /// </summary>
    /// <returns></returns>
    public static int Main() => Execute<Build>(x => x.TSGen);

    // Auto-injection fields:

    // require Mono, don`t work in TravisCI [GitVersion] readonly GitVersion GitVersion;
    // Semantic versioning. Must have 'GitVersion.CommandLine' referenced.

    // [GitRepository] readonly GitRepository GitRepository;
    // Parses origin, branch name and head from git config.

    // [Parameter] readonly string MyGetApiKey;
    // Returns command-line arguments and environment variables.

    // [Solution] readonly Solution Solution;
    // Provides access to the structure of the solution.

    Target Clean => _ => _
        //.OnlyWhen(() => false) // Disabled for safety.
        .Executes(() =>
        {
            DeleteDirectories(GlobDirectories(SourceDirectory, "**/bin", "**/obj"));
            EnsureCleanDirectory(OutputDirectory);
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore(s => DefaultDotNetRestore);
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .DependsOn(MyTarget)
        .Executes(() =>
        {
            DotNetBuild(s => DefaultDotNetBuild);
        });

    //Target Test => _ => _
    //    .DependsOn(Compile)
    //    .Executes(() =>
    //    {
    //        var nunitSettings = new Nunit3Settings()
    //            .AddTargetAssemblies(GlobFiles(SolutionDirectory, $"*/bin/{Configuration}/net4*/Nuke.*.Tests.dll").NotEmpty())
    //            .AddResultReport(Xunit2ResultFormat.Xml, OutputDirectory / "tests.xml");

    //        if (IsWin)
    //        {
    //            OpenCover(s => DefaultOpenCover
    //                .SetOutput(OutputDirectory / "coverage.xml")
    //                .SetTargetSettings(xunitSettings)
    //                .SetSearchDirectories(xunitSettings.TargetAssemblyWithConfigs.Select(x => Path.GetDirectoryName(x.Key)))
    //                .AddFilters("-[Nuke.Common]Nuke.Core.*"));

    //            ReportGenerator(s => s
    //                .AddReports(OutputDirectory / "coverage.xml")
    //                .AddReportTypes(ReportTypes.Html)
    //                .SetTargetDirectory(OutputDirectory / "coverage"));
    //        }
    //        else
    //            Nunit3(s => xunitSettings);
    //    });


    Target MyTarget => _ => _
        .Executes(() =>
        {
            Console.WriteLine("Run custom target - MyTarget!");
            Console.WriteLine("IsWin " + IsWin);
            Console.WriteLine("IsUnix " + IsUnix);

            Console.WriteLine("build server " + Host);

            if (AppVeyor.Instance != null)
                Console.WriteLine("Run inside AppVeyor build server.");

            if (Travis.Instance != null)
                Console.WriteLine("Run inside Travis build server.");

            var path = Path.Combine(SolutionDirectory.ToString(), "_autogenerated.txt");
            using (var file = new StreamWriter(path, false))
            {
                file.WriteLine("autogenerated at " + DateTime.UtcNow.ToString("f", CultureInfo.InvariantCulture));
                //file.WriteLine($"git commit {GitVersion.BranchName} {GitVersion.Sha} at {GitVersion.CommitDate}");
            }
        });

    Target TSGen => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            Console.WriteLine("Run custom target - TSGen!");

            var outputPath = Path.Combine(SolutionDirectory.ToString(), "_tsModels");

            if (Directory.Exists(outputPath))
                Directory.Delete(outputPath, true);
            Directory.CreateDirectory(outputPath);

            var appDomain = AppDomain.CurrentDomain;

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
                        var aName = AssemblyLoadContext.GetAssemblyName(file);
                        //appDomain.Load(aName);
                        AssemblyLoadContext.Default.LoadFromAssemblyPath(file);
                    }


                    var assembly = appDomain.GetAssemblies().Single(x => x.FullName.StartsWith(project.Name + ","));

                    var models = assembly.GetTypes().Where(x => x.Namespace.StartsWith(project.Name + ".Public"));

                    var generator = new TypeScriptFluent()
                            .WithConvertor<Guid>(c => "string")
                        ;
                    

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
        });

    private static void WriteError(string message)
    {
        var original = Console.BackgroundColor;
        Console.BackgroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.BackgroundColor = original;
    }
}