using Nuke.Common;

//https://github.com/nuke-build/nuke
partial class Build : NukeBuild
{
    //Target Test => _ => _
    //    .DependsOn(CompileDotNet)
    //    .Executes(() =>
    //    {
    //        var nunitSettings = new Nunit3Settings()
    //            .AddTargetAssemblies(GlobFiles(mySolutionDirectory, $"*/bin/{Configuration}/net4*/Nuke.*.Tests.dll").NotEmpty())
    //            .AddResultReport(Xunit2ResultFormat.Xml, myOutputDirectory / "tests.xml");

    //        if (IsWin)
    //        {
    //            OpenCover(s => DefaultOpenCover
    //                .SetOutput(myOutputDirectory / "coverage.xml")
    //                .SetTargetSettings(xunitSettings)
    //                .SetSearchDirectories(xunitSettings.TargetAssemblyWithConfigs.Select(x => Path.GetDirectoryName(x.Key)))
    //                .AddFilters("-[Nuke.Common]Nuke.Core.*"));

    //            ReportGenerator(s => s
    //                .AddReports(myOutputDirectory / "coverage.xml")
    //                .AddReportTypes(ReportTypes.Html)
    //                .SetTargetDirectory(myOutputDirectory / "coverage"));
    //        }
    //        else
    //            Nunit3(s => xunitSettings);
    //    });
}
