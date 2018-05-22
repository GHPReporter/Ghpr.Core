namespace Ghpr.Core.Interfaces
{
    public interface IScreenshotService
    {
        IDataService DataService { get; }

        void SaveScreenshot(byte[] screenshotBytes);
        void InitializeDataService(IDataService dataService);
    }
}