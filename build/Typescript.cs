using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NJsonSchema;
using NJsonSchema.CodeGeneration.TypeScript;
using NJsonSchema.Generation;
using NSwag;
using NSwag.CodeGeneration.OperationNameGenerators;
using NSwag.CodeGeneration.TypeScript;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;

partial class Build : NukeBuild
{
    Target TsClean => _ => _
        //.DependsOn(Clean) //After this target, will impossible run Clean, see https://github.com/dotnet/corefx/issues/14724
        .Executes(() =>
        {
            foreach (var myTsModelsDirectory in MyTsModelsDirectories)
            {
                var outputPath = myTsModelsDirectory;

                if (Directory.Exists(outputPath))
                    Directory.Delete(outputPath, true);
            }
        });

    Target TsGen => _ => _
        .DependsOn(TsClean)
        .DependsOn(CompileDotNet)
        .Before(Microdocum)//DotNetTasks.DotNetRun before LoadDtoAssemblies()
        .Executes(async () =>
        {
            Console.WriteLine("Run custom target - TsGen!");
            
            var ngApiClient = await GetSwaggerWay1();
            var tsModels = await GetSwaggerWay2();

            foreach (var myTsModelsDirectory in MyTsModelsDirectories)
            {
                var outputPath = myTsModelsDirectory;

                Directory.CreateDirectory(outputPath);

                File.WriteAllText(Path.Combine(outputPath, "classes.ts"), tsModels);
                File.WriteAllText(Path.Combine(outputPath, "api-client.ts"), ngApiClient);
                
            }
        });

    async Task<string> GetSwaggerWay1()
    {
        //see Web -> GenerateSwaggerFiles  
        DotNetTasks.DotNetRun(
            s=>s
                //.SetWorkingDirectory(MySolutionDirectory)
                .SetProjectFile(Solution.GetProject("Web").Path)
                .SetLaunchProfile("localProfile")
                .SetApplicationArguments("swagger")
        );

        var apiVersion = "v0.1";
        
        var document = await SwaggerDocument.FromFileAsync($"./swagger/swagger-Web-{apiVersion}.json");
      
        var tsGenerator = new SwaggerToTypeScriptClientGenerator(document,
            new SwaggerToTypeScriptClientGeneratorSettings
            {
                ClassName = "{controller}Client",
                CodeGeneratorSettings =
                {
                    GenerateDefaultValues = true,
                    SchemaType = SchemaType.JsonSchema,
                },
                //RxJsVersion = 6,
                Template = TypeScriptTemplate.Angular,
                HttpClass = HttpClass.HttpClient,
                PromiseType = PromiseType.Promise,
                WrapResponses = false,
                GenerateClientClasses = true,
                GenerateClientInterfaces = false,
                GenerateDtoTypes = true,
                GenerateOptionalParameters = true,
                GenerateResponseClasses = true,
                ImportRequiredTypes = true,
                InjectionTokenType = InjectionTokenType.InjectionToken,
                UseSingletonProvider = true,
                OperationNameGenerator = new SingleClientFromPathSegmentsOperationNameGenerator(),
                WrapDtoExceptions = false,
                BaseUrlTokenName = "API_BASE_URL",
                TypeScriptGeneratorSettings =
                {
                    GenerateDefaultValues = true,
                    SchemaType = SchemaType.JsonSchema,
                    NullValue = TypeScriptNullValue.Null,
                    ExportTypes = true,
                    TypeStyle = TypeScriptTypeStyle.Class,
                    DateTimeType = TypeScriptDateTimeType.MomentJS,
                    MarkOptionalProperties = true,
                    TypeScriptVersion = 2.7m,
                    ConvertConstructorInterfaceData = false,
                    ModuleName = null,
                },
                UseGetBaseUrlMethod = false,
                UseTransformOptionsMethod = false,
                UseTransformResultMethod = false,
            });
        var ngApiClient = tsGenerator.GenerateFile();
        return ngApiClient;
    }

    async Task<string> GetSwaggerWay2()
    {
        var document = new SwaggerDocument();
        var Settings = new JsonSchemaGeneratorSettings()
        {
            SchemaType = SchemaType.Swagger2,
                
        };
        var generator = new JsonSchemaGenerator(Settings);
        var schemaResolver = new SwaggerSchemaResolver(document, Settings);

        var loadedAssemblies = LoadDtoAssemblies();
        foreach (var dtoAsm in loadedAssemblies.Dto)
        {
            var models = dtoAsm.Assembly.GetTypes()
                .Where(x => x.Namespace.StartsWith(dtoAsm.ProjectName + ".Public"));

            foreach (var model in models)
            {
                await generator.GenerateAsync(model, schemaResolver).ConfigureAwait(false);
            }
                
            /*
            //this swagger.json will be wrong (paths and controller is not detected)
            var controllers = dtoAsm.Assembly.GetTypes()
                .Where(x => x.Name.EndsWith("Controller"));

            foreach (var controller in controllers)
            {
                await generator.GenerateAsync(controller, schemaResolver).ConfigureAwait(false);
            }
            */
        }
        //File.WriteAllText(Path.Combine(outputPath, "swagger.json"), document.ToJson());
        
        var tsGenerator = new SwaggerToTypeScriptClientGenerator(document,
            new SwaggerToTypeScriptClientGeneratorSettings
            {
                ClassName = "{controller}Client",
            });
        var tsModels = tsGenerator.GenerateFile();
        return tsModels;
    }

/*
    Target TsGen => _ => _
        .DependsOn(TsClean)
        .DependsOn(CompileDotNet)
        .Executes(() =>
        {
            Console.WriteLine("Run custom target - TsGen!");

            var generator = new TypeScriptFluent().WithConvertor<Guid>(c => "string");

            var loadedAssemblies = LoadDtoAssemblies();
            foreach (var dtoAsm in loadedAssemblies.Dto)
            {
                var models = dtoAsm.Assembly.GetTypes()
                    .Where(x => x.Namespace.StartsWith(dtoAsm.ProjectName + ".Public"));

                foreach (var model in models)
                {
                    generator.ModelBuilder.Add(model);
                }
            }

            //Generate enums
            var tsEnumDefinitions = generator.Generate(TsGeneratorOutput.Enums);
            //Generate interface definitions for all classes
            var tsClassDefinitions = generator.Generate(TsGeneratorOutput.Properties | TsGeneratorOutput.Fields);

            foreach (var myTsModelsDirectory in MyTsModelsDirectories)
            {
                var outputPath = myTsModelsDirectory;

                Directory.CreateDirectory(outputPath);

                File.WriteAllText(Path.Combine(outputPath, "enums.ts"), tsEnumDefinitions);
                File.WriteAllText(Path.Combine(outputPath, "classes.d.ts"), tsClassDefinitions);
            }
           
        });
*/

    Target Ng => _ => _
        .DependsOn(TsGen)
        .Executes(() =>
        {
            Console.WriteLine("Create Angular bundle");
            var task = ProcessTasks.StartProcess("ng", " build --prod --aot", AngularSrcDir);
            task.WaitForExit();
            Console.Write(task.Output);
        });
}