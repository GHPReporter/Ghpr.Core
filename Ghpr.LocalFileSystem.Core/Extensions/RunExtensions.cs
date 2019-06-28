using System.IO;
using Ghpr.Core.Common;
using Newtonsoft.Json;

namespace Ghpr.LocalFileSystem.Extensions
{
    public static class RunExtensions
    {
        public static RunDto LoadRun(this string path, string fileName)
        {
            RunDto run = null;
            var fullRunPath = Path.Combine(path, fileName);
            if (File.Exists(fullRunPath))
            {
                using (var file = File.OpenText(fullRunPath))
                {
                    var serializer = new JsonSerializer();
                    run = (RunDto)serializer.Deserialize(file, typeof(RunDto));
                }
            }
            return run;
        }
    }
}