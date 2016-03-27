using System.Data.Common;
using System.Data.OleDb;
using System.Data.SQLite;

namespace PivotTableUtils.Utils
{
    public class CSVDatabase : SinglePathDatabase
    {
        public CSVDatabase(string path) : base(path) { }

        public static CSVDatabase SampleSales = new CSVDatabase(@"c:\Users\Joel\Dropbox\Programming\Subversion\Sample data\SQLite");

        public override DbConnection Connection => new OleDbConnection($"Provider = Microsoft.Jet.OleDb.4.0; Data Source = { Path }; Extended Properties = \"Text;HDR=YES;FMT=Delimited\"");
        public override DbCommand Command(string sql)
        {
            var connection = Connection;
            connection.Open();
            return new OleDbCommand(sql, connection as OleDbConnection);
        }

        public DbDataReader GetReader(string sql)
        {
            var command = Command(sql);
            return command.ExecuteReader();
        }
    }
}
