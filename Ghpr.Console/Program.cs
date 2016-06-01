using Ghpr.Core;

namespace Ghpr.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var res = "n";
            while (!res.Equals("y"))
            {
                Reporter.ExtractReportFiles();
                System.Console.WriteLine("Exit? (y/n)");
                res = System.Console.ReadLine() ?? "n";
            }
        }
    }
}
