using Ghpr.Core.Common;
using Ghpr.CouchDb.Entities;

namespace Ghpr.CouchDb.Mappers
{
    public static class ReportSettingsDtoMapper
    {
        public static DatabaseEntity<ReportSettings> Map(this ReportSettingsDto runDto)
        {
            var run = new ReportSettings(runDto.RunsToDisplay, runDto.TestsToDisplay);
            var entity = new DatabaseEntity<ReportSettings>
            {
                Data = run,
                Id = "ghpr_report_settings",
                Type = EntityType.ReportSettingsType,
                Rev = "1-ghpr_report_settings"
            };
            return entity;
        }
    }
}