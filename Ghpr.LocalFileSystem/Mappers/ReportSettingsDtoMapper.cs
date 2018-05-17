using Ghpr.Core.Common;
using Ghpr.LocalFileSystem.Entities;

namespace Ghpr.LocalFileSystem.Mappers
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