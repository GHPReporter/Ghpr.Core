using Ghpr.Core.Common;

namespace Ghpr.Core.Helpers
{
    public static class RunsHelper
    {
        public static void SaveCurrentRunInfo(string path, ItemInfo runInfo)
        {
            ItemInfoHelper.SaveItemInfo(path, "runs.json", runInfo);
        }
    }
}