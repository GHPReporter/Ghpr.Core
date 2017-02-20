using Ghpr.Core.Common;
using Ghpr.Core.Utils;

namespace Ghpr.Core.Helpers
{
    public static class TestRunsHelper
    {
        public static void SaveCurrentTestInfo(string path, ItemInfo testInfo)
        {
            ItemInfoHelper.SaveItemInfo(path, Paths.Files.Tests, testInfo, false);
        }
    }
}