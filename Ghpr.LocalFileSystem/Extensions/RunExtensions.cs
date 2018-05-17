using System.IO;
using Ghpr.Core.Extensions;
using Ghpr.LocalFileSystem.Entities;
using Newtonsoft.Json;

namespace Ghpr.LocalFileSystem.Extensions
{
    public static class RunExtensions
    {
        public static void Save(this Run run, string path)
        {
            var fileName = $"run_{run.RunInfo.Guid.ToString().ToLower()}.json";
            run.RunInfo.FileName = fileName;
            path.Create();
            var fullRunPath = Path.Combine(path, fileName);
            using (var file = File.CreateText(fullRunPath))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(file, run);
            }
        }

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