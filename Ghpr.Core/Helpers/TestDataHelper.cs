using Ghpr.Core.Utils;

namespace Ghpr.Core.Helpers
{
    public class TestDataHelper
    {
        public static string GetTestDataDateTimeKey(int count)
        {
            return $"{Paths.Names.TestDataDateTimeKeyTemplate}{count}";
        }

        public static string GetTestDataCommentKey(int count)
        {
            return $"{Paths.Names.TestDataCommentKeyTemplate}{count}";
        }

        public static string GetTestDataActualKey(int count)
        {
            return $"{Paths.Names.TestDataActualKeyTemplate}{count}";
        }

        public static string GetTestDataExpectedKey(int count)
        {
            return $"{Paths.Names.TestDataExpectedKeyTemplate}{count}";
        }

    }
}