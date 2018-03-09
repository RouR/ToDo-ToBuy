﻿namespace k8s.KubeConfigModels
{
    using YamlDotNet.Serialization;

    public class User
    {
        [YamlMember(Alias = "user")]
        public UserCredentials UserCredentials { get; set; }

        [YamlMember(Alias = "name")]
        public string Name { get; set; }
    }
}
