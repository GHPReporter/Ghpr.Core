using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ghpr.Core.Enums;
using Ghpr.Core.Extensions;
using Ghpr.Core.Interfaces;

namespace Ghpr.Core.EmbeddedResources
{
    public static class ResourceExtractor
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

        private static void ExtractResource(IEmbeddedResource res, string outputPath)
        {
            var currentAssembly = typeof(ResourceExtractor).Assembly;
            var arrResources = currentAssembly.GetManifestResourceNames();
            var destinationPath = res.RelativePath.Equals("") ? outputPath : Path.Combine(outputPath, res.RelativePath);
            destinationPath.Create();

            var destinationFullPath = res.RelativePath.Equals("") ? Path.Combine(outputPath, res.FileName) : Path.Combine(outputPath, res.RelativePath, res.FileName);

            if (File.Exists(destinationFullPath) && !res.AlwaysReplaceExisting) return;

            foreach (
                var resourceName in
                    arrResources.Where(resourceName => resourceName.ToUpper().Contains(res.SearchQuery.ToUpper())))
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
        }

        private static void ExtractResources(ResourceType type, string outputPath)
        {
            var ress = AllResources.Where(r => r.Type == type);
            foreach (var res in ress)
            {
                ExtractResource(res, outputPath);
            }
        }

        private static void ExtractResources(IEnumerable<ResourceType> types, string outputPath)
        {
            foreach (var type in types)
            {
                ExtractResources(type, outputPath);
            }
        }

        public static void ExtractReportBase(string outputPath)
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
            ExtractResources(types, outputPath);
        }
    }
}