using System.IO;
using System.Linq;

namespace IdentityServer_Common.Common
{
    public static class VisualStudioProvider
    {
        public static DirectoryInfo TryGetSolutionDirectoryInfo(string currentPath = null!)
        {
            var directory = new DirectoryInfo(
                currentPath ?? Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            return directory!;
        }

        public static string TryGetSolutionDirectory(string currentPath = null!)
        {
            return TryGetSolutionDirectoryInfo(currentPath).FullName;
        }
    }
}
