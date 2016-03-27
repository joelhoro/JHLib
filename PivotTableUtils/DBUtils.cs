using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;

namespace PivotTableUtils
{
    public class SQLiteDatabase {
        string _path;
        public SQLiteDatabase(string path) {
            _path = path;
        }

        public static SQLiteDatabase SampleSales = new SQLiteDatabase(@"c:\Users\Joel\Dropbox\Programming\Subversion\Sample data\SQLite\sales.db");

        public SQLiteConnection Connection => new SQLiteConnection($"Data Source={_path};Version=3;");
    }


    public class DBUtils
    {
        public static IEnumerable<T> Query<T>(SQLiteDatabase database, string sql, Func<DbDataReader,T> yield)
        {
            var connection = database.Connection;
            connection.Open();
            var command = new SQLiteCommand(sql,connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
                yield return yield(reader);
        }
    }
}
