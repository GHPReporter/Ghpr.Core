using System.IO;
using Ghpr.Core.Extensions;
using Ghpr.LocalFileSystem.Entities;
using Ghpr.LocalFileSystem.Providers;
using Newtonsoft.Json;

namespace Ghpr.LocalFileSystem.Extensions
{
    public static class RunExtensions
    {
        public static string Save(this Run run, string path)
        {
            var fileName = LocationsProvider.GetRunFileName(run.RunInfo.Guid);
            run.RunInfo.FileName = fileName;
            path.Create();
            var fullRunPath = Path.Combine(path, fileName);
            using (var file = File.CreateText(fullRunPath))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(file, run);
            }
            return fullRunPath;
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