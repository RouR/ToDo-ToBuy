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
        public string CodeVer { get; } = "ver-0.7.4.2593299fb89366a244551b6803698d17db97619b";

        #endregion
    }
}