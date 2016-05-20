using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public class MutationVisitor : NodeVisitor
    {
        private Random _random;
        private double _mutationRate;
        private NodeCreator _nodeCreator;
        private int _maxCount;
        private int _maxDepth;
        private bool HasMutation { get { return _random.NextDouble() <= _mutationRate; } }

        public MutationVisitor(NodeCreator nodeCreator, double mutationRate = 0.05, int maxDepth = 50, int maxSize = 500, int? seed = 7)
        {
            Init(nodeCreator, mutationRate, maxDepth, maxSize, seed);
        }

        public override Node Visit(BinaryNode n)
        {
            var node = TryMutate(n);
            if (node != n) return node;
            return base.Visit(n);
        }

        public override Node Visit(ConstantNode n)
        {
            var node = TryMutate(n);
            if (node != n) return node;
            return base.Visit(n);
        }

        public override Node Visit(IfNode n)
        {
            var node = TryMutate(n);
            if (node != n) return node;
            return base.Visit(n);
        }

        public override Node Visit(UnaryNode n)
        {
            var node = TryMutate(n);
            if (node != n) return node;
            return base.Visit(n);
        }

        public override Node Visit(VariableNode n)
        {
            var node = TryMutate(n);
            if (node != n) return node;
            return base.Visit(n);
        }

        private void Init(NodeCreator nodeCreator, double mutationRate, int maxDepth, int maxCount, int? seed)
        {
            _mutationRate = mutationRate;
            _maxCount = maxCount;
            _maxDepth = maxDepth;
            _random = seed.HasValue ? new Random(seed.Value) : new Random();
            _nodeCreator = nodeCreator;
        }

        private Node TryMutate(Node n)
        {
            return HasMutation
                ? _nodeCreator.RandomNode(_maxDepth, _maxCount)
                : n;
        }
    }
}