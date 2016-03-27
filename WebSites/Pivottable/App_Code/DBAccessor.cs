using PivotTableUtils;
using PivotTableUtils.Utils;
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

    public IDataSet _data;
    public void Initialize()
    {
        //_data = PivotTableUtils.SampleDataStore.GetSample(2);
        _data = new SQLDataSet(CSVDatabase.SampleSales, "SELECT * FROM PRODUCTS.CSV");

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
    
}
