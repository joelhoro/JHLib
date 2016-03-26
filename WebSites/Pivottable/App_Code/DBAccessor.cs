using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class DBAccessor : System.Web.Services.WebService {

    public List<Dictionary<string, string>> _data;
    public void Initialize()
    {
        var fileName = @"c:\Users\Joel\Dropbox\Programming\HTML\pivottable\dummydata.json";
        var result = File.ReadAllText(fileName);
        _data = new System.Web.Script.Serialization.JavaScriptSerializer()
            .Deserialize<List<Dictionary<string, string>>>(result)
            .ToList();
    }

    public DBAccessor () {
        Initialize();
    }

    public class QueryParams
    {
        public Dictionary<string, string> filter;
        public string nextPivot;
        public List<string> valueFields;
    }

    public class Settings
    {
        public int limit = -1;
    }

    public class QueryResults
    {
        public string key;
        public Dictionary<string, double> values;
    }

    [WebMethod]
    public List<QueryResults> Query(QueryParams queryParams, Settings settings)
    {
        var filter = queryParams.filter;
        var valueFields = queryParams.valueFields;
        var limitedData = _data;
        if (settings.limit > 0)
            limitedData = limitedData.Take(settings.limit).ToList();

        return limitedData
            .Where(row =>
            {
                return filter.ToList().All(kvp => row[kvp.Key] == kvp.Value);
            })
            .GroupBy(row => row[queryParams.nextPivot])
            .Select(grp => new QueryResults()
            {
                key = grp.Key,
                values = valueFields.ToDictionary(f => f, f => grp.Sum(row => double.Parse(row[f])))
            })
            .ToList();
    }
    
}
