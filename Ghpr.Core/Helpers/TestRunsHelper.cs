using Ghpr.Core.Interfaces;

namespace Ghpr.Core.Helpers
{
    public static class TestRunsHelper
    {
        public static void SaveCurrentTestInfo(string path, IItemInfo testInfo)
        {
            ItemInfoHelper.SaveItemInfo(path, "tests.json", testInfo);
        }
    }
}