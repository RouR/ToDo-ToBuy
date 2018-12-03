using System;
using System.IO;
using System.Linq;
using MicroDocum.Analyzers.Analizers;
using MicroDocum.Graphviz;
using MicroDocum.Themes.DefaultTheme;
using Nuke.Common;
using Nuke.Common.BuildServers;
using Nuke.Common.Tooling;

//https://github.com/nuke-build/nuke
partial class Build : NukeBuild
{
    Target Microdocum => _ => _
        .DependsOn(CompileDotNet)
        .Executes(() =>
        {
            Console.WriteLine("Run custom target - Microdocum!");

            var pathDot = Path.Combine(MySolutionDirectory.ToString(), "DTO_routing.dot");
            var pathPng = Path.Combine(MySolutionDirectory.ToString(), "DTO_routing.png");

            var loadedAssemblies = LoadDtoAssemblies();

            var theme = new DefaultTheme();
            var a = new AssemblyAnalizer<DefaultLinkStyle>(theme);
            //var asm = AppDomain.CurrentDomain.GetAssemblies();
            //var c = a.Analize(asm, theme.GetAvailableThemeAttributes());
            var assemblies = loadedAssemblies.Dto.Select(x => x.Assembly).ToArray();
            var attributes = theme.GetAvailableThemeAttributes();
            var c = a.Analize(assemblies, attributes);

            var gen = new GraphvizDotGenerator<DefaultLinkStyle>(new DefaultTheme());
            var graphwizFileData = gen.Generate(c);

            /*
             For example, if you have the graphviz exe files (dot.exe, fdp.exe etc.) in a folder in C:\GraphViz 
                then your web.config or app.config should have the following:

                <appSettings>
                    <add key="graphVizLocation" value="C:\GraphViz" />
                </appSettings>
             */
            //EnvironmentInfo.SetVariable("graphVizLocation","");

            var graphizBin = "";

            if (Host == HostType.Console)
            {
                var packagesDirectory = NuGetPackageResolver.GetPackagesDirectory(null);
                var dir = Path.Combine(packagesDirectory, "graphviz");
                dir = Path.Combine(dir, Directory.EnumerateDirectories(dir).First());
                graphizBin = Path.Combine(dir, "dot.exe");
            }

            if (AppVeyor.Instance != null)
                graphizBin = "dot";

            if (Travis.Instance != null)
                graphizBin = "dot";

            using (var file = new StreamWriter(pathDot, false))
            {
                file.Write(graphwizFileData);
                Console.WriteLine("saved to " + pathDot);
            }

            var process = ProcessTasks.StartProcess(
                graphizBin,
                arguments: $"-Tpng  \"{pathDot}\" -o \"{pathPng}\""
            );
            if (process != null)
            {
                process.WaitForExit();
                Console.WriteLine("graphiz exit code " + process.ExitCode);
            }
            else
            {
                WriteError($"Can`t start graphiz {graphizBin}");
            }
        });
}