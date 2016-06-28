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
                run.Name = $"Run {run.RunInfo.Start.ToString("yy-MM-dd hh:mm:ss")} - {run.RunInfo.Finish.ToString("yy-MM-dd hh:mm:ss")}";
            }
            if (fileName.Equals(""))
            {
                fileName = $"run_{run.RunInfo.Guid.ToString().ToLower()}.json";
            }
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var fullRunPath = Path.Combine(path, fileName);
            using (var file = File.CreateText(fullRunPath))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(file, run);
            }
        }
    }
}