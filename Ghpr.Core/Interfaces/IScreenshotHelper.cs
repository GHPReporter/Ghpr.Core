using System;

namespace Ghpr.Core.Interfaces
{
    public interface IScreenshotHelper
    {
        void SaveScreenshot(byte[] screenshotBytes, Guid testRunGuid);
    }
}