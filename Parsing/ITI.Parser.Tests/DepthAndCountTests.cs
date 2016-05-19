using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser.Tests
{
    [TestFixture]
    public class DepthAndCountTests
    {
        [TestCase("3", 1, 1)]
        [TestCase("3+8", 2, 3)]
        [TestCase("3*7/2", 3, 5)]
        [TestCase("3712/(45+98)*12*(58/12)", 5, 11)]
        [TestCase("37*(12+4)/(45+98/(4+5+68-8-7+10))*1+(41/9*7+6-72)+2*(5+8/1-2)", 12, 43)]
        [TestCase("37 ? 8 : 5", 2, 4)]
        [TestCase("3+7 ? 1+8 : 4+1", 3, 10)]
        [TestCase("3-7 ? 1+8 : (-4+1 ? -8 : -24 )", 5, 16)]
        public void test_depth_and_count(string text, int depth, int count)
        {
            Node n = new Analyser().Analyse(new StringTokenizer(text));

            int resultDepth = n.Depth;
            int resultCount = n.Count;
            Assert.That(resultDepth, Is.EqualTo(depth));
            Assert.That(resultCount, Is.EqualTo(count));
        }
    }
}