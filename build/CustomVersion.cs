using System.Linq;
using Nuke.Common.Utilities;

class CustomVersion
{
    byte Major = 0;
    byte Minor = 0;
    string Sha = string.Empty;

    public CustomVersion(string version)
    {
        var parts = version.Split(".");

        if (parts.Length > 0)
            Major = byte.Parse(parts[0]);
        if (parts.Length > 1)
            Minor = byte.Parse(parts[1]);
        if (parts.Length > 2)
            Sha = parts[2];
    }

    public CustomVersion Copy()
    {
        return new CustomVersion(this.ToString());
    }

    public void IncreaseMajor()
    {
        Major++;
        Minor = 0;
    }

    public void IncreaseMinor()
    {
        Minor++;
    }

    public void SetSha(string sha)
    {
        Sha = sha;
    }

    public override string ToString()
    {
        return new[] {Major.ToString(), Minor.ToString(), Sha}.Where(x=> !string.IsNullOrEmpty(x)).Join(".");
    }
    
    public string ToFileVersion()
    {
        return new[] {Major.ToString(), Minor.ToString(), "0", "0"}.Join(".");
    }
    
    public string ToAssemblyVersion()
    {
        return ToString();
    }
}