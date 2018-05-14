using Ghpr.Core.Extensions;
using Ghpr.Core.Interfaces;

namespace Ghpr.Core.Providers
{
    public class FileSystemDataProvider : AbstractDataProvider
    {
        public FileSystemDataProvider(IReporterSettings reporterSettings, ILocationsProvider locationsProvider) 
            : base(reporterSettings, locationsProvider) { }

        public override void SaveRun(IRun run)
        {
            run.Save(LocationsProvider.RunsPath);
            run.RunInfo.SaveRunInfo(LocationsProvider);
        }

        public override void SaveTestRun(ITestRun testRun)
        {
            throw new System.NotImplementedException();
        }
    }
}