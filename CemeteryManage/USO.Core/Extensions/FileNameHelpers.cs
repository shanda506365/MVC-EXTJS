
namespace USO.Core.Extensions
{
    using System.Linq;

    public static class FileNameHelpers
    {
        public static string GetFileName(this string fullPath)
        {
            if (string.IsNullOrWhiteSpace(fullPath))
            {
                return string.Empty;
            }
            string[] pathParts = fullPath.Split(@"\/".ToCharArray());

            if (string.IsNullOrEmpty(pathParts.Last()))
            {
                return string.Empty;
            }
            return pathParts.Last();
        }

        public static string GetFileExtension(this string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return string.Empty;
            }
            string[] fileParts = fileName.Split(".".ToCharArray());
            if (fileParts.Count() == 1 || string.IsNullOrEmpty(fileParts.Last()))
            {
                return string.Empty;
            }
            return fileParts.Last();
        }
    }
}
