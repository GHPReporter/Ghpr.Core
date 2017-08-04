using System;

namespace Ghpr.Core.Interfaces
{
    public interface ITestData
    {
        string Comment { get; set; }
        DateTime Date { get; set; }
        string Actual { get; set; }
        string Expected { get; set; }
    }
}