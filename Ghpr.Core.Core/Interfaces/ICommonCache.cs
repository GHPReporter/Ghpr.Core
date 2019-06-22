namespace Ghpr.Core.Core.Interfaces
{
    public interface ICommonCache : IDataReaderService, IDataWriterService
    {
        void TearDown();
    }
}