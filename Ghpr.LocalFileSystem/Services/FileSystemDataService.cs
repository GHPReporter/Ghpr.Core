using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Ghpr.Core;
using Ghpr.Core.Common;
using Ghpr.Core.Extensions;
using Ghpr.Core.Interfaces;
using Ghpr.LocalFileSystem.Entities;
using Ghpr.LocalFileSystem.Extensions;
using Ghpr.LocalFileSystem.Interfaces;
using Ghpr.LocalFileSystem.Mappers;
using Ghpr.LocalFileSystem.Providers;

namespace Ghpr.LocalFileSystem.Services
{
    public class FileSystemDataService : IDataService
    {
        private ILocationsProvider _locationsProvider;
        private ILogger _logger;

        public void Initialize(ReporterSettings settings, ILogger logger)
        {
            _locationsProvider = new LocationsProvider(settings.OutputPath);
            _logger = logger;
        }
        
        public void SaveRun(RunDto runDto)
        {
            var run = runDto.Map();
            var runFullPath = run.Save(_locationsProvider.RunsPath);
            _logger.Info($"Run was saved: '{runFullPath}'");
            var runsInfoFullPath = run.RunInfo.SaveRunInfo(_locationsProvider);
            _logger.Info($"Runs Info was saved: '{runsInfoFullPath}'");
        }
        
        public void SaveScreenshot(TestScreenshotDto screenshotDto)
        {
            var testScreenshot = screenshotDto.Map();
            using (var image = Image.FromStream(new MemoryStream(testScreenshot.Data)))
            {
                var screenPath = _locationsProvider.GetScreenshotPath(testScreenshot.TestGuid.ToString());
                screenPath.Create();
                var screenName = LocationsProvider.GetScreenshotFileName(testScreenshot.Date);
                var file = Path.Combine(screenPath, screenName);
                var screen = new Bitmap(image);
                screen.Save(file, ImageFormat.Png);
                var fileInfo = new FileInfo(file);
                fileInfo.Refresh();
                fileInfo.CreationTime = testScreenshot.Date;
                _logger.Info($"Screenshot was saved: '{file}'");
            }
        }

        public void SaveReportSettings(ReportSettingsDto reportSettingsDto)
        {
            var reportSettings = reportSettingsDto.Map();
            var fullPath = reportSettings.Save(_locationsProvider);
            _logger.Info($"Report settings were saved: '{fullPath}'");
        }

        public void SaveTestRun(TestRunDto testRunDto)
        {
            var testRun = testRunDto.Map();
            var imgFolder = _locationsProvider.GetScreenshotPath(testRun.TestInfo.Guid.ToString());
            if (Directory.Exists(imgFolder))
            {
                var imgFiles = new DirectoryInfo(imgFolder).GetFiles("*.png");
                foreach (var imgFile in imgFiles)
                {
                    if (imgFile.CreationTime > testRun.TestInfo.Start)
                    {
                        testRun.Screenshots.Add(new TestScreenshot
                        {
                            Date = imgFile.CreationTime,
                            Name = LocationsProvider.GetScreenshotFileName(imgFile.CreationTime)
                        });
                    }
                }
            }
            var testRunFullPath = testRun.Save(_locationsProvider.GetTestPath(testRun.TestInfo.Guid.ToString()));
            _logger.Info($"Test run was saved: '{testRunFullPath}'");
            var testRunsInfoFullPath = testRun.TestInfo.SaveTestInfo(_locationsProvider);
            _logger.Info($"Test runs Info was saved: '{testRunsInfoFullPath}'");
        }
    }
}