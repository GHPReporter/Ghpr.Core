using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ghpr.Core.Extensions;
using Ghpr.LocalFileSystem.Core.Comparers;
using Ghpr.LocalFileSystem.Core.Entities;
using Ghpr.LocalFileSystem.Core.Interfaces;
using Newtonsoft.Json;

namespace Ghpr.LocalFileSystem.Core.Extensions
{
    public static class ItemInfoExtensions
    {
        public static string SaveRunInfo(this ItemInfo runInfo, ILocationsProvider locationsProvider)
        {
            return runInfo.SaveItemInfo(locationsProvider.RunsFolderPath, locationsProvider.Paths.File.Runs, true);
        }

        public static string SaveTestInfo(this ItemInfo testInfo, ILocationsProvider locationsProvider)
        {
            return testInfo.SaveItemInfo(locationsProvider.GetTestFolderPath(testInfo.Guid), locationsProvider.Paths.File.Tests, false);
        }

        public static string SaveItemInfo(this ItemInfo itemInfo, string path, string filename, bool removeExisting)
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

        public static List<ItemInfo> LoadItemInfos(this string path, string filename)
        {
            List<ItemInfo> existingItems;
            var fullItemInfoPath = Path.Combine(path, filename);
            using (var file = File.OpenText(fullItemInfoPath))
            {
                var serializer = new JsonSerializer();
                existingItems = (List<ItemInfo>)serializer.Deserialize(file, typeof(List<ItemInfo>));
            }
            return existingItems;
        }

        public static void DeleteItemsFromItemInfosFile(this string path, string filename, List<ItemInfo> itemsToDelete)
        {
            var serializer = new JsonSerializer();
            List<ItemInfo> existingItems;
            var fullItemInfoPath = Path.Combine(path, filename);
            using (var file = File.OpenText(fullItemInfoPath))
            {
                existingItems = (List<ItemInfo>)serializer.Deserialize(file, typeof(List<ItemInfo>));
            }
            existingItems.RemoveAll(i => itemsToDelete.Any(itd => itd.Guid == i.Guid && itd.Finish == i.Finish));
            using (var file = File.CreateText(fullItemInfoPath))
            {
                existingItems = existingItems.OrderByDescending(x => x.Start).ToList();
                serializer.Serialize(file, existingItems);
            }
        }
    }
}