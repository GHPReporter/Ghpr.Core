using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ghpr.Core.Core.Enums;
using Ghpr.Core.Core.Extensions;
using Ghpr.Core.Core.Interfaces;

namespace Ghpr.Core.Core.EmbeddedResources
{
    public static class ResourceExtractor
    {
        private static readonly IEmbeddedResource[] AllResources = {
            new EmbeddedResource("ghpr.controller.js", ResourceType.GhprController, "src\\js",        "ghpr.controller.js", true ),
            new EmbeddedResource("plotly.min.js",      ResourceType.Plotly,         "src\\js",        "plotly.min.js",      true ),
            new EmbeddedResource("",                   ResourceType.Octicons,       "src\\octicons",  "octicons",           true ),
            new EmbeddedResource("ghpr.css",           ResourceType.GhprCss,        "src\\style",     "ghpr.css",           true ),
            new EmbeddedResource("build.css",          ResourceType.Primer,         "src\\style",     "build.css",          true ),
            new EmbeddedResource("index.html",         ResourceType.TestPage,       "tests",          "tests.index.html",   true ),
            new EmbeddedResource("index.html",         ResourceType.TestRunPage,    "runs",           "runs.index.html",    true ),
            new EmbeddedResource("index.html",         ResourceType.MainPage,       "",               "Report.index.html",  true ),
            new EmbeddedResource("logo.svg",           ResourceType.GhprLogo,       "src",            "logo.svg",           true ),
            new EmbeddedResource("favicon.ico",        ResourceType.Favicon,        "src",            "favicon.ico",        true )
        };

        private static void ExtractResource(IEmbeddedResource res, string outputPath)
        {
            var currentAssembly = typeof(ResourceExtractor).Assembly;
            var arrResources = currentAssembly.GetManifestResourceNames();
            var destinationPath = res.RelativePath.Equals("") ? outputPath : Path.Combine(outputPath, res.RelativePath);
            destinationPath.Create();

            foreach (
                var resourceName in
                    arrResources.Where(resourceName => resourceName.ToUpper().Contains(res.SearchQuery.ToUpper())))
            {
                var resInfo = currentAssembly.GetManifestResourceInfo(resourceName);
                var resSplit = resourceName.Split('.');
                var extraFileName = string.Join(".", resSplit.Skip(Math.Max(0, resSplit.Length - 2)));
                if (resInfo != null)
                {
                    var fileName = res.FileName.Equals("") ? resInfo.FileName ?? extraFileName : res.FileName;
                    var destinationFullPath = res.RelativePath.Equals("")
                        ? Path.Combine(outputPath, fileName)
                        : Path.Combine(outputPath, res.RelativePath, fileName);

                    if (File.Exists(destinationFullPath) && !res.AlwaysReplaceExisting) return;

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
                ResourceType.GhprCss,
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