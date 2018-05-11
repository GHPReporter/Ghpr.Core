using System.IO;

namespace Ghpr.Core.Extensions
{
    public static class StringExtensions
    {
        public static void Create(this string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}