using System.IO;
using Ghpr.Core.Interfaces;
using Newtonsoft.Json;

namespace Ghpr.Core.Extensions
{
    public static class RunExtensions
    {
        public static void Save(this IRun run, string path, string name = "")
        {
            if (name.Equals(""))
            {
                name = $"run_{run.Guid.ToString().ToLower()}.json";
            }
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var fullPath = Path.Combine(path, name);
            using (var file = File.CreateText(fullPath))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(file, run);
            }
        }
    }
}