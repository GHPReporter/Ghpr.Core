using Ghpr.Core.Common;
using Ghpr.Core.Utils;

namespace Ghpr.Core.Helpers
{
    public static class RunsHelper
    {
        public static void SaveCurrentRunInfo(string path, ItemInfo runInfo)
        {
            ItemInfoHelper.SaveItemInfo(path, Paths.Files.Runs, runInfo);
        }
    }
}