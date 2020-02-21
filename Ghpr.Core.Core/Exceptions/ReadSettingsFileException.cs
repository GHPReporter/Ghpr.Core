using System;

namespace Ghpr.Core.Exceptions
{
    public class ReadSettingsFileException : Exception
    {
        public ReadSettingsFileException(string s, Exception exception) : base(s, exception)
        {
        }
    }
}