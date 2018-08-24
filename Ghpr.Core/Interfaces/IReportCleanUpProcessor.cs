using Ghpr.Core.Settings;

namespace Ghpr.Core.Interfaces
{
    public interface IReportCleanUpProcessor
    {
        void CleanUpReport(RetentionSettings retentionSettings, IDataReaderService reader, IDataWriterService writer, ILogger logger);
    }
}