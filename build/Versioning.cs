using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Nuke.Common.Tools.Git;

partial class Build : NukeBuild
{
    /// Semantic versioning. Must have 'GitVersion.CommandLine' referenced.
    /// GitVersion.GetNormalizedFileVersion() is 0.1.0
    /// GitVersion.AssemblySemVer is 0.1.0.0
    [GitVersion] readonly GitVersion GitVersion;
   

    /// Parses origin, branch name and head from git config.
    [GitRepository] readonly GitRepository GitRepository;
   


    Target Ver => _ => _
        .DependsOn(Clean)
        .DependsOn(Compile)
        .Executes(() =>
        {
            Console.WriteLine(GitVersion.GetNormalizedFileVersion());
            Console.WriteLine(GitVersion.AssemblySemVer);

            var versionNormalized = "0.1.1";
            var versionAssembly = $"{versionNormalized}.4";

            var branch =  GitTasks.GitCurrentBranch();
            var hasUncommitedChanges =  !GitTasks.GitHasCleanWorkingCopy();

            var d = 0;
            //DotNetTasks.DotNetBuild(s => s
            //    .SetWorkingDirectory(MySolutionDirectory)
            //    .SetProjectFile(MySolutionFile)
            //    .SetFileVersion(versionNormalized)
            //    .SetAssemblyVersion(versionAssembly));
        });
}