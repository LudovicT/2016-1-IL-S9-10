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
        private int _maxExplorationCount;
        private int _currentExplorationCount;
        private double _mutationRate;
        private int _mutationPlace;
        private NodeCreator _nodeCreator;
        private Node _rootNode;
        private int _maxCount;
        private int _maxDepth;
        private bool _hasMutation;
        private Node _node;
        private bool HasMutation { get { return _random.NextDouble() <= _mutationRate; } }

        public MutationVisitor(NodeCreator nodeCreator, double mutationRate = 0.05, int maxDepth = 50, int maxSize = 500, int? seed = 7)
        {
            Init(nodeCreator, mutationRate, maxDepth, maxSize, seed);
        }

        public override Node Visit(BinaryNode n)
        {
            return TryMutate(n);
        }

        public override Node Visit(ConstantNode n)
        {
            return TryMutate(n);
        }

        public override Node Visit(IfNode n)
        {
            return TryMutate(n);
        }

        public override Node Visit(UnaryNode n)
        {
            return TryMutate(n);
        }

        public override Node Visit(VariableNode n)
        {
            return TryMutate(n);
        }

        private void Init(NodeCreator nodeCreator, double mutationRate, int maxDepth, int maxCount, int? seed)
        {
            _mutationRate = mutationRate;
            _maxCount = maxCount;
            _maxDepth = maxDepth;
            _random = seed.HasValue ? new Random(seed.Value) : new Random();
            _hasMutation = _random.NextDouble() <= _mutationRate;
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