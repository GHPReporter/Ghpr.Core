using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ghpr.Core.Common;
using Ghpr.Core.Extensions;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Providers;
using Ghpr.Core.Settings;
using Ghpr.Core.Utils;
using Ghpr.LocalFileSystem.Extensions;
using Ghpr.LocalFileSystem.Interfaces;
using Ghpr.LocalFileSystem.Providers;
using Newtonsoft.Json;

namespace Ghpr.LocalFileSystem.Services
{
    public class FileSystemDataWriterService : IDataWriterService
    {
        private ILocationsProvider _locationsProvider;
        private ILogger _logger;
        private IDataReaderService _reader;
        private Dictionary<Guid, ItemInfoDto> _processedTests;

        public void InitializeDataWriter(ProjectSettings settings, ILogger logger, IDataReaderService reader)
        {
            _locationsProvider = new LocationsProvider(settings.OutputPath);
            _logger = logger;
            _processedTests = new Dictionary<Guid, ItemInfoDto>();
            _reader = reader;
        }
        
        public ItemInfoDto SaveRun(RunDto runDto)
        {
            runDto.RunInfo.ItemName = NamesProvider.GetRunFileName(runDto.RunInfo.Guid);
            var runGuid = runDto.RunInfo.Guid;
            var fileName = NamesProvider.GetRunFileName(runGuid);
            runDto.RunInfo.ItemName = fileName;
            _locationsProvider.RunsFolderPath.Create();
            var fullRunPath = _locationsProvider.GetRunFullPath(runGuid);
            using (var file = File.CreateText(fullRunPath))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(file, runDto);
            }
            _logger.Info($"Run was saved: '{fullRunPath}'");
            var runsInfoFullPath = runDto.RunInfo.SaveRunInfo(_locationsProvider);
            _logger.Info($"Runs Info was saved: '{runsInfoFullPath}'");
            _logger.Debug($"Run data was saved correctly: {JsonConvert.SerializeObject(runDto, Formatting.Indented)}");
            return runDto.RunInfo;
        }

        public SimpleItemInfoDto SaveScreenshot(TestScreenshotDto screenshotDto)
        {
            screenshotDto.TestScreenshotInfo.ItemName =
                NamesProvider.GetScreenshotFileName(screenshotDto.TestScreenshotInfo.Date);
            var testGuid = screenshotDto.TestGuid;
            var path = _locationsProvider.GetScreenshotFolderPath(testGuid);
            screenshotDto.Save(path);
            _logger.Info($"Screenshot was saved: '{path}'");
            _logger.Debug($"Screenshot data was saved correctly: {JsonConvert.SerializeObject(screenshotDto, Formatting.Indented)}");
            if (_processedTests.ContainsKey(testGuid))
            {
                var testRun = _reader.GetTestRun(_processedTests[testGuid]);
                if (testRun.Screenshots.All(s => s.Date != screenshotDto.TestScreenshotInfo.Date))
                {
                    testRun.Screenshots.Add(screenshotDto.TestScreenshotInfo);
                    var output = _reader.GetTestOutput(testRun);
                    SaveTestRun(testRun, output);
                }
            }
            return screenshotDto.TestScreenshotInfo;
        }

        public void DeleteRun(ItemInfoDto runInfo)
        {
            var runFullPath = _locationsProvider.GetRunFullPath(runInfo.Guid);
            _logger.Debug($"Deleting Run: {runFullPath}");
            File.Delete(runFullPath);
            _locationsProvider.RunsFolderPath
                .DeleteItemsFromItemInfosFile(_locationsProvider.Paths.File.Runs, new List<ItemInfoDto>(1){runInfo});
        }

        public void DeleteTest(TestRunDto testRun)
        {
            var testFullPath = _locationsProvider.GetTestFullPath(testRun.TestInfo.Guid, 
                testRun.TestInfo.Finish);
            _logger.Debug($"Deleting Test: {testFullPath}");
            File.Delete(testFullPath);
            _locationsProvider.GetTestFolderPath(testRun.TestInfo.Guid)
                .DeleteItemsFromItemInfosFile(_locationsProvider.Paths.File.Tests, new List<ItemInfoDto>(1) { testRun.TestInfo });
        }

        public void DeleteTestOutput(TestRunDto testRun, TestOutputDto testOutput)
        {
            var testOutputFullPath = _locationsProvider.GetTestOutputFullPath(testRun.TestInfo.Guid, 
                testRun.TestInfo.Finish);
            _logger.Debug($"Deleting Test Output: {testOutputFullPath}");
            File.Delete(testOutputFullPath);
        }

        public void DeleteTestScreenshot(TestRunDto testRun, TestScreenshotDto testScreenshot)
        {
            var testScreenshotFullPath = _locationsProvider
                .GetTestScreenshotFullPath(testRun.TestInfo.Guid, testScreenshot.TestScreenshotInfo.Date);
            _logger.Debug($"Deleting Test screenshot: {testScreenshotFullPath}");
            File.Delete(testScreenshotFullPath);
        }

        public void SaveReportSettings(ReportSettingsDto reportSettingsDto)
        {
            var fullPath = reportSettingsDto.Save(_locationsProvider.SrcFolderPath, Paths.Files.ReportSettings);
            _logger.Info($"Report settings were saved: '{fullPath}'");
            _logger.Debug($"Report settings were saved correctly: {JsonConvert.SerializeObject(reportSettingsDto, Formatting.Indented)}");
        }

        public ItemInfoDto SaveTestRun(TestRunDto testRunDto, TestOutputDto testOutputDto)
        {
            testRunDto.TestInfo.ItemName = NamesProvider.GetTestRunFileName(testRunDto.TestInfo.Finish);
            testOutputDto.TestOutputInfo.ItemName = NamesProvider.GetTestOutputFileName(testRunDto.TestInfo.Finish);
            var imgFolder = _locationsProvider.GetScreenshotFolderPath(testRunDto.TestInfo.Guid);
            if (Directory.Exists(imgFolder))
            {
                var imgFiles = new DirectoryInfo(imgFolder).GetFiles("*.json");
                _logger.Info($"Checking unassigned img files: {imgFiles.Length} file found");
                foreach (var imgFile in imgFiles)
                {
                    var img = Path.Combine(imgFolder, imgFile.Name).LoadTestScreenshot();
                    if (imgFile.CreationTime > testRunDto.TestInfo.Start)
                    {
                        _logger.Info($"New img file found: {imgFile.CreationTime}, {imgFile.Name}");
                        if (testRunDto.Screenshots.All(s => s.Date != img.TestScreenshotInfo.Date))
                        {
                            testRunDto.Screenshots.Add(img.TestScreenshotInfo);
                        }
                    }
                }
            }
            var testOutputFullPath = testOutputDto.Save(_locationsProvider.GetTestOutputFolderPath(testRunDto.TestInfo.Guid));
            _logger.Info($"Test output was saved: '{testOutputFullPath}'");
            _logger.Debug($"Test run data was saved correctly: {JsonConvert.SerializeObject(testOutputDto, Formatting.Indented)}");
            var testRunFullPath = testRunDto.Save(_locationsProvider.GetTestFolderPath(testRunDto.TestInfo.Guid));
            _logger.Info($"Test run was saved: '{testRunFullPath}'");
            var testRunsInfoFullPath = testRunDto.TestInfo.SaveTestInfo(_locationsProvider);
            _logger.Info($"Test runs Info was saved: '{testRunsInfoFullPath}'");
            _logger.Debug($"Test run data was saved correctly: {JsonConvert.SerializeObject(testRunDto, Formatting.Indented)}");

            if (!_processedTests.ContainsKey(testRunDto.TestInfo.Guid))
            {
                _processedTests.Add(testRunDto.TestInfo.Guid, testRunDto.TestInfo);
            }

            return testRunDto.TestInfo;
        }

        public void UpdateTestOutput(ItemInfoDto testInfo, TestOutputDto testOutput)
        {
            testInfo.ItemName = NamesProvider.GetTestRunFileName(testInfo.Finish);
            testOutput.TestOutputInfo.ItemName = NamesProvider.GetTestOutputFileName(testInfo.Finish);
            var outputFolderPath = _locationsProvider.GetTestOutputFolderPath(testInfo.Guid);
            var outputFileName = NamesProvider.GetTestOutputFileName(testInfo.Finish);
            var existingOutput = Path.Combine(outputFolderPath, outputFileName).LoadTestOutput();
            _logger.Debug($"Loaded existing output: {JsonConvert.SerializeObject(existingOutput, Formatting.Indented)}");
            existingOutput.SuiteOutput = testOutput.SuiteOutput;
            existingOutput.Output = testOutput.Output;
            File.Delete(Path.Combine(outputFolderPath, outputFileName));
            _logger.Debug("Deleted old output");
            existingOutput.Save(outputFolderPath);
            _logger.Debug($"Saved updated output: {JsonConvert.SerializeObject(existingOutput, Formatting.Indented)}");
        }
    }
}