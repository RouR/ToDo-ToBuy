using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.BuildServers;
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
    
    
    //just run (F5) this project to debug Build Targets

    /// <summary>
    ///  Console application entry point. Also defines the default target.
    /// </summary>
    /// <returns></returns>
#if DEBUG
    public static int Main()
    {
        //return Execute<Build>(x => x.Microdocum);
        return Execute<Build>(x => x.AllCustom);
        //return Execute<Build>(x => x.Compile);
    }
#else
    this never call
    public static int Main() => Execute<Build>(x => x.Compile);
#endif

    // Auto-injection fields:

    // require Mono, don`t work in TravisCI [GitVersion] readonly GitVersion GitVersion;
    // Semantic versioning. Must have 'GitVersion.CommandLine' referenced.

    // [GitRepository] readonly GitRepository GitRepository;
    // Parses origin, branch name and head from git config.

    // [Parameter] readonly string MyGetApiKey;
    // Returns command-line arguments and environment variables.

    /// Provides access to the structure of the solution.
    //[Solution] readonly Solution Solution;
    

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

    Target Debug => _ => _
        .Executes(() =>
        {
            void TraceItem(string key, string value) => Logger.Trace($"  - {key} = {value}");

            Logger.Trace("Environment variables:");
            char[] s_pathSeparators = { EnvironmentInfo.IsWin ? ';' : ':' }; 
            foreach (var pair in EnvironmentInfo.Variables.OrderBy(x => x.Key, StringComparer.OrdinalIgnoreCase))
            {
                if (pair.Key.EqualsOrdinalIgnoreCase("path"))
                {
                    var paths = pair.Value.Split(s_pathSeparators);
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
        .DependsOn(Debug)
        .DependsOn(Compile_For_Custom)
        .DependsOn(TS_Gen)
        .DependsOn(Microdocum)
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

    Target Compile => _ => _
        //.DependsOn(AllCustom)
        .Executes(() =>
        {
            Console.WriteLine("Solution Compile");
            DotNetBuild(s => DefaultDotNetBuild);
        });

    Target Compile_For_Custom => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => DefaultDotNetBuild);
        });

}