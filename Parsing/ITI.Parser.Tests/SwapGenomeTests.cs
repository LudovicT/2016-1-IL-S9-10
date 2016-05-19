using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser.Tests
{
    [TestFixture]
    public class SwapGenomeTests
    {
        [TestCase("3+8", "3+8")]
        [TestCase("3*7/2", "3*7/2")]
        [TestCase("3-7 ? 1+8 : (-4+1 ? -8 : -24 )", "3712/(45+98)*12*(58/12)")]
        [TestCase("37*(12+4)/(45+98/(4+5+68-8-7+10))*1+(41/9*7+6-72)+2*(5+8/1-2)", "3712/(45+98)*12*(58/12)")]
        public void test_evaluation(string text, string text2)
        {
            Node n = new Analyser().Analyse(new StringTokenizer(text));
            Node n2 = new Analyser().Analyse(new StringTokenizer(text2));
            Random r = new Random(7);

            SwapGenomeVisitor genomeSwapper = new SwapGenomeVisitor();
            Node n3 = genomeSwapper.SwapGenome(n, n2, r.Next(n.Count), r.Next(n2.Count));

            EvalVisitor visitor = new EvalVisitor();
            visitor.VisitNode(n2);
            double result = visitor.Result;
            visitor.VisitNode(n3);
            double resultBase = visitor.Result;
            Assert.That(result, Is.Not.EqualTo(resultBase));
        }
    }
}