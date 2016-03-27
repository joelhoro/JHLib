using System.Data.Common;
using System.Data.OleDb;
using System.Data.SQLite;

namespace PivotTableUtils.Utils
{
    public interface ISQLDatabase
    {
        DbConnection Connection { get; }
        DbCommand Command(string sql);
    }

    public abstract class SinglePathDatabase : ISQLDatabase
    {
        string _path;
        public string Path => _path;
        public SinglePathDatabase(string path) {
            _path = path;
        }

        public abstract DbConnection Connection { get; }

        public abstract DbCommand Command(string sql);
    }

    public class SQLiteDatabase : SinglePathDatabase
    {
        public SQLiteDatabase(string path) : base(path) { }
        public static SQLiteDatabase SampleSales = new SQLiteDatabase(@"c:\Users\Joel\Dropbox\Programming\Subversion\Sample data\SQLite\sales.db");

        public override DbConnection Connection => new SQLiteConnection($"Data Source={Path};Version=3;");
        public override DbCommand Command(string sql)
        {
            return new SQLiteCommand(sql, Connection as SQLiteConnection);
        }

    }
}
