using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

partial class Build : NukeBuild
{
    
    private static void WriteError(string message)
    {
        Logger.Error(message);

        //var original = Console.BackgroundColor;
        //Console.BackgroundColor = ConsoleColor.Red;
        //Console.WriteLine(message);
        //Console.BackgroundColor = original;
    }

    struct LoadedAssembly
    {
        public string FileName { get; set; }
        public string Path { get; set; }
        public Assembly Assembly { get; set; }
    }
    struct DtoInfo
    {
        public string Namespace { get; set; }
        public string ProjectName { get; set; }
        public Assembly Assembly { get; set; }
    }

    class AssemblyData
    {
        public List<LoadedAssembly> allLoaded { get; set; }
        public List<DtoInfo> dto { get; set; }
    }

    AssemblyData _cached = null;

    AssemblyData LoadDtoAssemblies()
    {
        if (_cached != null)
            return _cached;

        var projects = ProjectModelTasks.ParseSolution(SolutionFile).Projects;
        var appDomain = AppDomain.CurrentDomain;

        var loadedAssembiles = new List<LoadedAssembly>(100);
        var dtoAssembiles = new List<DtoInfo>(100);
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
                    var fileName = Path.GetFileName(file);

                    if(loadedAssembiles.Any(x=> x.FileName == fileName))
                        continue;

                    var newFilePath = Path.Combine(appDomain.BaseDirectory, fileName);
                    File.Copy(file, newFilePath, true);
                    var asm = AssemblyLoadContext.Default.LoadFromAssemblyPath(file);
                    loadedAssembiles.Add(new LoadedAssembly()
                    {
                        Assembly = asm,
                        Path = newFilePath,
                        FileName = fileName
                    });
                }


                //var assembly = appDomain.GetAssemblies().Single(x => x.FullName.StartsWith(project.Name + ","));
                var assembly = loadedAssembiles.Single(x => x.Assembly.FullName.StartsWith(project.Name + ","));
                dtoAssembiles.Add(new DtoInfo()
                {
                    Assembly = assembly.Assembly,
                    Namespace = assembly.Assembly.FullName,
                    ProjectName = project.Name
                });
                
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
        _cached = new AssemblyData()
        {
            allLoaded = loadedAssembiles,
            dto = dtoAssembiles
        };
        return _cached;
    }
}
