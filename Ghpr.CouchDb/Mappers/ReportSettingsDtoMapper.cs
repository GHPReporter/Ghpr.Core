using Ghpr.Core.Common;
using Ghpr.CouchDb.Entities;

namespace Ghpr.CouchDb.Mappers
{
    public static class ReportSettingsDtoMapper
    {
        public static ReportSettings Map(this ReportSettingsDto runDto)
        {
            var run = new ReportSettings(runDto.RunsToDisplay, runDto.TestsToDisplay);
            return run;
        }
    }
}