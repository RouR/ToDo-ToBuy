using System;
<<<<<<< HEAD
using System.Linq;
using System.Threading;
using k8s;
using Newtonsoft.Json;
=======
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using k8s;
>>>>>>> CI

namespace DockerHubChecker
{
    class Program
    {
        private const string StagingTagName = "staging";

<<<<<<< HEAD
=======

        private static string DetectNamespace()
        {
            return null;
        }


        private static void CheckNamespace(string k8Namespace)
        {
            string environmentVariable1 = Environment.GetEnvironmentVariable("KUBERNETES_SERVICE_HOST");
        }

>>>>>>> CI
        private static void Check()
        {
            var clientDocker = new DockerHubClient();

            //var repos = clientDocker.GetRepos("roured");
            var tags = clientDocker.GetTags("roured","tdtb-web");

            var tagStaging = tags.SingleOrDefault(x => x.Name == StagingTagName);

            if(tagStaging != null)
                Console.WriteLine($"{StagingTagName}: \tId={tagStaging.Id} \tLastUpdated={tagStaging.LastUpdated:R}");

<<<<<<< HEAD
            var config = KubernetesClientConfiguration.InClusterConfig();
=======
            KubernetesClientConfiguration config = KubernetesClientConfiguration.InClusterConfig();
            //KubernetesClientConfiguration config = KubernetesClientConfiguration.InClusterConfig();
>>>>>>> CI
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
<<<<<<< HEAD
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
=======
                        Console.WriteLine($"\t\t\tFound!");
                        if (tagStaging != null)
                        {
                            var newVal = "timestamp"+tagStaging.LastUpdated.ToString("s");
                            Console.WriteLine($"\t\t\t try set to {newVal}");

                            //item.Spec.Template.Metadata.Labels.Add("ci2", "newVal");
                            //client.PatchNamespacedDeployment(item, item.Metadata.Name, "default");
                            client.PatchNamespacedDeployment(new
                            {
                                op = "replace",
                                path = "/spec/template/metadata/labels/" + label.Key,
                                value = newVal
                            }, item.Metadata.Name, "default");
                        }


>>>>>>> CI
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
<<<<<<< HEAD
=======
            Console.WriteLine(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), ".kube\\config") : Path.Combine(Environment.GetEnvironmentVariable("HOME"), ".kube/config"));
>>>>>>> CI

            Check();

            Console.WriteLine("------------------------------------------------------------");
            Thread.Sleep(TimeSpan.FromSeconds(20));
            Console.WriteLine("------------------------------------------------------------");
            Thread.Sleep(TimeSpan.FromSeconds(20));
            Console.WriteLine("------------------------------------------------------------");
            Check();
        }
    }
}
