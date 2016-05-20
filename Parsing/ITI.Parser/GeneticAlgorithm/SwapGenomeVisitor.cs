using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public class SwapGenomeVisitor : NodeVisitor
    {
        private Node _parent1;
        private Node _parent2;
        private int _p1Pos;
        private int _p2Pos;
        private Node _node1;
        private int _currentPos;

        private Random _random;

        public SwapGenomeVisitor(Random random)
        {
            _random = random;
        }

        public Node Swap(Node parent1, Node parent2)
        {
            _currentPos = 0;
            _parent1 = parent1;
            _parent2 = parent2;
            _p1Pos = _random.Next(_parent1.Count);
            _p2Pos = _random.Next(_parent2.Count);
            VisitNode(parent1);
            _currentPos = 0;
            return VisitNode(parent2);
        }

        public override Node Visit(BinaryNode n)
        {
            if (_node1 != null && _currentPos == _p2Pos) return n;
            TrySetFirstNode(n);
            return base.Visit(n);
        }

        public override Node Visit(IfNode n)
        {
            if (_node1 != null && _currentPos == _p2Pos) return _node1;
            TrySetFirstNode(n);
            return base.Visit(n);
        }

        public override Node Visit(UnaryNode n)
        {
            if (_node1 != null && _currentPos == _p2Pos) return _node1;
            TrySetFirstNode(n);
            return base.Visit(n);
        }

        public override Node Visit(ConstantNode n)
        {
            if (_node1 != null && _currentPos == _p2Pos) return _node1;
            TrySetFirstNode(n);
            return base.Visit(n);
        }

        public override Node Visit(VariableNode n)
        {
            if (_node1 != null && _currentPos == _p2Pos) return _node1;
            TrySetFirstNode(n);
            return base.Visit(n);
        }

        public override Node Visit(ErrorNode n)
        {
            if (_node1 != null && _currentPos == _p2Pos) return _node1;
            TrySetFirstNode(n);
            return base.Visit(n);
        }

        private void TrySetFirstNode(Node n)
        {
            if (_node1 == null && _currentPos == _p1Pos) _node1 = n;
            _currentPos++;
        }
    }
}