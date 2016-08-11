using Ghpr.Core.Common;
namespace Ghpr.Core.Helpers
{
    public static class TestRunsHelper
    {
        public static void SaveCurrentTestInfo(string path, ItemInfo testInfo)
        {
            ItemInfoHelper.SaveItemInfo(path, "tests.json", testInfo, false);
        }
    }
}