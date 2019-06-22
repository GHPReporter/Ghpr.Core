using Ghpr.Core.Common;
using Ghpr.LocalFileSystem.Core.Entities;

namespace Ghpr.LocalFileSystem.Core.Mappers
{
    public static class ReportSettingsDtoMapper
    {
        public static ReportSettings Map(this ReportSettingsDto rsDto)
        {
            var rs = new ReportSettings(rsDto.RunsToDisplay, rsDto.TestsToDisplay, rsDto.ReportName, rsDto.ProjectName);
            return rs;
        }

        public static ReportSettingsDto ToDto(this ReportSettings rs)
        {
            var rsDto = new ReportSettingsDto(rs.TestsToDisplay, rs.RunsToDisplay, rs.ReportName, rs.ProjectName);
            return rsDto;
        }
    }
}