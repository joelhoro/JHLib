using PivotTableUtils;
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

    public TableDataSet _data;
    public void Initialize()
    {
        _data = PivotTableUtils.SampleDataStore.GetSample(2);
    }

    public DBAccessor () {
        Initialize();
    }

    [WebMethod]
    public List<QueryResults> Query(QueryParams queryParams, QuerySettings settings)
    {
        return _data.Query(queryParams, settings);
    }
    
}
