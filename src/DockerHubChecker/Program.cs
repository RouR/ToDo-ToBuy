using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using k8s;
using k8s.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Rest;

//Install-Package KubernetesClient -Version 0.6.0-beta

namespace DockerHubChecker
{
    class Program
    {
        private static void CheckNamespace(string k8Namespace, string k8Label)
        {
#if false
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
#else
            var config = KubernetesClientConfiguration.InClusterConfig(); //for docker image, both on Debug and in Release versions
#endif

            IKubernetes client = new Kubernetes(config);
            Console.WriteLine("Starting Request to Kubernetes!");

            ServiceClientTracing.IsEnabled = true;

            Console.WriteLine("\nList Deployments:");
            foreach (var item in client.ListNamespacedDeployment(k8Namespace).Items)
            {
                Console.WriteLine($"*\t Name {item.Metadata.Name}");
                foreach (var label in item.Spec.Template.Metadata.Labels)
                {
                    Console.WriteLine($"Label {label.Key} = {label.Value}");
                }

                var ciLabel = item.Spec.Template.Metadata.Labels.SingleOrDefault(x => x.Key == k8Label);
                if (ciLabel.Key != null)
                {
                    Console.WriteLine($"\t\t\tFound!");
                    var images = item.Spec.Template.Spec.Containers.Select(x => x.Image).ToArray();
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
                        client.PatchNamespacedDeployment(new V1Patch(patch), item.Metadata.Name, k8Namespace);
                    }
                    
                }
            }
        }


        static void Main(string[] args)
        {
            const string envNameNamespace = "KUBERNETES_NAMESPACE";
            const string envNameLabel = "LABEL";

            var k8Namespace = Environment.GetEnvironmentVariable(envNameNamespace);
            if (string.IsNullOrWhiteSpace(k8Namespace))
                throw new ArgumentNullException(envNameNamespace);

            var k8Label = Environment.GetEnvironmentVariable(envNameLabel);
            if (string.IsNullOrWhiteSpace(envNameLabel))
                throw new ArgumentNullException(envNameLabel);

            Console.WriteLine($"k8l Namespace {k8Namespace}");
            Console.WriteLine($"k8l Label {k8Label}");

            try
            {
                CheckNamespace(k8Namespace, k8Label);
            }
            catch (Microsoft.Rest.HttpOperationException ex)
            {
                var phase = ex.Response.ReasonPhrase;
                var content = ex.Response.Content;
                Console.WriteLine(ex.Message);
                Console.WriteLine(content);
                Console.WriteLine(phase);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

            

            Console.WriteLine("Finish");
        }
    }
}
