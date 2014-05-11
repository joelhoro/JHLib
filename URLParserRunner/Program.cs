using JHLib.Utils;

namespace JHLib.Utils.Executable
{
    class Program
    {
        static void Main(string[] args)
        {
            URLParser.ParseURL(args[0]);
        }
    }
}
