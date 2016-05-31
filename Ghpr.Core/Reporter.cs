using System.Configuration;

namespace Ghpr.Core
{
    public static class Reporter
    {
        public static string OutputPath => ConfigurationManager.AppSettings["outputPath"];



    }
}
