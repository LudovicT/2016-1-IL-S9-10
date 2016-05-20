using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser.Tests
{
    [TestFixture]
    public class GeneticAlgorithmTests
    {
        public List<int[]> _values = new List<int[]>();

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
            GeneticAlgorithm GA = new GeneticAlgorithm(0.25, 0.5, 100, 50000, 10, 50, 7, test_function);
            GA.Elitism = true;
            GA.ReverseComparison = false;
            var bestByGeneration = GA.Run();
        }

        private double test_function(GeneticAlgorithm origin, Node n)
        {
            Dictionary<string, double> dic = new Dictionary<string, double>();
            EvalVisitor evalVisitor = new EvalVisitor();
            double fitness = 0;
            if (n.Count > 50)
                n.Fitness -= n.Count * 5;
            if (n.Depth > 8)
                n.Fitness -= n.Depth * 50;
            foreach (var value in _values)
            {
                dic.Add("A", value[0]);
                dic.Add("B", value[1]);
                var result = evalVisitor.EvalWithVariable(n, dic);
                fitness -= Math.Abs(value[2] - ((ConstantNode)result).Value);
                dic.Clear();
            }

            return double.IsNaN(fitness) || double.IsInfinity(fitness) ? int.MinValue : fitness;
        }
    }
}