using Ghpr.Core.Interfaces;

namespace Ghpr.Core.Providers
{
    public class FileSystemDataProvider : AbstractDataProvider
    {
        public FileSystemDataProvider(IReporterSettings reporterSettings, ILocationsProvider locationsProvider) 
            : base(reporterSettings, locationsProvider)
        {
        }

        public override void SaveTestRun(IRun run)
        {
            throw new System.NotImplementedException();
        }

        public override void SaveRun(ITestRun testRun)
        {
            throw new System.NotImplementedException();
        }
    }
}