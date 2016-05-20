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
        private VariableVisitor variableVisitor = new VariableVisitor();
        private EvalVisitor evalVisitor = new EvalVisitor();
        Dictionary<string, double> dic = new Dictionary<string, double>();

        [SetUp]
        public void Startup()
        {
            _values.Add(new[] { 6, 6, 100012 });
            _values.Add(new[] { 1, 1, 100002 });
            _values.Add(new[] { 2, 2, 100004 });
            _values.Add(new[] { 3, 3, 100006 });
            _values.Add(new[] { 4, 4, 100008 });
            _values.Add(new[] { 5, 5, 100010 });
        }

        [Test]
        public void best_fitness_level()
        {
            GeneticAlgorithm GA = new GeneticAlgorithm(0.25, 0.1, 500, 5000, 10, 1000, 42, test_function);
            GA.Elitism = true;
            GA.ReverseComparison = true;
            var bestByGeneration = GA.Run();
        }

        private double test_function(GeneticAlgorithm origin, Node n)
        {
            double fitness = 0;
            foreach (var value in _values)
            {
                dic.Add("A", value[0]);
                dic.Add("B", value[1]);
                evalVisitor.EvalWithVariable(n, dic);
                fitness -= Math.Abs(value[2] - evalVisitor.Result);
                dic.Clear();
            }
            return fitness;
        }
    }
}