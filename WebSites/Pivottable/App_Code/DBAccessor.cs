using PivotTableUtils;
using PivotTableUtils.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class DBAccessor : System.Web.Services.WebService {

    IDataSet _data;
    List<Dictionary<string,string>> _voldata;

    public void Initialize()
    {
        //_data = PivotTableUtils.SampleDataStore.GetSample(2);
        _data = new SQLDataSet(CSVDatabase.SampleSales, "SELECT * FROM PRODUCTS.CSV");
        //_voldata = PivotTableUtils.SampleDataStore.GetResource("dummyvolsurfaces.js");

        //_data = new SQLDataSet(SQLiteDatabase.SampleSales, "SELECT * FROM SALES");
    }

    public DBAccessor () {
        Initialize();
    }

    public struct Q
    {
        public string Name;

    }
    [WebMethod]
    public List<QueryResults> Query(QueryParams queryParams, QuerySettings settings)
    {
        settings = settings ?? new QuerySettings();
        return _data.Query(queryParams, settings);
    }

    public struct DataPoint
    {
        public string key;
        public string value;
    }

    [WebMethod]
//    public string RetrieveVolSurfaces(QueryParams q)
    public List<List<DataPoint>> RetrieveVolSurfaces2()
    {
        return _voldata
            .Select(d => d.Select(kvp => new DataPoint { key = kvp.Key, value = kvp.Value}).ToList())
            .Take(10)
            .ToList();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void RetrieveVolSurfaces()
    {
        var str = SampleDataStore.GetResourceAsString("dummyvolsurfaces.js");

        //JavaScriptSerializer js = new JavaScriptSerializer();
        //string str = js.Serialize(item);

        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        Context.Response.AddHeader("content-disposition", "attachment; filename=export.json");
        Context.Response.AddHeader("content-length", str.Length.ToString());
        Context.Response.Flush();
        Context.Response.Write(str);
    }


}
