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
                var r = new Reporter();
                r.RunStarted();

                r.TestStarted("11111111-1111-1111-1111-111111111111");
                r.TestStarted("11111111-1111-1111-1111-111111111112");
                r.TestFinished("11111111-1111-1111-1111-111111111111");
                r.TestFinished("11111111-1111-1111-1111-111111111112");
                
                r.RunFinished();

                System.Console.WriteLine("Exit? (y/n)");
                res = System.Console.ReadLine() ?? "n";
            }
        }
    }
}
