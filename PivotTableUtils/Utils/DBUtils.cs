using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SQLite;

namespace PivotTableUtils.Utils
{
    public static class DBUtils
    {
        public static IEnumerable<T> Query<T>(this ISQLDatabase database, string sql, Func<DbDataReader,T> yield)
        {
            var connection = database.Connection;
            connection.Open();
            var command = database.Command(sql);
            var reader = command.ExecuteReader();
            while (reader.Read())
                yield return yield(reader);
        }
    }
}
