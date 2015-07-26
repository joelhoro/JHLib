using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHLib.QuantLIB.Utils
{
    public class TeamCity
    {
        public static void Log(string key, object value)
        {
            var message = string.Format(@"##teamcity[buildStatisticValue key='{0}' value='{1}']", key, value);
            Console.WriteLine(message);
        }
    }
}
