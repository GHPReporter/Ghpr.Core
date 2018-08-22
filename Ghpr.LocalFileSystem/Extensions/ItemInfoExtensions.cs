using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ghpr.Core.Extensions;
using Ghpr.LocalFileSystem.Comparers;
using Ghpr.LocalFileSystem.Entities;
using Ghpr.LocalFileSystem.Interfaces;
using Newtonsoft.Json;

namespace Ghpr.LocalFileSystem.Extensions
{
    public static class ItemInfoExtensions
    {
        public static string SaveRunInfo(this ItemInfo runInfo, ILocationsProvider locationsProvider)
        {
            return runInfo.SaveItemInfo(locationsProvider.RunsPath, locationsProvider.Paths.File.Runs);
        }

        public static string SaveTestInfo(this ItemInfo testInfo, ILocationsProvider locationsProvider)
        {
            return testInfo.SaveItemInfo(locationsProvider.GetTestPath(testInfo.Guid), locationsProvider.Paths.File.Tests, false);
        }

        public static string SaveItemInfo(this ItemInfo itemInfo, string path, string filename, bool removeExisting = true)
        {
            var serializer = new JsonSerializer();
            path.Create();
            var fullItemInfoPath = Path.Combine(path, filename);
            if (!File.Exists(fullItemInfoPath))
            {
                using (var file = File.CreateText(fullItemInfoPath))
                {
                    serializer.Serialize(file, new List<ItemInfo>(1) { itemInfo });
                }
            }
            else
            {
                List<ItemInfo> existingItems;
                using (var file = File.OpenText(fullItemInfoPath))
                {
                    existingItems = (List<ItemInfo>)serializer.Deserialize(file, typeof(List<ItemInfo>));
                }
                var itemsToSave = new List<ItemInfo>(existingItems.Count);
                existingItems.ForEach(i => { itemsToSave.Add(i); });

                if (removeExisting && itemsToSave.Any(i => i.Guid.Equals(itemInfo.Guid)))
                {
                    itemsToSave.RemoveAll(i => i.Guid.Equals(itemInfo.Guid));
                }
                if (!itemsToSave.Contains(itemInfo, new ItemInfoComparer()))
                {
                    itemsToSave.Add(itemInfo);
                }
                using (var file = File.CreateText(fullItemInfoPath))
                {
                    itemsToSave = itemsToSave.OrderByDescending(x => x.Start).ToList();
                    serializer.Serialize(file, itemsToSave);
                }
            }
            return fullItemInfoPath;
        }

        public static List<ItemInfo> LoadItemInfos(this string path, string filename, bool removeExisting = true)
        {
            var serializer = new JsonSerializer();
            var fullItemInfoPath = Path.Combine(path, filename);
            List<ItemInfo> existingItems;
            using (var file = File.OpenText(fullItemInfoPath))
            {
                existingItems = (List<ItemInfo>) serializer.Deserialize(file, typeof(List<ItemInfo>));
            }
            return existingItems;        
        }
    }
}