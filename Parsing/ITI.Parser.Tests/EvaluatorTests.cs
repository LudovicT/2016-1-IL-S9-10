using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ITI.Parser.Tests
{
    [TestFixture]
    public class EvaluatorTests
    {

        [TestCase( "3", 3.0 )]
        [TestCase( "3+8", 3.0+8.0)]
        [TestCase( "3*7/2", 3.0*7.0/2.0 )]
        [TestCase( "3712/(45+98)*12*(58/12)", 3712.0 / (45.0 + 98.0) * 12.0 *( 58.0 / 12.0 ) )]
        [TestCase( "37*(12+4)/(45+98/(4+5+68-8-7+10))*1+(41/9*7+6-72)+2*(5+8/1-2)", 37.0 * (12.0 + 4.0) / (45.0 + 98.0 / (4.0 + 5.0 + 68.0 - 8.0 - 7.0 + 10.0)) * 1.0 + (41.0 / 9.0 * 7.0 + 6.0 - 72) + 2.0 * (5.0 + 8.0 / 1.0 - 2.0) )]
        [TestCase( "37 ? 8 : 5", 8.0 )]
        [TestCase( "3+7 ? 1+8 : 4+1", 9.0 )]
        [TestCase( "3-7 ? 1+8 : (-4+1 ? -8 : -24 )", -24.0 )]
        public void test_evaluation( string text, double expectedResult )
        {
            Node n = new Analyser().Analyse( new StringTokenizer( text ) );

            EvalVisitor visitor = new EvalVisitor();
            visitor.VisitNode( n );
            double result = visitor.Result;
            Assert.That( result, Is.EqualTo( expectedResult ) );
        }

    }
}
