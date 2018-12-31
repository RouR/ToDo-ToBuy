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
        public string CodeVer { get; } = "ver-2.0.1.ccc518663809464c5cb2be576489c65689c6de1b";

        #endregion
    }
}