using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser.Tests
{
    [TestFixture]
    public class AnalyserTests
    {
        [Test]
        public void simple_factors()
        {
            var a = new Analyser();
            var node = a.Analyse(new StringTokenizer("8*12/5*12"));
            Assert.That(node.ToString(), Is.EqualTo("(((8 * 12) / 5) * 12)"));
        }

        [Test]
        public void factors_with_addition()
        {
            var a = new Analyser();
            var node = a.Analyse(new StringTokenizer("8+12*12"));
            Assert.That(node.ToString(), Is.EqualTo("(8 + (12 * 12))"));
        }

        [Test]
        public void factors_with_minus()
        {
            var a = new Analyser();
            var node = a.Analyse(new StringTokenizer("8-12*12+5"));
            Assert.That(node.ToString(), Is.EqualTo("((8 - (12 * 12)) + 5)"));
        }

        [Test]
        public void addition_and_factors()
        {
            var a = new Analyser();
            var node = a.Analyse(new StringTokenizer("8*12+12"));
            Assert.That(node.ToString(), Is.EqualTo("((8 * 12) + 12)"));
        }

        [Test]
        public void unary_minus_at_work()
        {
            var a = new Analyser();
            var node = a.Analyse(new StringTokenizer("12*-7"));
            Assert.That(node.ToString(), Is.EqualTo("(12 * -(7))"));
        }

        [Test]
        public void if_at_work()
        {
            var a = new Analyser();
            var node = a.Analyse(new StringTokenizer("3-7 ? 1+8 : (-4+1 ? -8 : -24 )"));
            Assert.That(node.ToString(), Is.EqualTo("((3 - 7) ? (1 + 8) : ((-(4) + 1) ? -(8) : -(24)))"));
        }

        [Test]
        public void variable()
        {
            var a = new Analyser();
            var node = a.Analyse(new StringTokenizer("a-7 ? 1+b : (-c+1 ? -8*a : -24 )"));
            Assert.That(node.ToString(), Is.EqualTo("((a - 7) ? (1 + b) : ((-(c) + 1) ? (-(8) * a) : -(24)))"));
        }
    }
}