using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser.Tests
{
    [TestFixture]
    public class TokenizerTests
    {
        [TestCase("3")]
        [TestCase("3 8")]
        [TestCase("3 8 12 8787 3712")]
        public void list_of_numbers(string toParse)
        {
            var t = new StringTokenizer(toParse);
            List<double> values = new List<double>();
            t.GetNextToken();
            while (t.CurrentToken != TokenType.EndOfInput)
            {
                Assert.That(t.CurrentToken == TokenType.Number);
                double v;
                Assert.That(t.MatchDouble(out v));
                values.Add(v);
            }
            CollectionAssert.AreEqual(toParse.Split(' ').Select(double.Parse).ToList(), values);
        }

        [TestCase("a")]
        [TestCase("a b")]
        [TestCase("a b cd efgh ijkl")]
        public void list_of_variable(string toParse)
        {
            var t = new StringTokenizer(toParse);
            List<string> values = new List<string>();
            t.GetNextToken();
            while (t.CurrentToken != TokenType.EndOfInput)
            {
                Assert.That(t.CurrentToken == TokenType.Variable);
                string v;
                Assert.That(t.MatchString(out v));
                values.Add(v);
            }
            CollectionAssert.AreEqual(toParse.Split(' ').ToList(), values);
        }
    }
}