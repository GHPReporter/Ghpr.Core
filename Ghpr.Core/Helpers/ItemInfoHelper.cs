using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ghpr.Core.Common;
using Ghpr.Core.Interfaces;
using Newtonsoft.Json;

namespace Ghpr.Core.Helpers
{
    public static class ItemInfoHelper
    {
        public static void SaveItemInfo(string path, string filename, IItemInfo itemInfo, bool removeExisting = true)
        {
            var serializer = new JsonSerializer();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var fullRunsPath = Path.Combine(path, filename);
            if (!File.Exists(fullRunsPath))
            {
                var items = new List<IItemInfo>
                {
                    itemInfo
                };
                using (var file = File.CreateText(fullRunsPath))
                {
                    serializer.Serialize(file, items);
                }
            }
            else
            {
                List<ItemInfo> items;
                using (var file = File.OpenText(fullRunsPath))
                {
                    items = (List<ItemInfo>)serializer.Deserialize(file, typeof(List<ItemInfo>));
                    if (removeExisting && items.Any(i => i.Guid.Equals(itemInfo.Guid)))
                    {
                        items.Remove(items.First(i => i.Guid.Equals(itemInfo.Guid)));
                    }
                    items.Add(new ItemInfo(itemInfo));
                }
                using (var file = File.CreateText(fullRunsPath))
                {
                    items = items.OrderByDescending(x => x.Start).ToList();
                    serializer.Serialize(file, items);
                }
            }
        }
    }
}