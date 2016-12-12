using System;

namespace Ghpr.Core.Interfaces
{
    public interface ILog
    {
        void WriteToFile(string msg, string fileName);
        void Write(string msg);
        void SetOutputFileName(string fileWithExtension);
        void Exception(Exception exception, string exceptionMessage = "");
    }
}