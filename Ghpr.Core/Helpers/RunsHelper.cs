using Ghpr.Core.Interfaces;

namespace Ghpr.Core.Helpers
{
    public static class RunsHelper
    {
        public static void SaveCurrentRunInfo(string path, IItemInfo runInfo)
        {
            ItemInfoHelper.SaveItemInfo(path, "runs.json", runInfo);
        }
    }
}