using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace PivotTableUtils
{
    public class SampleDataStore
    {
        public static TableDataSet GetSample(int i)
        {
            var resourceName = "dummydata" + i + ".json";
            return new TableDataSet(GetResource(resourceName));
        }

        public static string GetResourceAsString(string resourceName)
        {
            var fullName = "PivotTableUtils.SampleData." + resourceName;
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream(fullName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public static List<Dictionary<string,string>> GetResource(string resourceName)
        {
            var result = GetResourceAsString(resourceName);
            var data = new JavaScriptSerializer() { MaxJsonLength = 8000000 }
                .Deserialize<List<Dictionary<string, string>>>(result)
                .ToList();
            return data;
        }
    }
}
