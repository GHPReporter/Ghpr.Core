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
                
                System.Console.WriteLine("Exit? (y/n)");
                res = System.Console.ReadLine() ?? "n";
            }
        }
    }
}
