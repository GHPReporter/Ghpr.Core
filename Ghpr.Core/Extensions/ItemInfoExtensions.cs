using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ghpr.Core.Common;
using Ghpr.Core.Comparers;
using Ghpr.Core.Utils;
using Newtonsoft.Json;

namespace Ghpr.Core.Extensions
{
    public static class ItemInfoExtensions
    {
        public static void SaveCurrentRunInfo(this ItemInfo runInfo, string path)
        {
            runInfo.SaveItemInfo(path, Paths.Files.Runs);
        }

        public static void SaveCurrentTestInfo(this ItemInfo testInfo, string path)
        {
            testInfo.SaveItemInfo(path, Paths.Files.Tests, false);
        }

        public static void SaveItemInfo(this ItemInfo itemInfo, string path, string filename, bool removeExisting = true)
        {
            var ii = new ItemInfo(itemInfo);
            var serializer = new JsonSerializer();
            Paths.Create(path);
            var fullItemInfoPath = Path.Combine(path, filename);
            if (!File.Exists(fullItemInfoPath))
            {
                var items = new List<ItemInfo>(1) { ii };
                using (var file = File.CreateText(fullItemInfoPath))
                {
                    serializer.Serialize(file, items);
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
                existingItems.ForEach(i => { itemsToSave.Add(new ItemInfo(i)); });

                if (removeExisting && itemsToSave.Any(i => i.Guid.Equals(ii.Guid)))
                {
                    itemsToSave.RemoveAll(i => i.Guid.Equals(ii.Guid));
                }
                if (!itemsToSave.Contains(ii, new ItemInfoComparer()))
                {
                    itemsToSave.Add(new ItemInfo(ii));
                }
                using (var file = File.CreateText(fullItemInfoPath))
                {
                    itemsToSave = itemsToSave.OrderByDescending(x => x.Start).ToList();
                    serializer.Serialize(file, itemsToSave);
                }
            }
        }

        public static List<ItemInfo> LoadItemInfos(this string path, string filename, bool removeExisting = true)
        {
            var serializer = new JsonSerializer();
            var fullItemInfoPath = Path.Combine(path, filename);
            var existingItems = new List<ItemInfo>();
            if (File.Exists(fullItemInfoPath))
            {
                using (var file = File.OpenText(fullItemInfoPath))
                {
                    existingItems = (List<ItemInfo>) serializer.Deserialize(file, typeof(List<ItemInfo>));
                }
            }
            return existingItems;
        
        }
    }
}