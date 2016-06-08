using System;

namespace Ghpr.Core.Interfaces
{
    public interface ITestScreenshot
    {
        string Name { get; set; }
        DateTime Date { get; set; }
    }
}