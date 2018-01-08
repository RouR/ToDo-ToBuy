using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using k8s;


namespace DockerHubChecker
{
    class Program
    {
        private const string StagingTagName = "staging";
        const string EnvName = "KUBERNETES_NAMESPACE";

        private static string DetectNamespace()
        {
            
            var envHost = Environment.GetEnvironmentVariable(EnvName);
            return envHost;
            //return new Regex("kubernetes.(?<name>.*).svc.cluster.local").Match(envHost).Groups["name"].Value;
        }


        private static void CheckNamespace(string k8Namespace)
        {
            var config = KubernetesClientConfiguration.InClusterConfig();
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
