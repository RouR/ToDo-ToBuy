using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using k8s;
using k8s.Models;
using Microsoft.AspNetCore.JsonPatch;

//using Marvin.JsonPatch;

//Install-Package KubernetesClient -Version 0.3.0-beta

namespace DockerHubChecker
{
    class Program
    {
        private const string StagingTagName = "staging";
        const string EnvName = "KUBERNETES_NAMESPACE";

        private static string DetectNamespace()
        {
            
            var envHost = Environment.GetEnvironmentVariable(EnvName);
            if(string.IsNullOrWhiteSpace(envHost))
                throw new ArgumentNullException(nameof(envHost));
            return envHost;
            //return new Regex("kubernetes.(?<name>.*).svc.cluster.local").Match(envHost).Groups["name"].Value;
        }


        private static void CheckNamespace(string k8Namespace)
        {
#if false
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
#else
            var config = KubernetesClientConfiguration.InClusterConfig(); //for docker image, both on Debug and in Release versions
#endif

            IKubernetes client = new Kubernetes(config);
            Console.WriteLine("Starting Request to Kubernetes!");

            var list = client.ListNamespacedPod(k8Namespace);
            foreach (var item in list.Items)
            {
                Console.WriteLine($"Pod {item.Metadata.Name}");
            }
            if (list.Items.Count == 0)
            {
                Console.WriteLine("No Pods!");
            }


            Console.WriteLine("\nList Deployments:");
            foreach (var item in client.ListNamespacedDeployment(k8Namespace).Items)
            {
                Console.WriteLine($"*\t Name {item.Metadata.Name}");
                foreach (var label in item.Spec.Template.Metadata.Labels)
                {
                    Console.WriteLine($"Label {label.Key} = {label.Value}");
                }

                var ciLabel = item.Spec.Template.Metadata.Labels.SingleOrDefault(x => x.Key == "ci");
                if (ciLabel.Key != null)
                {
                    Console.WriteLine($"\t\t\tFound!");
                    var images = item.Spec.Template.Spec.Containers.Select(x => x.Image);
                    Console.WriteLine($"\t\t\tImages = {string.Join("; ", images)}");

                    // await Task.Factory.StartNew<Tag>(clientDocker.GetTags(x[0],x[1]))
                    var clientDocker = new DockerHubClient();

                    var tasks = images
                        .Select(x=> x.Split('/', '\\', ':'))
                        .Select(async (x) => clientDocker.GetTags(x[0], x[1]).SingleOrDefault(y=> y.Name == x[2]));

                    var result = Task.WhenAll(tasks);

                    var newTime = result.Result.Max(x => x.LastUpdated);
                    var newTag = newTime.ToString("O").Replace(".", "").Replace(":", "").Replace("-", ""); //some symbols is not allowed;

                    if (ciLabel.Value != newTag)
                    {
                        Console.WriteLine($"change from {ciLabel.Value} to {newTag}");
                        var newlables = new Dictionary<string, string>(item.Spec.Template.Metadata.Labels)
                        {
                            [ciLabel.Key] = newTag
                        };
                        var patch = new JsonPatchDocument<Appsv1beta1Deployment>();
                        patch.Replace(e => e.Spec.Template.Metadata.Labels, newlables);
                        client.PatchNamespacedDeployment(patch, item.Metadata.Name, k8Namespace);
                    }
                    
                }

                foreach (var container in item.Spec.Template.Spec.Containers)
                {
                    Console.WriteLine($"Image {container.Image}");
                }
            }
        }


        private static void Check()
        {
            var clientDocker = new DockerHubClient();

            //var repos = clientDocker.GetRepos("roured");
            var tags = clientDocker.GetTags("roured","tdtb-web");

            var tagStaging = tags.SingleOrDefault(x => x.Name == StagingTagName);

            if(tagStaging != null)
                Console.WriteLine($"{StagingTagName}: \tId={tagStaging.Id} \tLastUpdated={tagStaging.LastUpdated:R}");


            KubernetesClientConfiguration config = KubernetesClientConfiguration.InClusterConfig();
            //KubernetesClientConfiguration config = KubernetesClientConfiguration.InClusterConfig();

            IKubernetes client = new Kubernetes(config);
            Console.WriteLine("Starting Request!");

            var list = client.ListNamespacedPod("default");
            foreach (var item in list.Items)
            {
                Console.WriteLine(item.Metadata.Name);
            }
            if (list.Items.Count == 0)
            {
                Console.WriteLine("Empty!");
            }

            Console.WriteLine("ListNamespacedDeployment!");
            foreach (var item in client.ListNamespacedDeployment("default").Items)
            {
                Console.WriteLine($"Name {item.Metadata.Name}");
                foreach (var label in item.Spec.Template.Metadata.Labels)
                {
                    Console.WriteLine($"Label {label.Key} = {label.Value}");
                    if (label.Key == "ci")
                    {

                        var newVal = "newdata";
                        Console.WriteLine($"\t\t\tFound! try set to {newVal}");
                        item.Spec.Template.Metadata.Labels.Add("ci2", "newVal");
                        client.PatchNamespacedDeployment(item, item.Metadata.Name, "default");
                        //client.PatchNamespacedDeployment(new
                        //{
                        //    op = "replace",
                        //    path = "/spec/template/metadata/labels/"+ label.Key,
                        //    value = newVal
                        //},item.Metadata.Name, "default");

                    }
                }
                foreach (var container in item.Spec.Template.Spec.Containers)
                {
                    Console.WriteLine($"Image {container.Image}");
                }
            }

            var d = 0;
        }
      

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World! ");

            //Check();

            var k8Namespace = DetectNamespace();
            Console.WriteLine($"k8l Namespace {k8Namespace}  from Environment {EnvName} {Environment.GetEnvironmentVariable(EnvName)}");

            CheckNamespace(k8Namespace);


            Console.WriteLine("------------------------------------------------------------");
            Thread.Sleep(TimeSpan.FromSeconds(20));
            Console.WriteLine("------------------------------------------------------------");
            Thread.Sleep(TimeSpan.FromSeconds(20));
            Console.WriteLine("------------------------------------------------------------");
            //Check();
        }
    }
}
