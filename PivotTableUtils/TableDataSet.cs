using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PivotTableUtils
{
    public class QueryParams
    {
        public Dictionary<string, string> filter;
        public string nextPivot;
        public List<string> valueFields;
    }

    public class QuerySettings
    {
        public int limit = -1;
    }

    public struct QueryResults
    {
        readonly string _key;
        readonly Dictionary<string, double> _values;
        public string Key => _key;
        public Dictionary<string, double> Values => _values;

        public QueryResults(string key, Dictionary<string,double> values)
        {
            _key = key;
            _values = values;
        }

        public override string ToString()
        {
            return $"[{GetType().Name}] {_key}";
        }
    }

    public interface IDataSet
    {
        List<QueryResults> Query(QueryParams queryParams, QuerySettings settings);
    }

    public class TableDataSet : IDataSet
    {
        List<Dictionary<string,string>> _data;

        public TableDataSet(List<Dictionary<string,string>> data) {
            _data = data;
        }

        public List<QueryResults> Query(QueryParams queryParams, QuerySettings settings)
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
                .Select(grp => new QueryResults(grp.Key,valueFields.ToDictionary(f => f, f => grp.Sum(row => double.Parse(row[f])))))
                .ToList();
        }
    }
}
