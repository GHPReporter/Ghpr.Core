using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ghpr.Core.Enums;

namespace Ghpr.Core.EmbeddedResources
{
    public class ResourceExtractor
    {
        public ResourceExtractor(string destPathFull = "", string destPathRelative = "", bool replaceExisting = false,
            Resource[] resources = null)
        {
            DestinationPathFull = destPathFull;
            DestinationPathRelative = destPathRelative;
            ReplaceExisting = replaceExisting;
            CurrentResources = resources;
        }

        public string DestinationPathFull { get; }
        public string DestinationPathRelative { get; }
        public bool ReplaceExisting { get; private set; }
        public Resource[] CurrentResources { get; private set; }

        public void Extract(Resource resource, string destinationPath = "", bool replaceExisting = false)
        {
            if (destinationPath.Equals(""))
            {
                destinationPath = DestinationPathFull;
            }

            ExtractResources(GetNames(resource), destinationPath, replaceExisting);
        }

        public void Extract(Resource[] resources, string destinationPath = "", bool replaceExisting = false)
        {
            if (destinationPath.Equals(""))
            {
                destinationPath = DestinationPathFull;
            }
            foreach (var resource in resources)
            {
                ExtractResources(GetNames(resource), destinationPath, replaceExisting);
            }
        }

        private void ExtractResource(string embeddedFileName, string destinationPath, bool replaceExisting)
        {
            var currentAssembly = GetType().Assembly;
            var arrResources = currentAssembly.GetManifestResourceNames();
            Directory.CreateDirectory(destinationPath);
            var destinationFullPath = Path.Combine(destinationPath, embeddedFileName);

            if (File.Exists(destinationFullPath) && !replaceExisting) return;

            foreach (
                var resourceName in
                    arrResources.Where(resourceName => resourceName.ToUpper().EndsWith(embeddedFileName.ToUpper())))
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

        private void ExtractResources(IEnumerable<string> embeddedFileNames, string destinationPath,
            bool replaceExisting)
        {
            foreach (var embeddedFileName in embeddedFileNames)
            {
                ExtractResource(embeddedFileName, destinationPath, replaceExisting);
            }
        }

        public static List<string> GetNames(Resource resource)
        {
            switch (resource)
            {
                case Resource.JQuery:
                    return new List<string>
                    {
                        "jquery-1.11.0.min.js"
                    };

                case Resource.Octicons:
                    return new List<string>
                    {
                        "octicons.css",
                        "octicons.eot",
                        "octicons.svg",
                        "octicons.ttf",
                        "octicons.woff"
                    };
                case Resource.Primer:
                    return new List<string>
                    {
                        "primer.css"
                    };
                case Resource.Github:
                    return new List<string>
                    {
                        "github.css"
                    };
                case Resource.Tablesort:
                    return new List<string>
                    {
                        "tablesort.min.js"
                    };
                case Resource.All:
                    return new List<string>
                    {
                        "tablesort.min.js",
                        "jquery-1.11.0.min.js",
                        "octicons.css",
                        "octicons.eot",
                        "octicons.svg",
                        "octicons.ttf",
                        "octicons.woff",
                        "primer.css",
                        "github.css"
                    };
                case Resource.TestPage:
                    return new List<string>
                    {
                        "Test.index.html"
                    };
                case Resource.TestRunPage:
                    return new List<string>
                    {
                        "TestRun.index.html"
                    };
                case Resource.TestRunsPage:
                    return new List<string>
                    {
                        "TestRuns.index.html"
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(resource), resource, null);
            }
        }

        public List<string> GetResoucrePaths(Resource resource, string resourceExtension = "")
        {
            if (resourceExtension.Equals(""))
            {
                return GetNames(resource).Select(name => Path.Combine(
                    DestinationPathRelative.Equals("")
                        ? DestinationPathFull
                        : DestinationPathRelative, name)).ToList();
            }

            return GetNames(resource)
                .Where(n => n.ToLower().EndsWith(resourceExtension.ToLower()))
                .Select(name => Path.Combine(
                    DestinationPathRelative.Equals("")
                        ? DestinationPathFull
                        : DestinationPathRelative, name)).ToList();
        }

        public List<string> GetResoucresPaths(Resource[] resources, string resourceExtension = "")
        {
            var paths = new List<string>();
            foreach (var resource in resources)
            {
                paths.AddRange(GetResoucrePaths(resource, resourceExtension));
            }
            return paths;
        }
    }
}