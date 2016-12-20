using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ghpr.Core.Common;
using Ghpr.Core.Comparers;
using Ghpr.Core.Utils;
using Newtonsoft.Json;

namespace Ghpr.Core.Helpers
{
    public static class ItemInfoHelper
    {
        public static void SaveItemInfo(string path, string filename, ItemInfo itemInfo, bool removeExisting = true)
        {
            var serializer = new JsonSerializer();
            Paths.Create(path);
            var fullItemInfoPath = Path.Combine(path, filename);
            if (!File.Exists(fullItemInfoPath))
            {
                var items = new List<ItemInfo>
                {
                    itemInfo
                };
                using (var file = File.CreateText(fullItemInfoPath))
                {
                    serializer.Serialize(file, items);
                }
            }
            else
            {
                List<ItemInfo> items;
                using (var file = File.OpenText(fullItemInfoPath))
                {
                    items = (List<ItemInfo>)serializer.Deserialize(file, typeof(List<ItemInfo>));
                    if (removeExisting && items.Any(i => i.Guid.Equals(itemInfo.Guid)))
                    {
                        items.RemoveAll(i => i.Guid.Equals(itemInfo.Guid));
                    }
                    if (!items.Contains(itemInfo, new ItemInfoComparer()))
                    {
                        items.Add(itemInfo);
                    }
                }
                using (var file = File.CreateText(fullItemInfoPath))
                {
                    items = items.OrderByDescending(x => x.Start).ToList();
                    serializer.Serialize(file, items);
                }
            }
        }
    }
}