using System.Data.SQLite;

namespace PivotTableUtils.Utils
{
    public class SQLiteDatabase {
        string _path;
        public SQLiteDatabase(string path) {
            _path = path;
        }

        public static SQLiteDatabase SampleSales = new SQLiteDatabase(@"c:\Users\Joel\Dropbox\Programming\Subversion\Sample data\SQLite\sales.db");

        public SQLiteConnection Connection => new SQLiteConnection($"Data Source={_path};Version=3;");
    }
}
