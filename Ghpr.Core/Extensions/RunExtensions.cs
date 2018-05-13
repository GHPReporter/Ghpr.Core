using System.IO;
using Ghpr.Core.Common;
using Ghpr.Core.Interfaces;
using Newtonsoft.Json;

namespace Ghpr.Core.Extensions
{
    public static class RunExtensions
    {
        public static void Save(this IRun run, string path, string fileName = "")
        {
            if (fileName.Equals(""))
            {
                fileName = $"run_{run.RunInfo.Guid.ToString().ToLower()}.json";
            }
            run.RunInfo.FileName = fileName;
            path.Create();
            var fullRunPath = Path.Combine(path, fileName);
            using (var file = File.CreateText(fullRunPath))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(file, run);
            }
        }

        public static IRun LoadRun(this string path, string fileName)
        {
            IRun run;
            var fullRunPath = Path.Combine(path, fileName);
            using (var file = File.OpenText(fullRunPath))
            {
                var serializer = new JsonSerializer();
                run = (IRun)serializer.Deserialize(file, typeof(Run));
            }
            return run;
        }
    }
}