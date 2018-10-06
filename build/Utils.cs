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
    static void WriteError(string message)
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
        public List<LoadedAssembly> AllLoaded { get; set; }
        public List<DtoInfo> Dto { get; set; }
    }

    AssemblyData Cached = null;

    AssemblyData LoadDtoAssemblies()
    {
        if (Cached != null)
            return Cached;

        var projects = ProjectModelTasks.ParseSolution(MySolutionFile).Projects;
        var appDomain = AppDomain.CurrentDomain;

        var loadedAssemblies = new List<LoadedAssembly>(100);
        var dtoAssemblies = new List<DtoInfo>(100);
        foreach (var project in projects.Where(x => x.Name.Contains("DTO")))
        {
            try
            {
                //DotNetBuild(s => DefaultDotNetBuild.SetProjectFile(project.Path));
                DotNetBuild(s => s.SetProjectFile(project.Path));

                //var dllFile = Path.Combine(project.Directory, "bin", DefaultDotNetBuild.Configuration, DefaultDotNetBuild.Framework ?? "", project.Name, ".dll");
                //var dllFile = BuildAssemblyDirectory ;

                var dllFiles = GlobFiles(MySourceDirectory, "*.dll")
                    .Where(x => !x.Contains("obj") && x.StartsWith(project.Directory))
                    .Distinct()
                    .ToArray();

                foreach (var file in dllFiles)
                {
                    Console.WriteLine("Load assembly " + file);
                    var fileName = Path.GetFileName(file);

                    if (loadedAssemblies.Any(x => x.FileName == fileName))
                        continue;

                    try
                    {
                        var newFilePath = Path.Combine(appDomain.BaseDirectory, fileName);
                        File.Copy(file, newFilePath, true);
                        var asm = AssemblyLoadContext.Default.LoadFromAssemblyPath(file);
                        loadedAssemblies.Add(new LoadedAssembly()
                        {
                            Assembly = asm,
                            Path = newFilePath,
                            FileName = fileName
                        });
                    }
                    catch (Exception e)
                    {
                        WriteError(e.Message);
                    }
                }


                //var assembly = appDomain.GetAssemblies().Single(x => x.FullName.StartsWith(project.Name + ","));
                var assembly = loadedAssemblies.Single(x => x.Assembly.FullName.StartsWith(project.Name + ","));
                dtoAssemblies.Add(new DtoInfo()
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

        Cached = new AssemblyData()
        {
            AllLoaded = loadedAssemblies,
            Dto = dtoAssemblies
        };
        return Cached;
    }
}