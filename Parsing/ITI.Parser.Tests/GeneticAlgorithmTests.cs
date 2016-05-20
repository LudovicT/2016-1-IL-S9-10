using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser.Tests
{
    [TestFixture]
    public class GeneticAlgorithmTests
    {
        public List<int[]> _values = new List<int[]>();
        private VariableSetVisitor variableSetVisitor = new VariableSetVisitor();
        private EvalVisitor evalVisitor = new EvalVisitor();

        [SetUp]
        public void Startup()
        {
            _values.Add(new[] { 1, 8, 300 });
            _values.Add(new[] { 2, 7, 500 });
            _values.Add(new[] { 3, 6, 700 });
            _values.Add(new[] { 4, 5, 900 });
            _values.Add(new[] { 5, 4, 1100 });
            _values.Add(new[] { 6, 3, 1300 });
        }

        [Test]
        public void best_fitness_level()
        {
            GeneticAlgorithm GA = new GeneticAlgorithm(0.1, 1, 1000, 5000, 8, 100, 10, test_function);
            var bestByGeneration = GA.Run();
        }

        private double test_function(Node n)
        {
            double fitness = 0;
            foreach (var value in _values)
            {
                variableSetVisitor.SetVariable(n, "A", value[0]);
                variableSetVisitor.SetVariable(n, "B", value[1]);
                evalVisitor.VisitNode(n);
                fitness -= Math.Abs(value[2] - evalVisitor.Result);
            }
            return fitness;
        }
    }
}