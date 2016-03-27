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
            var ressourceName = "PivotTableUtils.SampleData.dummydata" + i + ".json";
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream(ressourceName))
            using (var reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                var data = new JavaScriptSerializer() { MaxJsonLength = 8000000 }
                    .Deserialize<List<Dictionary<string, string>>>(result)
                    .ToList();
                return new TableDataSet(data);
            }
        }
    }
}
