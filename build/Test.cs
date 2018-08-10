using System;
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
partial class Build : NukeBuild
{
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
}
