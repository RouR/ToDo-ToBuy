using System;

namespace Web.Utils
{
    public class InstanceInfo
    {
        public InstanceInfo()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }
        public string CodeVer { get; } = "v2";
       
    }
}