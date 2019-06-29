using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ghpr.Core.Common;
using Ghpr.Core.Comparers;
using Ghpr.Core.Extensions;
using Ghpr.LocalFileSystem.Interfaces;
using Newtonsoft.Json;

namespace Ghpr.LocalFileSystem.Extensions
{
    public static class ItemInfoExtensions
    {
        public static string SaveRunInfo(this ItemInfoDto runInfo, ILocationsProvider locationsProvider)
        {
            return runInfo.SaveItemInfo(locationsProvider.RunsFolderPath, locationsProvider.Paths.File.Runs, true);
        }

        public static string SaveTestInfo(this ItemInfoDto testInfo, ILocationsProvider locationsProvider)
        {
            return testInfo.SaveItemInfo(locationsProvider.GetTestFolderPath(testInfo.Guid), locationsProvider.Paths.File.Tests, false);
        }

        public static string SaveItemInfo(this ItemInfoDto itemInfo, string path, string filename, bool removeExisting)
        {
            var serializer = new JsonSerializer();
            path.Create();
            var fullItemInfoPath = Path.Combine(path, filename);
            if (!File.Exists(fullItemInfoPath))
            {
                using (var file = File.CreateText(fullItemInfoPath))
                {
                    serializer.Serialize(file, new List<ItemInfoDto>(1) { itemInfo });
                }
            }
            else
            {
                List<ItemInfoDto> existingItems;
                using (var file = File.OpenText(fullItemInfoPath))
                {
                    existingItems = (List<ItemInfoDto>)serializer.Deserialize(file, typeof(List<ItemInfoDto>));
                }
                var itemsToSave = new List<ItemInfoDto>(existingItems.Count);
                existingItems.ForEach(i => { itemsToSave.Add(i); });

                if (removeExisting && itemsToSave.Any(i => i.Guid.Equals(itemInfo.Guid)))
                {
                    itemsToSave.RemoveAll(i => i.Guid.Equals(itemInfo.Guid));
                }
                if (!itemsToSave.Contains(itemInfo, new ItemInfoDtoComparer()))
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

        public static List<ItemInfoDto> LoadItemInfos(this string path, string filename)
        {
            List<ItemInfoDto> existingItems;
            var fullItemInfoPath = Path.Combine(path, filename);
            using (var file = File.OpenText(fullItemInfoPath))
            {
                var serializer = new JsonSerializer();
                existingItems = (List<ItemInfoDto>)serializer.Deserialize(file, typeof(List<ItemInfoDto>));
            }
            return existingItems;
        }

        public static void DeleteItemsFromItemInfosFile(this string path, string filename, List<ItemInfoDto> itemsToDelete)
        {
            var serializer = new JsonSerializer();
            List<ItemInfoDto> existingItems;
            var fullItemInfoPath = Path.Combine(path, filename);
            using (var file = File.OpenText(fullItemInfoPath))
            {
                existingItems = (List<ItemInfoDto>)serializer.Deserialize(file, typeof(List<ItemInfoDto>));
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