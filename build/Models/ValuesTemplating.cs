using System.Collections.Generic;

class ValuesTemplating
{
    public string DefaultNamespace { get; set; }
    /// <summary>
    /// [name, [namespace,valueForNamespace]
    /// </summary>
    public Dictionary<string,Dictionary<string, string>> Values { get; set; }
}