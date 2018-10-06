using System.Linq;
using Nuke.Common.Utilities;

class CustomVersion
{
    byte Major = 0;
    byte Minor = 0;
    byte Build = 0;
    string Sha = string.Empty;

    public CustomVersion(string version)
    {
        var parts = version.Split(".");

        if (parts.Length > 0)
            Major = byte.Parse(parts[0]);
        if (parts.Length > 1)
            Minor = byte.Parse(parts[1]);
        if (parts.Length > 2)
            Build = byte.Parse(parts[2]);
        if (parts.Length > 3)
            Sha = parts[3];
    }

    public CustomVersion Copy()
    {
        return new CustomVersion(this.ToString());
    }

    public void IncreaseMajor()
    {
        Major++;
        Minor = 0;
        Build = 0;
    }

    public void IncreaseMinor()
    {
        Minor++;
        Build = 0;
    }
    public void IncreaseBuild()
    {
        Build++;
    }

    public void SetSha(string sha)
    {
        Sha = sha;
    }

    public override string ToString()
    {
        return new[] {Major.ToString(), Minor.ToString(), Build.ToString(), Sha}.Where(x=> !string.IsNullOrEmpty(x)).Join(".");
    }
    
    public string ToFileVersion()
    {
        return new[] {Major.ToString(), Minor.ToString(), Build.ToString()}.Join(".");
    }
    
    public string ToAssemblyVersion()
    {
        return new[] {Major.ToString(), Minor.ToString(), Build.ToString()}.Join(".");
    }

    public string ToGitTag()
    {
        return new[] {Major.ToString(), Minor.ToString(), Build.ToString()}.Join(".");
    }
    
    public string ToDockerTag()
    {
        return new[] {Major.ToString(), Minor.ToString(), Build.ToString()}.Join(".");
    }
}