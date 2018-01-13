using Marvin.JsonPatch.Exceptions;
using Marvin.JsonPatch.NetStandard.Properties;

namespace Marvin.JsonPatch.Internal
{
    internal static class PathHelpers
    {
        internal static string NormalizePath(string path)
        {
            // check for most common path errors on create.  This is not
            // absolutely necessary, but it allows us to already catch mistakes
            // on creation of the patch document rather than on execute.

            if (path.Contains(".") || path.Contains("//") || path.Contains(" ") || path.Contains("\\"))
            {
                throw new JsonPatchException(string.Format(Resources.InvalidValueForPath, path), null);
            }

            if (!(path.StartsWith("/")))
            {
                return "/" + path;
            }
            else
            {
                return path;
            }
        }
    }
}
