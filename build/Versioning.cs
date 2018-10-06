using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.Tools.GitVersion;
using System;
using System.IO;
using System.Text.RegularExpressions;
using Nuke.Common.Tools.Git;
using static Nuke.Common.IO.PathConstruction;

partial class Build : NukeBuild
{
    /// Semantic versioning. Must have 'GitVersion.CommandLine' referenced.
    /// GitVersion.GetNormalizedFileVersion() is 0.1.0
    /// GitVersion.AssemblySemVer is 0.1.0.0
    [GitVersion]
    readonly GitVersion GitVersion;

    /// Parses origin, branch name and head from git config.
    [GitRepository]
    readonly GitRepository GitRepository;

    string Branch => GitTasks.GitCurrentBranch();
    bool HasUncommitedChanges => !GitTasks.GitHasCleanWorkingCopy();

    AbsolutePath VersionFile => MySourceDirectory / "Shared" / "InstanceInfo.cs";
    const string VersionPrefix = "ver-";

    bool CanChangeVersion => !HasUncommitedChanges
                             && (Branch.Equals("dev") || Branch.Equals("master"))
                             && VersionFileIsOk();

    Target ShowVersion => _ => _
        .Executes(() =>
        {
            Console.WriteLine($"GitVersion {GitVersion.GetNormalizedFileVersion()}");
            Console.WriteLine($"GitVersion {GitVersion.AssemblySemVer}");
            Console.WriteLine($"GitVersion {GitVersion.InformationalVersion}");

            var currentVersion = GetVersion();
            Console.WriteLine($"VersionFile {currentVersion.ToString()}");
        });

    Target IncMinorVer => _ => _
        .Executes(() =>
        {
            if (!CanChangeVersion)
            {
                var message = $"can`t change version (commit all, use branch 'dev' or 'master')";
                throw new Exception(message);
            }
            else
            {
                var oldVersion = GetVersion();
                var newVersion = oldVersion.Copy();

                newVersion.IncreaseMinor();

                newVersion.SetSha(GitVersion.Sha);
                SetVersion(oldVersion, newVersion);

                GitTasks.Git($"commit -a -m \"Change version from {oldVersion} to {newVersion}\"");
            }
        });

    Target IncMajorVer => _ => _
        .Executes(() =>
        {
            if (!CanChangeVersion)
            {
                var message = $"can`t change version (commit all, use branch 'dev' or 'master')";
                throw new Exception(message);
            }
            else
            {
                var oldVersion = GetVersion();
                var newVersion = oldVersion.Copy();

                newVersion.IncreaseMajor();

                newVersion.SetSha(GitVersion.Sha);
                SetVersion(oldVersion, newVersion);

                GitTasks.Git($"commit -a -m \"Change version from {oldVersion} to {newVersion}\"");
            }
        });

    bool VersionFileIsOk()
    {
        return File.Exists(VersionFile);
    }

    CustomVersion GetVersion()
    {
        if (!VersionFileIsOk())
        {
            var message = $"Not Valid VersionFile {VersionFile}";
            throw new Exception(message);
        }

        var data = File.ReadAllText(VersionFile);
        var version = Regex
            .Match(data, "string CodeVer([\\s\\w{};]*)=(?<val>[\\w\\s\\d\\\"-.]*);", RegexOptions.Multiline)
            .Groups["val"].Value;
        version = version.Trim().Replace("\"", "");

        if (!version.StartsWith(VersionPrefix))
            throw new Exception($"Wrong Version {version} in {VersionFile}");
        else
            version = version.Substring(VersionPrefix.Length);

        if (string.IsNullOrWhiteSpace(version))
            throw new Exception($"Wrong Version {version} in {VersionFile}");

        return new CustomVersion(version);
    }

    void SetVersion(CustomVersion oldVersion, CustomVersion newVersion)
    {
        var data = File.ReadAllText(VersionFile);
        data = data.Replace(VersionPrefix + oldVersion, VersionPrefix + newVersion);
        File.WriteAllText(VersionFile, data);
    }
}