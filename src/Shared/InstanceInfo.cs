using System;
using System.Reflection;

namespace Shared
{
    public class InstanceInfo
    {
        public InstanceInfo()
        {
            ServiceName = Assembly.GetEntryAssembly().GetName().Name;
            ServiceVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }
        public string ServiceName { get; }
        public string ServiceVersion { get; }

        #region Autochanged, don`t touch

        // ReSharper disable once StringLiteralTypo
        public string CodeVer { get; } = "ver-0.10.0.712e473cc717305fb79cd67aae00ec698af6f61b";

        #endregion
    }
}