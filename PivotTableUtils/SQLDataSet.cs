using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PivotTableUtils
{
    public class SQLDataSet : IDataSet
    {
        readonly SQLiteDatabase _database;
        readonly string _sql;
        readonly string _name;

        public SQLDataSet(SQLiteDatabase database, string sql, string name = "")
        {
            _database = database;
            _sql = sql;
            _name = name;
        }

        public string QueryString(QueryParams queryParams)
        {
            var filter = queryParams.filter;
            var conditions = string.Join(" AND ", filter.Select(kvp => $"{kvp.Key}='{kvp.Value}'").Concat(new[] { "1=1" }) );
            var groupbys = string.Join(", ", filter.Select(kvp => kvp.Key).Concat(new[] { queryParams.nextPivot }));
            var aggregations = string.Join(", ", queryParams.valueFields.Select(field => $"SUM({field})")); ;
            var queryString = $"SELECT {queryParams.nextPivot}, {aggregations} FROM ({_sql}) WHERE {conditions} GROUP BY {groupbys}";
            return queryString;
        }

        public List<QueryResults> Query(QueryParams queryParams, QuerySettings settings)
        {
            var queryString = QueryString(queryParams);
            // use the querystring to actually do the query

            var results = DBUtils.Query(_database, queryString, r => {
                var key = r[0].ToString();
                var values = Enumerable.Range(0, queryParams.valueFields.Count)
                    .ToDictionary(i => queryParams.valueFields[i], i => double.Parse(r[i+1].ToString()));
                return new QueryResults(key,values);
                });

            return results.ToList();
        }

        public override string ToString()
        {
            return $"[{_name}] {_sql}";
        }
    }
}
