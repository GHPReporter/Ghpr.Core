using System;

namespace Ghpr.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static string GetTestName(this DateTime finishDateTime)
        {
            return $"test_{finishDateTime.ToString("yyyyMMdd_HHmmssfff")}.json";
        }
    }
}