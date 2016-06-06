namespace Ghpr.Core.Utils
{
    public static class Ids
    {
        public struct MainResults
        {
            public const string Id = nameof(Ids) + "-" + nameof(MainResults);

            public const string StartDateTime     = Id + "-" + nameof(StartDateTime);
            public const string FinishDateTime    = Id + "-" + nameof(FinishDateTime);
            public const string Duration          = Id + "-" + nameof(Duration);
            public const string TotalAll          = Id + "-" + nameof(TotalAll);
            public const string TotalPassed       = Id + "-" + nameof(TotalPassed);
            public const string TotalBroken       = Id + "-" + nameof(TotalBroken);
            public const string TotalFailed       = Id + "-" + nameof(TotalFailed);
            public const string TotalIgnored      = Id + "-" + nameof(TotalIgnored);
            public const string TotalInconclusive = Id + "-" + nameof(TotalInconclusive);

            public const string PieChart = Id + "-" + nameof(PieChart);
        }
    }
}
