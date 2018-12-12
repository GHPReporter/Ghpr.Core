using System;
using Ghpr.Core.Common;
using Ghpr.Core.Utils;
using NUnit.Framework;

namespace Ghpr.Core.Tests.Core
{
    [TestFixture]
    public class TestRunsRepositoryTests
    {
        [Test]
        public void NewRepositoryThrowsExceptionOnAddNewTest()
        {
            var repository = new TestRunsRepository();
            Assert.Throws<NullReferenceException>(() => repository.AddNewTestRun(new TestRunDto()));
        }

        [Test]
        public void NewRepositoryThrowsExceptionOnAddExtractTest()
        {
            var repository = new TestRunsRepository();
            Assert.Throws<ArgumentNullException>(() => repository.ExtractCorrespondingTestRun(new TestRunDto()));
        }

        [Test]
        public void ExtractWithNoTestsInRepository()
        {
            var guid = Guid.Parse("66e6f6ba-5b35-475a-a617-394696331f11");
            var dto = new TestRunDto(guid, "Cool test", "Cool test full name");
            var repository = new TestRunsRepository();
            repository.OnRunStarted();
            var extractedDto = repository.ExtractCorrespondingTestRun(dto);
            Assert.AreEqual(extractedDto.TestInfo.Guid, Guid.Empty);
            Assert.AreEqual(extractedDto.FullName, "");
            Assert.AreEqual(extractedDto.Name, "");
        }

        [Test]
        public void ExtractWithOneTestInRepository()
        {
            var guid = Guid.Parse("66e6f6ba-5b35-475a-a617-394696331f11");
            var dto = new TestRunDto(guid, "Cool test", "Cool test full name");
            var repository = new TestRunsRepository();
            repository.OnRunStarted();
            Assert.DoesNotThrow(() => repository.AddNewTestRun(dto));
            var extractedDto = repository.ExtractCorrespondingTestRun(dto);
            Assert.AreEqual(dto.TestInfo.Guid, extractedDto.TestInfo.Guid);
            Assert.AreEqual(dto.FullName, extractedDto.FullName);
            Assert.AreEqual(dto.Name, extractedDto.Name);
        }

        [Test]
        public void ExtractWithTwoTestsInRepository()
        {
            var guid1 = Guid.Parse("66e6f6ba-5b35-475a-a617-394696331f11");
            var guid2 = Guid.Parse("66e6f6ba-5b35-475a-a617-394696331f12");
            var dto1 = new TestRunDto(guid1, "Cool test 1", "Cool test full name 1");
            var dto2 = new TestRunDto(guid2, "Cool test 2", "Cool test full name 2");
            var repository = new TestRunsRepository();
            repository.OnRunStarted();
            Assert.DoesNotThrow(() => repository.AddNewTestRun(dto1));
            Assert.DoesNotThrow(() => repository.AddNewTestRun(dto2));
            var extractedDto = repository.ExtractCorrespondingTestRun(new TestRunDto(guid1));
            Assert.AreEqual(extractedDto.TestInfo.Guid, extractedDto.TestInfo.Guid);
            Assert.AreEqual(extractedDto.FullName, extractedDto.FullName);
            Assert.AreEqual(extractedDto.Name, extractedDto.Name);
        }
    }
}