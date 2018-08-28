using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Settings;

namespace Ghpr.Core.Common
{
    public class CommonCache : ICommonCache
    {
        private ILogger _dataReaderLogger;
        private ILogger _dataWriterLogger;
        private static MemoryCache _cache = new MemoryCache("Common Cache");

        private const string ReportSettingsKey = nameof(ReportSettingsKey);

        private const string AllRunDtosKey = nameof(AllRunDtosKey);
        private const string AllTestRunDtosKey = nameof(AllTestRunDtosKey);
        private const string AllTestOutputDtosKey = nameof(AllTestOutputDtosKey);
        private const string AllTestScreenshotDtosKey = nameof(AllTestScreenshotDtosKey);

        private List<RunDto> AllRunDtos => _cache.Get(AllRunDtosKey) as List<RunDto>;
        private List<TestRunDto> AllTestRunDtos => _cache.Get(AllTestRunDtosKey) as List<TestRunDto>;
        private List<TestOutputDto> AllTestOutputDtos => _cache.Get(AllTestOutputDtosKey) as List<TestOutputDto>;
        private List<TestScreenshotDto> AllTestScreenshotDtos => _cache.Get(AllTestScreenshotDtosKey) as List<TestScreenshotDto>;

        private DateTimeOffset Offset => new DateTimeOffset(
            DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified), TimeSpan.FromMinutes(10));

        private static readonly Lazy<ICommonCache> Lazy =
            new Lazy<ICommonCache>(() => new CommonCache());

        public static ICommonCache Instance => Lazy.Value;

        private CommonCache()
        {
        }

        public void InitializeDataReader(ReporterSettings settings, ILogger logger)
        {
            _dataReaderLogger = logger;
            _dataReaderLogger.Debug("Data reader initialized in Common cache");
        }

        public ReportSettingsDto GetReportSettings()
        {
            _dataReaderLogger.Debug("Getting report settings from Common cache");
            return _cache.Get(ReportSettingsKey) as ReportSettingsDto;
        }
        
        public TestRunDto GetLatestTestRun(Guid testGuid)
        {
            _dataReaderLogger.Debug("Getting latest test run from Common cache");
            return _cache.Get(testGuid.ToString()) as TestRunDto;
        }

        public TestRunDto GetTestRun(ItemInfoDto testInfo)
        {
            _dataReaderLogger.Debug("Getting test run from Common cache");
            return _cache.Get(testInfo.Guid.ToString()) as TestRunDto;
        }

        public List<ItemInfoDto> GetTestInfos(Guid testGuid)
        {
            _dataReaderLogger.Debug("Getting test runs by Guid from Common cache");
            return AllTestRunDtos?.Where(t => t.TestInfo.Guid.Equals(testGuid)).Select(t => t.TestInfo).ToList();
        }

        public List<TestScreenshotDto> GetTestScreenshots(TestRunDto test)
        {
            _dataReaderLogger.Debug("Getting test screenshots from Common cache");
            return AllTestScreenshotDtos?.Where(s => s.TestGuid.Equals(test.TestInfo.Guid) 
                && s.TestScreenshotInfo.Date >= test.TestInfo.Start && s.TestScreenshotInfo.Date <= test.TestInfo.Finish).ToList();
        }

        public TestOutputDto GetTestOutput(TestRunDto test)
        {
            _dataReaderLogger.Debug("Getting test output from Common cache");
            return test == null ? null : AllTestOutputDtos?.FirstOrDefault(to => to.TestOutputInfo.Equals(test.Output));
        }

        public RunDto GetRun(Guid runGuid)
        {
            _dataReaderLogger.Debug("Getting run by Guid from Common cache");
            return _cache.Get(runGuid.ToString()) as RunDto;
        }

        public List<ItemInfoDto> GetRunInfos()
        {
            _dataReaderLogger.Debug("Getting all runs from Common cache");
            var res = AllRunDtos?.Select(r => r.RunInfo).ToList();
            return res;
        }

        public List<TestRunDto> GetTestRunsFromRun(RunDto run)
        {
            _dataReaderLogger.Debug("Getting run's test runs from Common cache");
            var testRuns = run?.TestsInfo.Select(GetTestRun).ToList();
            var res = testRuns == null ? null : testRuns.Any(t => t == null) ? null : testRuns;
            return res;
        }

        public void InitializeDataWriter(ReporterSettings settings, ILogger logger)
        {
            _dataWriterLogger = logger;
            _dataWriterLogger.Debug("Data writer initialized in Common cache");
        }

        public void SaveReportSettings(ReportSettingsDto reportSettings)
        {
            _dataWriterLogger.Debug("Saving report settings in Common cache");
            _cache.Set(ReportSettingsKey, reportSettings, Offset);
        }

        public ItemInfoDto SaveTestRun(TestRunDto testRun, TestOutputDto testOutput)
        {
            _dataWriterLogger.Debug("Saving test run and output in Common cache");
            _cache.Set(testRun.TestInfo.Guid.ToString(), testRun, Offset);

            var tests = AllTestRunDtos ?? new List<TestRunDto>();
            tests.RemoveAll(tr => tr.TestInfo.Guid.Equals(testRun.TestInfo.Guid) && tr.TestInfo.Finish.Equals(testRun.TestInfo.Finish));
            tests.Add(testRun);
            _cache.Set(AllTestRunDtosKey, tests, Offset);

            var outputs = AllTestOutputDtos ?? new List<TestOutputDto>();
            outputs.RemoveAll(o => o.TestOutputInfo.Date.Equals(testOutput.TestOutputInfo.Date)
                                   && o.TestOutputInfo.ItemName.Equals(testOutput.TestOutputInfo.ItemName));
            outputs.Add(testOutput);
            _cache.Set(AllTestOutputDtosKey, outputs, Offset);
            _dataWriterLogger.Debug("Saving test run and output in Common cache: Done");
            return testRun.TestInfo;
        }

        public void UpdateTestOutput(ItemInfoDto testInfo, TestOutputDto testOutput)
        {
            _dataWriterLogger.Debug("Updating test output in Common cache");
            var test = GetTestRun(testInfo);
            if (test != null)
            {
                SaveTestRun(test, testOutput);
            }
        }

        public ItemInfoDto SaveRun(RunDto run)
        {
            _dataWriterLogger.Debug("Saving run in Common cache");
            _cache.Set(run.RunInfo.Guid.ToString(), run, Offset);
            var runs = AllRunDtos ?? new List<RunDto>();
            runs.RemoveAll(r => r.RunInfo.Guid.Equals(run.RunInfo.Guid));
            runs.Add(run);
            _cache.Set(AllRunDtosKey, runs, Offset);
            return run.RunInfo;
        }

        public SimpleItemInfoDto SaveScreenshot(TestScreenshotDto testScreenshot)
        {
            _dataWriterLogger.Debug("Saving test screenshot in Common cache");
            var screens = AllTestScreenshotDtos ?? new List<TestScreenshotDto>();
            screens.RemoveAll(s => s.TestGuid.Equals(testScreenshot.TestGuid)
                                   && s.TestScreenshotInfo.Date.Equals(testScreenshot.TestScreenshotInfo.Date)
                                   && s.TestScreenshotInfo.ItemName.Equals(testScreenshot.TestScreenshotInfo.ItemName));
            screens.Add(testScreenshot);
            _cache.Set(AllTestScreenshotDtosKey, screens, Offset);
            return testScreenshot.TestScreenshotInfo;
        }

        public void DeleteRun(ItemInfoDto runInfo)
        {
            _dataWriterLogger.Debug($"Deleting run with guid = {runInfo.Guid}");
            var runs = AllRunDtos ?? new List<RunDto>();
            runs.RemoveAll(r => r.RunInfo.Guid.Equals(runInfo.Guid));
            _cache.Set(AllRunDtosKey, runs, Offset);
        }

        public void DeleteTest(TestRunDto testRun)
        {
            _dataWriterLogger.Debug($"Deleting test run with guid = {testRun.TestInfo.Guid}");
            var tests = AllTestRunDtos ?? new List<TestRunDto>();
            tests.RemoveAll(tr => tr.TestInfo.Guid.Equals(testRun.TestInfo.Guid) && tr.TestInfo.Finish.Equals(testRun.TestInfo.Finish));
            tests.Add(testRun);
            _cache.Set(AllTestRunDtosKey, tests, Offset);
        }

        public void DeleteTestOutput(TestRunDto testRun, TestOutputDto testOutput)
        {
            _dataWriterLogger.Debug($"Deleting test run output with guid = {testRun.TestInfo.Guid}");
            var outputs = AllTestOutputDtos ?? new List<TestOutputDto>();
            outputs.RemoveAll(o => o.TestOutputInfo.Date.Equals(testOutput.TestOutputInfo.Date)
                                   && o.TestOutputInfo.ItemName.Equals(testOutput.TestOutputInfo.ItemName));
            outputs.Add(testOutput);
            _cache.Set(AllTestOutputDtosKey, outputs, Offset);
        }

        public void DeleteTestScreenshot(TestRunDto testRun, TestScreenshotDto testScreenshot)
        {
            _dataWriterLogger.Debug($"Deleting test run screenshot with guid = {testRun.TestInfo.Guid}");
            var screens = AllTestScreenshotDtos ?? new List<TestScreenshotDto>();
            screens.RemoveAll(s => s.TestGuid.Equals(testScreenshot.TestGuid)
                                   && s.TestScreenshotInfo.Date.Equals(testScreenshot.TestScreenshotInfo.Date)
                                   && s.TestScreenshotInfo.ItemName.Equals(testScreenshot.TestScreenshotInfo.ItemName));
            screens.Add(testScreenshot);
            _cache.Set(AllTestScreenshotDtosKey, screens, Offset);
        }
    }
}