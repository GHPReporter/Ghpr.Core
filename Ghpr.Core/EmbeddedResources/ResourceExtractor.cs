using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ghpr.Core.Enums;
using Ghpr.Core.Helpers;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Utils;

namespace Ghpr.Core.EmbeddedResources
{
    public class ResourceExtractor
    {
        private static readonly IEmbeddedResource[] AllResources = {
            new EmbeddedResource("ghpr.controller.js", ResourceType.GhprController, "src\\js",        "ghpr.controller.js", true ),
            new EmbeddedResource("plotly.min.js",      ResourceType.Plotly,         "src\\js",        "plotly.min.js",      true ),
            new EmbeddedResource("octicons.css",       ResourceType.Octicons,       "src\\octicons",  "octicons.css",       true ),
            new EmbeddedResource("octicons.eot",       ResourceType.Octicons,       "src\\octicons",  "octicons.eot",       true ),
            new EmbeddedResource("octicons.svg",       ResourceType.Octicons,       "src\\octicons",  "octicons.svg",       true ),
            new EmbeddedResource("octicons.ttf",       ResourceType.Octicons,       "src\\octicons",  "octicons.ttf",       true ),
            new EmbeddedResource("octicons.woff",      ResourceType.Octicons,       "src\\octicons",  "octicons.woff",      true ),
            new EmbeddedResource("github.css",         ResourceType.Github,         "src\\style",     "github.css",         true ),
            new EmbeddedResource("primer.css",         ResourceType.Primer,         "src\\style",     "primer.css",         true ),
            new EmbeddedResource("index.html",         ResourceType.TestPage,       "tests",          "tests.index.html",   true ),
            new EmbeddedResource("index.html",         ResourceType.TestRunPage,    "runs",           "runs.index.html",    true ),
            new EmbeddedResource("index.html",         ResourceType.MainPage,       "",               "Report.index.html",  true ),
            new EmbeddedResource("favicon.ico",        ResourceType.Favicon,        "src",            "favicon.ico",        true )
        };

        public ResourceExtractor(ActionHelper actionHelper, string outputPath, bool replaceExisting = false)
        {
            _actionHelper = actionHelper;
            OutputPath = outputPath;
            ReplaceExisting = replaceExisting;
        }

        private readonly ActionHelper _actionHelper;

        public string OutputPath { get; }
        public bool ReplaceExisting { get; }
        
        private void ExtractResource(string searchQuery, string outputPath, string relativePath, string fileName, bool replaceExisting)
        {
            _actionHelper.Safe(() =>
            {
                var currentAssembly = GetType().Assembly;
                var arrResources = currentAssembly.GetManifestResourceNames();
                var destinationPath = relativePath.Equals("") ? outputPath : Path.Combine(outputPath, relativePath);
                Paths.Create(destinationPath);

                var destinationFullPath = relativePath.Equals("") ? Path.Combine(outputPath, fileName) : Path.Combine(outputPath, relativePath, fileName);

                if (File.Exists(destinationFullPath) && !replaceExisting) return;

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
            });
        }

        private void ExtractResources(ResourceType type, string outputPath, bool replaceExisting)
        {
            var ress = AllResources.Where(r => r.Type == type);
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
                ResourceType.Plotly,
                ResourceType.Octicons,
                ResourceType.Github,
                ResourceType.Primer,
                ResourceType.MainPage,
                ResourceType.TestRunPage,
                ResourceType.TestPage,
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