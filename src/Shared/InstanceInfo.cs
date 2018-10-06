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
        public string CodeVer { get; } = "ver-0.2.377443b41c8f73d34926220101c0bf3b943fcced";

        #endregion
    }
}