using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser.Tests
{
    [TestFixture]
    public class NodeCreatorTests
    {
        [Test]
        public void test_evaluation()
        {
            NodeCreator creator = new NodeCreator(seed: 7, maxVariable: 0);
            for (int i = 0; i < 10000; i++)
            {
                var node = creator.RandomNode(50, 500);
                Assert.That(node.Depth, Is.LessThanOrEqualTo(50));
                Assert.That(node.Count, Is.LessThanOrEqualTo(500));
            }
        }
    }
}