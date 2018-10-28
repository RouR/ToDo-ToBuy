using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Nuke.Common;
using Stubble.Core.Builders;
using Stubble.Core.Settings;
using static Nuke.Common.IO.PathConstruction;

partial class Build : NukeBuild
{
    // ReSharper disable InconsistentNaming
    // ReSharper disable once CommentTypo
    /// <summary>
    /// -k8snamespace stage
    /// </summary>
    [Parameter()] readonly string K8sNamespace;
    
    AbsolutePath MyK8sDirectory => RootDirectory / "k8s";
    AbsolutePath MyK8sTemplatesDirectory => RootDirectory / "templates" / "k8s";
    AbsolutePath MyK8sTemplatesValueFile => RootDirectory / "templates" / "k8s" / "_values.json";
   
    protected internal const string TemplateTravisFileNamePattern = ".template";

    AbsolutePath TravisTemplateFile => RootDirectory / "templates" / ".travis.template";
    AbsolutePath TravisFile => RootDirectory / ".travis.yml";
    // ReSharper restore InconsistentNaming
    Target RunTemplate => _ => _
        .After(IncMajorVer, IncMinorVer)
        .DependsOn(TemplateTravis)
        .Executes(() =>
        {
            var version = GetVersion();

            k8sOverrideTemplates(version);
        });

    Target TemplateTravis => _ => _
        .Executes(() =>
        {
            var version = GetVersion();
            travisOverrideTemplates(version);
        });

    void k8sOverrideTemplates(CustomVersion version)
    {
        var selectedNamespace = K8sNamespace ?? Branch;

        var values = JsonConvert.DeserializeObject<ValuesTemplating>(File.ReadAllText(MyK8sTemplatesValueFile));
        //values.Values["a1"]["dev"]

        var dataHash = BuildTemplateValue(values, version);
        //https://github.com/StubbleOrg/Stubble/blob/master/docs/how-to.md
        var stubble = new StubbleBuilder()
            .Configure(settings => { })
            .Build();

        var stubbleRenderSettings = new RenderSettings
        {
            SkipHtmlEncoding = true,
            ThrowOnDataMiss = true,
        };

        foreach (var file in Directory.EnumerateFiles(MyK8sTemplatesDirectory, "*" + TemplateTravisFileNamePattern))
        {
            using (var streamReader = new StreamReader(file, Encoding.UTF8))
            {
                var content = stubble.Render(streamReader.ReadToEnd(), dataHash, stubbleRenderSettings);
                var pathOutput = MyK8sDirectory / selectedNamespace /
                                 (Path.GetFileName(file).Replace(TemplateTravisFileNamePattern, ".yaml"));
                File.WriteAllText(pathOutput, content);
            }
        }
    }

   

    void travisOverrideTemplates(CustomVersion version, string selectedNamespace = null)
    {
        if(selectedNamespace == null)
            selectedNamespace = K8sNamespace ?? Branch;

        var dataHash = new Dictionary<string, object>()
        {
            {"dockerVer", version.ToDockerTag()},
            {"namespace", selectedNamespace},
        };
        var stubble = new StubbleBuilder().Build();

        using (var streamReader = new StreamReader(TravisTemplateFile, Encoding.UTF8))
        {
            var content = stubble.Render(streamReader.ReadToEnd(), dataHash);
            File.WriteAllText(TravisFile, content);
        }
    }

    Dictionary<string, object> BuildTemplateValue(ValuesTemplating values,
        CustomVersion version, string selectedNamespace = null)
    {
        if(selectedNamespace == null)
            selectedNamespace = K8sNamespace ?? Branch;

        var rnd = new Random();
        var dataHash = new Dictionary<string, object>()
        {
            {"docker-ver", selectedNamespace == "dev" ? "dev" : version.ToDockerTag()},
            {"namespace", selectedNamespace },
            {"$now", DateTime.Now.ToString("F")},
            {"random", new Func<string, object>((data) => //always type Func<string, object>
            {
                if(int.TryParse(data, out var max))
                    return rnd.Next(0, max);
                WriteWarning($"Error templating for 'random' can`t parse int '{data}'");
                return rnd.Next(0, 999);
            })}
        };

        foreach (var preset in values.Values)
        {
            var key = preset.Key;
            string value = null;
            if (preset.Value.TryGetValue(selectedNamespace, out value)
                || preset.Value.TryGetValue(values.DefaultNamespace, out value)
            )
            {
                dataHash.Add(key, value);
            }
        }

        Console.WriteLine("Result {0} for namespace {1}", MyK8sTemplatesValueFile, selectedNamespace);
        foreach (var key in dataHash.Keys)
        {
            Console.WriteLine("\t{0}: {1}",key, dataHash[key]?.ToString());
        }

        return dataHash;
    }
}