using System.Collections.Generic;
using System.IO;
using Ghpr.Core.Common;
using Ghpr.Core.Interfaces;
using Newtonsoft.Json;

namespace Ghpr.Core.Helpers
{
    public static class RunsHelper
    {
        public static void SaveCurrentRunInfo(string path, IRunInfo runInfo)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var fullRunsPath = Path.Combine(path, "runs.json");
            if (!File.Exists(fullRunsPath))
            {
                var runs = new List<IRunInfo>
                {
                    runInfo
                };
                using (var file = File.CreateText(fullRunsPath))
                {
                    var serializer = new JsonSerializer();
                    serializer.Serialize(file, runs);
                }
            }
            else
            {
                List<RunInfo> runs;
                using (var file = File.OpenText(fullRunsPath))
                {
                    var serializer = new JsonSerializer();
                    runs = (List<RunInfo>)serializer.Deserialize(file, typeof(List<RunInfo>));
                    runs.Add(new RunInfo(runInfo));
                }
                using (var file = File.CreateText(fullRunsPath))
                {
                    var serializer = new JsonSerializer();
                    serializer.Serialize(file, runs);
                }
            }
        }
    }
}