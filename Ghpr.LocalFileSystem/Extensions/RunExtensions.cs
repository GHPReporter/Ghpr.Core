using System.IO;
using Ghpr.LocalFileSystem.Entities;
using Newtonsoft.Json;

namespace Ghpr.LocalFileSystem.Extensions
{
    public static class RunExtensions
    {
        public static Run LoadRun(this string path, string fileName)
        {
            Run run;
            var fullRunPath = Path.Combine(path, fileName);
            using (var file = File.OpenText(fullRunPath))
            {
                var serializer = new JsonSerializer();
                run = (Run)serializer.Deserialize(file, typeof(Run));
            }
            return run;
        }
    }
}