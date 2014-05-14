using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JHLib.XLFunctions
{
    public static class Handles
    {
        public static Dictionary<string, object> Store = new Dictionary<string, object>();
        private static int counter = 0;

        const string HANDLE_MASK = "H[{0}#{1}]";
        const string HANDLE_REGEX = @"H\[{0}#{1}\]";

        public static void Rename(string from, string to)
        {
            Store[to] = Store[from];
            Store.Remove(from);
        }

        public static string Create(object obj, string tag)
        {
            string handlename = String.Format(HANDLE_MASK, tag, counter++ );
            Store[tag] = obj;
            return handlename;
        }

        public static string GetHandleName(string handlename)
        {
            string pattern = String.Format(HANDLE_REGEX, @"(.*)", @".*");
            Regex regex = new Regex(pattern);
            Match match = regex.Match(handlename);

            string name = null;
            if (match.Success)
                name = match.Groups[1].ToString();

            return name;
        }

        public static object Get(string handlename)
        {
            string pattern = String.Format(HANDLE_REGEX, @"(.*)", @".*");
            Regex regex = new Regex(pattern);
            Match match = regex.Match(handlename);

            object value = null;

            if (match.Success)
            {
                string name = match.Groups[1].ToString();
                Store.TryGetValue(name, out value);
            }

            return value;
        }

    }
}
