using KryptoLab1BBS;

namespace TestBBS
{
    
    [TestClass]
    public class FIPS140_2Tests
    {
        private readonly BBS _bbs;
        const int sequenceLength = 20_000;
        List<int> bits;
        Dictionary<int, int> series;

        public FIPS140_2Tests()
        {
            _bbs = new BBS();
            bits = _bbs.GenerateBits(sequenceLength);
            series = GetSeries();
        }


        private Dictionary<int, int> GetSeries()
        {
            var lastVal = bits[0];
            var seriesLength = 1;
            var series = new Dictionary<int, int>();
            series[26] = 0;

            for (int i = 1; i < bits.Count; i++)
            {
                var bit = bits[i];
                if (bit == lastVal)
                {
                    seriesLength += 1;
                }
                else
                {
                    if(seriesLength >= 26)
                    {
                        series[26] += 1;
                    }
                    seriesLength = seriesLength > 6 ? 6 : seriesLength;
                    if (series.ContainsKey(seriesLength) == false)
                    {
                        series.Add(seriesLength, 1);
                    }
                    else
                    {
                        series[seriesLength] += 1;
                    }
                    lastVal = bit;
                    seriesLength = 1;
                }
            }
            return series;
        }

        [TestMethod]
        public void SingleBitTest()
        {
            var sum = bits.Sum();
            Assert.IsTrue(9725 < sum && sum < 10275);
        }

        [TestMethod]
        public void SeriesTest()
        {
            Assert.IsTrue(2315 < series[1] && series[1] < 2685, "Series of length 1 should be between 2315 and 2685");
            Assert.IsTrue(1113 < series[2] && series[2] < 1386, "Series of length 2 should be between 1113 and 1386");
            Assert.IsTrue(527 < series[3] && series[3] < 723, "Series of length 3 should be between 527 and 723");
            Assert.IsTrue(240 < series[4] && series[4] < 384, "Series of length 4 should be between 240 and 384");
            Assert.IsTrue(103 < series[5] && series[5] < 209, "Series of length 5 should be between 103 and 209");
            Assert.IsTrue(103 < series[6] && series[6] < 209, "Series of length 6 and more should be between 103 and 209");
        }

        [TestMethod]
        public void longSeriesTest()
        {
            Assert.AreEqual(0, series[26], "Shouldn't be any series equal or longer to 26");
        }

        [TestMethod]
        public void PokerTest()
        {
            var segments = new Dictionary<ValueTuple<int, int, int, int>, int>()
            {
                { (0,0,0,0),  0 },
                { (0,0,0,1),  0 },
                { (0,0,1,0),  0 },
                { (0,0,1,1),  0 },
                { (0,1,0,0),  0 },
                { (0,1,0,1),  0 },
                { (0,1,1,0),  0 },
                { (0,1,1,1),  0 },
                { (1,0,0,0),  0 },
                { (1,0,0,1),  0 },
                { (1,0,1,0),  0 },
                { (1,0,1,1),  0 },
                { (1,1,0,0),  0 },
                { (1,1,0,1),  0 },
                { (1,1,1,0),  0 },
                { (1,1,1,1),  0 },
            };

            for(int i = 0; i < bits.Count; i += 4)
            {
                var first = bits[i];
                var second = bits[i + 1];
                var third = bits[i + 2];
                var fourth = bits[i + 3];

                segments[ (first, second, third, fourth) ] += 1;
            }

            var sum = segments.Values.ToArray().Sum(num => Math.Pow(num, 2) - 5000);
            var x = (16f / 5000f) * segments.Values.ToArray().Sum( num => Math.Pow(num,2) - 5000);
            Assert.IsTrue(2.16 < x && x < 46.17, "x should be between 2.16 and 46.17");
            // doesnt make sense, if it was evenly distributed among all combinations, x would be 4744.
        }
    }
}