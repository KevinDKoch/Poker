using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerLib2.Reports;
using MathNet.Numerics.Statistics;

namespace PokerLib2Tests
{
    [TestClass]
    public class StartingHandGridTests
    {
        private class EVData //: IGridReport, ICSVReport
        {
            public EVData()
            {
                EV = new double[] { };
            }

            public double[] EV { get; set; }
        }

        [TestMethod]
        public void BasicTests()
        {
            StartingHandGrid<EVData> grid = new StartingHandGrid<EVData>();
            Assert.IsTrue(grid["QQ"].Name == "QQ");
            Assert.IsTrue(grid["QQ"].Data != null);
            
            Assert.IsTrue(Double.IsNaN(grid["QQ"].Data.EV.Mean()));
            
            grid["QQ"].Data.EV = new double[] { 1.0, 2.0, 3.0 };
            Assert.IsTrue(grid["QQ"].Data.EV.Mean() == 2);

        }
    }
}
