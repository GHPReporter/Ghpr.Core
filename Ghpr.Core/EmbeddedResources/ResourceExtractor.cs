using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ghpr.Core.Enums;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Utils;

namespace Ghpr.Core.EmbeddedResources
{
    public class ResourceExtractor
    {
        private static readonly IEmbeddedResource[] All = {
            new EmbeddedResource("jquery-1.11.0.min.js", ResourceType.JQuery, "src\\js"),
            new EmbeddedResource("ghpr.controller.js", ResourceType.GhprController, "src\\js"),
            new EmbeddedResource("tablesort.min.js", ResourceType.Tablesort, "src\\js"),
            new EmbeddedResource("plotly.min.js", ResourceType.Plotly, "src\\js"),
            new EmbeddedResource("octicons.css", ResourceType.Octicons, "src\\octicons"),
            new EmbeddedResource("octicons.eot", ResourceType.Octicons, "src\\octicons"),
            new EmbeddedResource("octicons.svg", ResourceType.Octicons, "src\\octicons"),
            new EmbeddedResource("octicons.ttf", ResourceType.Octicons, "src\\octicons"),
            new EmbeddedResource("octicons.woff", ResourceType.Octicons, "src\\octicons"),
            new EmbeddedResource("github.css", ResourceType.Github, "src\\style"),
            new EmbeddedResource("primer.css", ResourceType.Primer, "src\\style"),
            new EmbeddedResource("index.html", ResourceType.TestPage, "", "tests.index.html"),
            new EmbeddedResource("index.html", ResourceType.TestRunPage, "runs", "runs.index.html"),
            new EmbeddedResource("index.html", ResourceType.TestRunsPage, "", "Report.index.html"),
            new EmbeddedResource("favicon.ico", ResourceType.Favicon, "src")
        };

        public ResourceExtractor(string outputPath = "", bool replaceExisting = false)
        {
            OutputPath = outputPath;
            ReplaceExisting = replaceExisting;
        }

        public string OutputPath { get; }
        public bool ReplaceExisting { get; private set; }
        
        private bool ExtractResource(string searchQuery, string outputPath, string relativePath, string fileName, bool replaceExisting)
        {
            try
            {
                var currentAssembly = GetType().Assembly;
                var arrResources = currentAssembly.GetManifestResourceNames();

                var destinationPath = relativePath.Equals("") ? outputPath : Path.Combine(outputPath, relativePath);
                Directory.CreateDirectory(destinationPath);

                var destinationFullPath = relativePath.Equals("") ? Path.Combine(outputPath, fileName) : Path.Combine(outputPath, relativePath, fileName);

                if (File.Exists(destinationFullPath) && !replaceExisting) return true;

                foreach (
                    var resourceName in
                        arrResources.Where(resourceName => resourceName.ToUpper().Contains(searchQuery.ToUpper())))
                {
                    using (var resourceToSave = currentAssembly.GetManifestResourceStream(resourceName))
                    {
                        using (var output = File.Create(destinationFullPath))
                        {
                            resourceToSave?.CopyTo(output);
                        }
                        resourceToSave?.Close();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Exception(ex, "Exception while extracting resource file!");
                return false;
            }
        }

        private void ExtractResources(ResourceType type, string outputPath, bool replaceExisting)
        {
            var ress = All.Where(r => r.Type == type);
            foreach (var res in ress)
            {
                ExtractResource(res.SearchQuery, outputPath, res.RelativePath, res.FileName, replaceExisting);
            }
        }

        private void ExtractResources(IEnumerable<ResourceType> types, string outputPath, bool replaceExisting)
        {
            foreach (var type in types)
            {
                ExtractResources(type, outputPath, replaceExisting);
            }
        }

        public void ExtractReportBase()
        {
            var types = new[]
            {
                ResourceType.GhprController,
                ResourceType.JQuery,
                ResourceType.Tablesort,
                ResourceType.Plotly,
                ResourceType.Octicons,
                ResourceType.Github,
                ResourceType.Primer,
                ResourceType.TestRunsPage,
                ResourceType.TestRunPage,
                ResourceType.Favicon
            };
            ExtractResources(types, OutputPath, ReplaceExisting);
        }

        public void ExtractTestPage(string testPath)
        {
            ExtractResources(ResourceType.TestPage, testPath, ReplaceExisting);
        }
    }
}