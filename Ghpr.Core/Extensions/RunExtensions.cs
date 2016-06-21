using System.IO;
using Ghpr.Core.Interfaces;
using Newtonsoft.Json;

namespace Ghpr.Core.Extensions
{
    public static class RunExtensions
    {
        public static void Save(this IRun run, string path, string fileName = "")
        {
            if (run.Name.Equals(""))
            {
                run.Name = $"Run {run.Start.ToString("s")} - {run.Finish.ToString("s")}";
            }
            if (fileName.Equals(""))
            {
                fileName = $"run_{run.Guid.ToString().ToLower()}.json";
            }
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var fullPath = Path.Combine(path, fileName);
            using (var file = File.CreateText(fullPath))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(file, run);
            }
        }
    }
}