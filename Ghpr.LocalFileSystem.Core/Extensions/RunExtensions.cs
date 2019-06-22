using System.IO;
using Ghpr.LocalFileSystem.Core.Entities;
using Newtonsoft.Json;

namespace Ghpr.LocalFileSystem.Core.Extensions
{
    public static class RunExtensions
    {
        public static Run LoadRun(this string path, string fileName)
        {
            Run run = null;
            var fullRunPath = Path.Combine(path, fileName);
            if (File.Exists(fullRunPath))
            {
                using (var file = File.OpenText(fullRunPath))
                {
                    var serializer = new JsonSerializer();
                    run = (Run)serializer.Deserialize(file, typeof(Run));
                }
            }
            return run;
        }
    }
}