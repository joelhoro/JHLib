using System;
using System.Collections.Generic;
using System.Linq;
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

    public DBAccessor () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    public class QueryParams
    {
        public Dictionary<string,string> filter;
        public string nextPivot;
        public List<string> valueFields;
    }

    [WebMethod]
    public string Query(QueryParams parameters) {
        return "";
    }
    
}
