using JHLib.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class CompressionTests
    {
        [TestMethod]
        public void HuffmanTest()
        {
            var message = @"
That's the witness all right,
the one from the Barksdale case.
Gant, William. 41 years.
Single headshot, close range.
Bullet pancaked on the inner skull.
-Ain't necessarily what it looks like.
-No?
";
            message = "abaaadeefaheo";

            var huffmanData = HuffmanData.FromString(message);

            Assert.AreEqual(message, huffmanData.ToString());
        }
    }
}
