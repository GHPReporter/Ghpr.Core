using System;
using Ghpr.Core.Common;
using Ghpr.Core.Settings;
using Ghpr.Core.Utils;
using NUnit.Framework;

namespace Ghpr.Core.Tests.Core.Utils
{
    [TestFixture]
    public class RunDtoRepositoryTests
    {
        private readonly ReporterSettings _settings = 
            "Ghpr.Core.Tests.Settings.json".LoadSettingsAs<ReporterSettings>();

        [Test]
        public void CurrentRunIsNullBeforeStarted()
        {
            var repository = new RunDtoRepository();
            Assert.Null(repository.CurrentRun);
            Assert.Throws<NullReferenceException>(() => repository.SetRunName("any name"));
            Assert.Throws<NullReferenceException>(() => repository.OnTestFinished(new TestRunDto()));
            Assert.Throws<NullReferenceException>(() => repository.OnRunFinished(DateTime.Now));
        }

        [Test]
        public void CanSetRunName()
        {
            var now = DateTime.Now;
            var repository = new RunDtoRepository();
            repository.OnRunStarted(_settings, now);
            Assert.AreEqual(_settings.RunName, repository.CurrentRun.Name);
            const string newName = "Cool new name";
            repository.SetRunName(newName);
            Assert.AreEqual(newName, repository.CurrentRun.Name);
            Assert.AreEqual(Guid.Parse(_settings.RunGuid), repository.RunGuid);
        }

        [Test]
        public void OnRunStarted()
        {
            var now = DateTime.Now;
            var repository = new RunDtoRepository();
            repository.OnRunStarted(_settings, now);
            Assert.AreEqual(_settings.RunName, repository.CurrentRun.Name);
            Assert.AreEqual(Guid.Parse(_settings.RunGuid), repository.CurrentRun.RunInfo.Guid);
            Assert.AreEqual(now, repository.CurrentRun.RunInfo.Start);
            Assert.AreEqual(_settings.Sprint, repository.CurrentRun.Sprint);
            Assert.AreEqual(0, repository.CurrentRun.RunSummary.Errors);
            Assert.AreEqual(0, repository.CurrentRun.RunSummary.Failures);
            Assert.AreEqual(0, repository.CurrentRun.RunSummary.Ignored);
            Assert.AreEqual(0, repository.CurrentRun.RunSummary.Inconclusive);
            Assert.AreEqual(0, repository.CurrentRun.RunSummary.Success);
            Assert.AreEqual(0, repository.CurrentRun.RunSummary.Total);
            Assert.AreEqual(0, repository.CurrentRun.TestsInfo.Count);
        }

        [Test]
        public void OnRunFinished()
        {
            var start = DateTime.Now;
            var finish = DateTime.Now.AddSeconds(10);
            var repository = new RunDtoRepository();
            repository.OnRunStarted(_settings, start);
            Assert.AreEqual(_settings.RunName, repository.CurrentRun.Name);
            Assert.AreEqual(Guid.Parse(_settings.RunGuid), repository.CurrentRun.RunInfo.Guid);
            Assert.AreEqual(start, repository.CurrentRun.RunInfo.Start);
            Assert.AreEqual(_settings.Sprint, repository.CurrentRun.Sprint);
            Assert.AreEqual(0, repository.CurrentRun.RunSummary.Errors);
            Assert.AreEqual(0, repository.CurrentRun.RunSummary.Failures);
            Assert.AreEqual(0, repository.CurrentRun.RunSummary.Ignored);
            Assert.AreEqual(0, repository.CurrentRun.RunSummary.Inconclusive);
            Assert.AreEqual(0, repository.CurrentRun.RunSummary.Success);
            Assert.AreEqual(0, repository.CurrentRun.RunSummary.Total);
            Assert.AreEqual(0, repository.CurrentRun.TestsInfo.Count);
            repository.OnRunFinished(finish);
            Assert.AreEqual(_settings.RunName, repository.CurrentRun.Name);
            Assert.AreEqual(Guid.Parse(_settings.RunGuid), repository.CurrentRun.RunInfo.Guid);
            Assert.AreEqual(start, repository.CurrentRun.RunInfo.Start);
            Assert.AreEqual(_settings.Sprint, repository.CurrentRun.Sprint);
            Assert.AreEqual(finish, repository.CurrentRun.RunInfo.Finish);
            Assert.AreEqual(0, repository.CurrentRun.RunSummary.Errors);
            Assert.AreEqual(0, repository.CurrentRun.RunSummary.Failures);
            Assert.AreEqual(0, repository.CurrentRun.RunSummary.Ignored);
            Assert.AreEqual(0, repository.CurrentRun.RunSummary.Inconclusive);
            Assert.AreEqual(0, repository.CurrentRun.RunSummary.Success);
            Assert.AreEqual(0, repository.CurrentRun.RunSummary.Total);
            Assert.AreEqual(0, repository.CurrentRun.TestsInfo.Count);
        }

        [Test]
        public void OnRunWithTests()
        {
            var start = DateTime.Now;
            var repository = new RunDtoRepository();
            repository.OnRunStarted(_settings, start);
            repository.OnTestFinished(new TestRunDto(Guid.NewGuid())
            {
                Result = "passed"
            });
            Assert.AreEqual(0, repository.CurrentRun.RunSummary.Errors);
            Assert.AreEqual(0, repository.CurrentRun.RunSummary.Failures);
            Assert.AreEqual(0, repository.CurrentRun.RunSummary.Ignored);
            Assert.AreEqual(0, repository.CurrentRun.RunSummary.Inconclusive);
            Assert.AreEqual(1, repository.CurrentRun.RunSummary.Success);
            Assert.AreEqual(1, repository.CurrentRun.RunSummary.Total);
            Assert.AreEqual(1, repository.CurrentRun.TestsInfo.Count);
            repository.OnTestFinished(new TestRunDto(Guid.NewGuid())
            {
                Result = "error"
            });
            Assert.AreEqual(1, repository.CurrentRun.RunSummary.Errors);
            Assert.AreEqual(0, repository.CurrentRun.RunSummary.Failures);
            Assert.AreEqual(0, repository.CurrentRun.RunSummary.Ignored);
            Assert.AreEqual(0, repository.CurrentRun.RunSummary.Inconclusive);
            Assert.AreEqual(1, repository.CurrentRun.RunSummary.Success);
            Assert.AreEqual(2, repository.CurrentRun.RunSummary.Total);
            Assert.AreEqual(2, repository.CurrentRun.TestsInfo.Count);
            repository.OnTestFinished(new TestRunDto(Guid.NewGuid())
            {
                Result = "failed"
            });
            Assert.AreEqual(1, repository.CurrentRun.RunSummary.Errors);
            Assert.AreEqual(1, repository.CurrentRun.RunSummary.Failures);
            Assert.AreEqual(0, repository.CurrentRun.RunSummary.Ignored);
            Assert.AreEqual(0, repository.CurrentRun.RunSummary.Inconclusive);
            Assert.AreEqual(1, repository.CurrentRun.RunSummary.Success);
            Assert.AreEqual(3, repository.CurrentRun.RunSummary.Total);
            Assert.AreEqual(3, repository.CurrentRun.TestsInfo.Count);
            repository.OnTestFinished(new TestRunDto(Guid.NewGuid())
            {
                Result = "inconclusive"
            });
            Assert.AreEqual(1, repository.CurrentRun.RunSummary.Errors);
            Assert.AreEqual(1, repository.CurrentRun.RunSummary.Failures);
            Assert.AreEqual(0, repository.CurrentRun.RunSummary.Ignored);
            Assert.AreEqual(1, repository.CurrentRun.RunSummary.Inconclusive);
            Assert.AreEqual(1, repository.CurrentRun.RunSummary.Success);
            Assert.AreEqual(4, repository.CurrentRun.RunSummary.Total);
            Assert.AreEqual(4, repository.CurrentRun.TestsInfo.Count);
            repository.OnTestFinished(new TestRunDto(Guid.NewGuid())
            {
                Result = "ignored"
            });
            Assert.AreEqual(1, repository.CurrentRun.RunSummary.Errors);
            Assert.AreEqual(1, repository.CurrentRun.RunSummary.Failures);
            Assert.AreEqual(1, repository.CurrentRun.RunSummary.Ignored);
            Assert.AreEqual(1, repository.CurrentRun.RunSummary.Inconclusive);
            Assert.AreEqual(1, repository.CurrentRun.RunSummary.Success);
            Assert.AreEqual(5, repository.CurrentRun.RunSummary.Total);
            Assert.AreEqual(5, repository.CurrentRun.TestsInfo.Count);
        }
    }
}