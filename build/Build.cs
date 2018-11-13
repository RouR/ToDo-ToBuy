using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.BuildServers;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.EnvironmentInfo;

//https://github.com/nuke-build/nuke
partial class Build : NukeBuild
{
    //powershell -ExecutionPolicy ByPass -File ./build.ps1 -target AllCustom
    //powershell -ExecutionPolicy ByPass -File ./build.ps1
    
    //or just run (force rebuild this project and F5) this project to debug Build Targets

    /// <summary>
    ///  Console application entry point. Also defines the default target.
    /// </summary>
    /// <returns></returns>
    public static int Main()
    {
        return Execute<Build>(x => x.AllCustom);
    }

    // Auto-injection fields:

    // require Mono, don`t work in TravisCI [GitVersion] readonly GitVersion GitVersion;
    // Semantic versioning. Must have 'GitVersion.CommandLine' referenced.

    // [GitRepository] readonly GitRepository GitRepository;
    // Parses origin, branch name and head from git config.

    // [Parameter] readonly string MyGetApiKey;
    // Returns command-line arguments and environment variables.

    /// Provides access to the structure of the solution.
    [Solution(MySolutionFile)] readonly Solution Solution;

    const string MySolutionFile = "ExampleTDTB.sln";
    AbsolutePath MySolutionDirectory => RootDirectory;
    AbsolutePath MySourceDirectory => RootDirectory / "src";
    AbsolutePath MyOutputDirectory => RootDirectory / "output";
    AbsolutePath[] MyTsModelsDirectories => new []
    {
        RootDirectory / "_tsModels",
        RootDirectory / "src"/ "ngApp" / "src"/ "_tsModels",
    };
    AbsolutePath MyRestClientsDirectory => RootDirectory / "_csREST";

    Target Clean => _ => _
        //.OnlyWhen(() => false) // Disabled for safety.
        .Executes(() =>
        {
            DeleteDirectories(GlobDirectories(MySourceDirectory, "**/bin", "**/obj"));
            EnsureCleanDirectory(MyOutputDirectory);
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetWorkingDirectory(MySolutionDirectory)
                .SetProjectFile(MySolutionFile));
        });

    Target CompileDotNet => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {            
            Console.WriteLine("Solution CompileDotNet");



            var oldVersion = GetVersion();
            var newVersion = oldVersion.Copy();
            newVersion.IncreaseBuild();
            newVersion.SetSha(GitVersion.Sha);
            SetVersion(oldVersion, newVersion);

            DotNetTasks.DotNetBuild(s => s
                .SetWorkingDirectory(MySolutionDirectory)
                .SetProjectFile(MySolutionFile)
                .EnableNoRestore()
                //.SetConfiguration(Configuration)
                .SetAssemblyVersion(newVersion.ToAssemblyVersion())
                .SetFileVersion(newVersion.ToFileVersion())
                .SetInformationalVersion(GitVersion.InformationalVersion)
                );
        });

    Target Debug => _ => _
        .Executes(() =>
        {
            void TraceItem(string key, string value) => Console.WriteLine($"  - {key} = {value}");

            Logger.Trace("Environment variables:");
            char[] pathSeparators = { EnvironmentInfo.IsWin ? ';' : ':' }; 
            foreach (var pair in EnvironmentInfo.Variables.OrderBy(x => x.Key, StringComparer.OrdinalIgnoreCase))
            {
                if (pair.Key.EqualsOrdinalIgnoreCase("path"))
                {
                    TraceItem(pair.Key, pair.Value);

                    var paths = pair.Value.Split(pathSeparators);
                    var padding = paths.Length.ToString().Length;

                    for (var i = 0; i < paths.Length; i++)
                        TraceItem($"{pair.Key}[{i.ToString().PadLeft(padding, paddingChar: '0')}]", paths[i]);
                }
                else
                {
                    TraceItem(pair.Key, pair.Value);
                }
            } 
        });

    Target AllCustom => _ => _
        .DependsOn(CompileDotNet)
        .DependsOn(TsGen)
        .DependsOn(Microdocum)
        .DependsOn(RunTemplate)
        //.DependsOn(Test)
        .Executes(() =>
        {
            Console.WriteLine("Custom targets finished");
            Console.WriteLine("IsWin " + IsWin);
            Console.WriteLine("IsUnix " + IsUnix);
            Console.WriteLine("build server " + Host);

            if (AppVeyor.Instance != null)
                Console.WriteLine("Run inside AppVeyor build server.");

            if (Travis.Instance != null)
                Console.WriteLine("Run inside Travis build server.");

           
        });

  

   

}