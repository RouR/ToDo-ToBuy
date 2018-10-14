using System.Collections.Generic;

class ValuesTemplating
{
    public string DefaultNamespace { get; set; }
    public string[] Namespaces { get; set; }
    /// <summary>
    /// [name, [namespace,valueForNamespace]
    /// </summary>
    public Dictionary<string,Dictionary<string, string>> Values { get; set; }
}