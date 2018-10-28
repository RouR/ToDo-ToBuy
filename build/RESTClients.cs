using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.Git;
using Nuke.Common.Git;
using Nuke.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using dnlib.DotNet;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Stubble.Core.Builders;
using Stubble.Core.Settings;
using static Nuke.Common.IO.PathConstruction;


partial class Build : NukeBuild
{
    AbsolutePath RestClientClassTemplateFile => RootDirectory / "rest_class.template";
    AbsolutePath RestClientPostTemplateFile => RootDirectory / "rest_post.template";
    AbsolutePath RestClientGetTemplateFile => RootDirectory / "rest_get.template";
    AbsolutePath RestClientsCopyTo => RootDirectory / "src" / "Shared";
    
    Target RESTClean => _ => _
        //.DependsOn(Clean) //After this target, will impossible run Clean, see https://github.com/dotnet/corefx/issues/14724
        .Executes(() =>
        {
            var outputPath = MyRestClientsDirectory;

            if (Directory.Exists(outputPath))
                Directory.Delete(outputPath, true);
        });

    Target REST => _ => _
        .DependsOn(RESTClean)
        .DependsOn(CompileDotNet)
        .Executes(() =>
        {
            var outputPath = MyRestClientsDirectory;

            var clients = new Dictionary<string, string>
            {
                // (projectName, className)
                {"AccountService", "AccountServiceClient"}
            };

            Directory.CreateDirectory(outputPath);

            var meta = FindAssemblyForRestClient(clients);
            var files = CreateRestFiles(meta, outputPath);

            foreach (var file in files)
            {
                var toFile = RestClientsCopyTo / Path.GetFileName(file);
                File.Copy(file, toFile, true);
            }
        });

    
    List<string> CreateRestFiles(List<RestClientMeta> meta, AbsolutePath outputPath)
    {
        var result = new List<string>();
        var stubbleRenderSettings = new RenderSettings
        {
            SkipHtmlEncoding = true,
            ThrowOnDataMiss = true,
        };
        
        foreach (var clientMeta in meta)
        {
            var tabs = 2;

            var dataHash = new Dictionary<string, object>()
            {
                
                {"classname", clientMeta.RestClientName},
                {
                    "tabs", new Func<string, object>((data) => //always type Func<string, object>
                    {
                        if (int.TryParse(data, out var max))
                            tabs = max;
                        WriteWarning($"Error templating for 'tabs' can`t parse int '{data}'");
                        return "";
                    })
                },
                {"code", new Func<string, object>((data) => //always type Func<string, object>
                {
                    var sb = new StringBuilder();
                    var tabPrefix = Environment.NewLine + new String('\t', tabs);

                    foreach (var controller in clientMeta.Controllers)
                    foreach (var action in controller.Actions)
                    {
                        var dataHashAction = new Dictionary<string, object>()
                        {
                            {"ControllerName" ,controller.ControllerName},
                            {"Description" ,action.Description},
                            {"ActionName" ,action.ActionName},
                            {"RequestType" ,action.RequestType},
                            {"ResponseType" ,action.ResponseType},
                        };
                        
                        var stubbleAction = new StubbleBuilder().Build();
                        using (var streamReader = new StreamReader(action.IsPost ? RestClientPostTemplateFile : RestClientGetTemplateFile , Encoding.UTF8))
                        {
                            var content = stubbleAction.Render(streamReader.ReadToEnd(), dataHashAction, stubbleRenderSettings);
                            content = string.Join(tabPrefix,content.Split(Environment.NewLine));
                            sb.AppendLine(content);
                        }
                    }
                        
                    return sb.ToString();
                })
                },
            };
            
            var stubble = new StubbleBuilder().Build();
            using (var streamReader = new StreamReader(RestClientClassTemplateFile , Encoding.UTF8))
            {
                var content = stubble.Render(streamReader.ReadToEnd(), dataHash, stubbleRenderSettings);
                var path = outputPath / (clientMeta.RestClientName+"Auto.cs");
                File.WriteAllText(path, content);
                result.Add(path);
            }
        }

        return result;
    }

    List<RestClientMeta> FindAssemblyForRestClient(Dictionary<string, string> clients)
    {
        var result = new List<RestClientMeta>();

        var projects = Solution.Projects;

        foreach (var client in clients)
            foreach (var project in projects.Where(x => x.Name.Equals(client.Key, StringComparison.InvariantCultureIgnoreCase)))
            {
                var assembly = LoadAssemblies().Single(x => x.Assembly.FullName.StartsWith(project.Name));
                var info = GetRestClientMeta(client, project, assembly.Assembly);
                var clientMeta = new RestClientMeta()
                {
                    RestClientName = client.Value,
                    Controllers = info
                };
                LoadXMLComments(clientMeta, assembly);
                result.Add(clientMeta);
            }

        return result;
    }

    void LoadXMLComments(RestClientMeta clientMeta, AssemblyMeta assemblyMeta)
    {
        var XMLpath = assemblyMeta.Path.Replace(".dll", ".xml");
        if (!File.Exists(XMLpath))
        {
            WriteWarning($"check project settings {assemblyMeta.Assembly.Name}, add XML documentation option");
            return;
        }

        var doc = new XmlDocument();
        doc.Load(XMLpath);
        foreach (XmlNode member in doc.GetElementsByTagName("member"))
        {
            var action = member.Attributes.GetNamedItem("name").Value;
            var summary = string.Empty;
            var returns = string.Empty;
            foreach (XmlNode childNode in member.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "summary":
                        summary = childNode.InnerText;
                        break;
                    case "returns":
                        returns = childNode.InnerText;
                        break;
                    default:
                        break;
                }
            }

            var actionMeta = clientMeta.Controllers
                .FirstOrDefault(x => action.Contains(x.ControllerName))
                ?.Actions.FirstOrDefault(x => action.Contains(x.ActionName) && action.Contains(x.RequestType));
            if (actionMeta != null)
            {
                var sb = new StringBuilder();
                if (!string.IsNullOrEmpty(summary))
                {
                    sb.AppendLine("/// <summary>");
                    var lines = summary
                        .Split(Environment.NewLine)
                        .Select(ReplaceMultipleSpaces)
                        .Where(x=> !string.IsNullOrWhiteSpace(x));
                    foreach (var line in lines)
                    {
                        sb.AppendLine("/// " + line);
                    }
                    sb.AppendLine("/// </summary>");
                }
                if (!string.IsNullOrEmpty(returns))
                {
                    sb.AppendLine($"/// <returns>{returns.Replace(Environment.NewLine, string.Empty)}</returns>");
                }
                actionMeta.Description = ReplaceMultipleSpaces(sb.ToString());
            }
        }
    }

    List<ControllerMethods> GetRestClientMeta(KeyValuePair<string, string> client, Project project, ModuleDefMD assembly)
    {
        var result = new List<ControllerMethods>();
        //var controllers = assembly.Assembly.GetTypes().Where(x => x.Namespace.EndsWith("Controller"));
        var controllers = assembly.Types.Where(x => x.Name.EndsWith("Controller"));

        foreach (var controller in controllers)
        {
            var controllerMeta = new ControllerMethods()
            {
                ControllerName = controller.Name.Replace("Controller", string.Empty),
                Actions = new List<ControllerActions>()
            };
            //var actions = controller.GetMethods(BindingFlags.Public);
            var actions = controller.Methods.Where(x => !x.IsConstructor && x.IsPublic);
            foreach (var action in actions)
            {
                var isPost = action.CustomAttributes.IsDefined("Microsoft.AspNetCore.Mvc.HttpPostAttribute");

                var parameters = action.Parameters.Where(x => x.IsNormalMethodParameter);
                if (parameters.Count() != 1)
                    continue;

                var requestType = parameters.First();
                if (isPost && !requestType.ParamDef.CustomAttributes.IsDefined("Microsoft.AspNetCore.Mvc.FromBodyAttribute"))
                    continue;

                var returnType = action.ReturnType;
                if (returnType.IsGenericInstanceType)
                {
                    if (returnType.ToGenericInstSig().GenericArguments.Count != 1)
                        continue;
                    returnType = returnType.ToGenericInstSig().GenericArguments.Single();
                }

                var description = string.Empty;

                controllerMeta.Actions.Add(new ControllerActions()
                {
                    Description = description,
                    ActionName = action.Name,
                    IsPost = isPost,
                    RequestType = requestType.Type.FullName,
                    ResponseType = returnType.FullName
                });
            }

            result.Add(controllerMeta);
        }

        return result.Where(x=>x.Actions.Any()).ToList();
    }

   
    class RestClientMeta
    {
        public string RestClientName { get; set; }
        public List<ControllerMethods> Controllers { get; set; }
    }

    class ControllerMethods
    {
        public string ControllerName { get; set; }
        public List<ControllerActions> Actions { get; set; }
    }

    class ControllerActions
    {
        public string Description { get; set; }
        public string ActionName { get; set; }
        public bool IsPost { get; set; }

        /// <summary>
        /// project convetion - only one parameter
        /// </summary>
        public string RequestType { get; set; }

        public string ResponseType { get; set; }
    }
}