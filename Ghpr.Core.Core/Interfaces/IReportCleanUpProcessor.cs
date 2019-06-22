using Ghpr.Core.Core.Settings;

namespace Ghpr.Core.Core.Interfaces
{
    public interface IReportCleanUpProcessor
    {
        void CleanUpReport(RetentionSettings retentionSettings, IDataReaderService reader, IDataWriterService writer);
    }
}