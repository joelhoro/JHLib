using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PivotTableUtils;
using PivotTableUtils.Utils;

namespace Tests
{
    /// <summary>
    /// Summary description for PivotTableUtilsTests
    /// </summary>
    [TestClass]
    public class PivotTableUtilsTests
    {
        public PivotTableUtilsTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        private void TestPivotTableQuery(IDataSet dataset, int expected)
        {
            var settings = new QuerySettings();
            var queryParams = new QueryParams()
            {
                filter = new Dictionary<string, string>(), 
                valueFields = new List<string>() { "Sales", "Profit" },
                nextPivot = "Province"
            };

            var queryResults = dataset.Query(queryParams, settings);
            Assert.AreEqual(queryResults.Count, expected);
        }

        [TestMethod]
        public void TestPivotTableQuerySQL()
        {
            var dataset = new SQLDataSet(SQLiteDatabase.SampleSales, "SELECT * FROM SALES", "Sales sample");
            TestPivotTableQuery(dataset, 14);
        }

        [TestMethod]
        public void TestPivotTableQueryTable()
        {
            var dataset = SampleDataStore.GetSample(2);
            TestPivotTableQuery(dataset, 13);
        }

    }
}
