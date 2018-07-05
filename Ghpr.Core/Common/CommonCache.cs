using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using Ghpr.Core.Interfaces;

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
        }

        public ReportSettingsDto GetReportSettings()
        {
            return _cache.Get(ReportSettingsKey) as ReportSettingsDto;
        }

        public void InitializeDataWriter(ReporterSettings settings, ILogger logger)
        {
            _dataWriterLogger = logger;
        }

        public TestRunDto GetLatestTestRun(Guid testGuid)
        {
            return _cache.Get(testGuid.ToString()) as TestRunDto;
        }

        public TestRunDto GetTestRun(ItemInfoDto testInfo)
        {
            return _cache.Get(testInfo.Guid.ToString()) as TestRunDto;
        }

        public List<TestRunDto> GetTestRuns(Guid testGuid)
        {
            return AllTestRunDtos?.Where(t => t.TestInfo.Guid.Equals(testGuid)).ToList();
        }

        public List<TestScreenshotDto> GetTestScreenshots(ItemInfoDto testInfo)
        {
            return AllTestScreenshotDtos?.Where(s => s.TestGuid.Equals(testInfo.Guid) 
                && s.TestScreenshotInfo.Date >= testInfo.Start && s.TestScreenshotInfo.Date <= testInfo.Finish).ToList();
        }

        public TestOutputDto GetTestOutput(ItemInfoDto testInfo)
        {
            var test = GetTestRun(testInfo);
            return test == null ? null : AllTestOutputDtos?.FirstOrDefault(to => to.TestOutputInfo.Equals(test.Output));
        }

        public RunDto GetRun(Guid runGuid)
        {
            return _cache.Get(runGuid.ToString()) as RunDto;
        }

        public List<RunDto> GetRuns()
        {
            return AllRunDtos;
        }

        public List<TestRunDto> GetTestRunsFromRun(Guid runGuid)
        {
            var run = GetRun(runGuid);
            var res = run?.TestsInfo.Select(GetTestRun).ToList();
            return res == null ? null : res.Any(t => t == null) ? null : res;
        }
        
        public void SaveReportSettings(ReportSettingsDto reportSettings)
        {
            _cache.Set(ReportSettingsKey, reportSettings, Offset);
        }

        public ItemInfoDto SaveTestRun(TestRunDto testRun, TestOutputDto testOutput)
        {
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
            return testRun.TestInfo;
        }

        public void UpdateTestOutput(ItemInfoDto testInfo, TestOutputDto testOutput)
        {
            var test = GetTestRun(testInfo);
            if (test != null)
            {
                SaveTestRun(test, testOutput);
            }
        }

        public ItemInfoDto SaveRun(RunDto run)
        {
            _cache.Set(run.RunInfo.Guid.ToString(), run, Offset);
            var runs = AllRunDtos ?? new List<RunDto>();
            runs.RemoveAll(r => r.RunInfo.Guid.Equals(run.RunInfo.Guid));
            runs.Add(run);
            _cache.Set(AllRunDtosKey, runs, Offset);
            return run.RunInfo;
        }

        public SimpleItemInfoDto SaveScreenshot(TestScreenshotDto testScreenshot)
        {
            var screens = AllTestScreenshotDtos ?? new List<TestScreenshotDto>();
            screens.RemoveAll(s => s.TestGuid.Equals(testScreenshot.TestGuid)
                                   && s.TestScreenshotInfo.Date.Equals(testScreenshot.TestScreenshotInfo.Date)
                                   && s.TestScreenshotInfo.ItemName.Equals(testScreenshot.TestScreenshotInfo.ItemName));
            screens.Add(testScreenshot);
            _cache.Set(AllTestScreenshotDtosKey, screens, Offset);
            return testScreenshot.TestScreenshotInfo;
        }
    }
}