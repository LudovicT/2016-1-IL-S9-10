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

        private Random _random;

        public SwapGenomeVisitor(Random random)
        {
            _random = random;
        }

        public Node Swap(Node parent1, Node parent2)
        {
            _parent1 = parent1;
            _parent2 = parent2;
            _p1Pos = _random.Next(_parent1.Count);
            _p2Pos = _random.Next(_parent2.Count);
            return _parent1;
        }
    }
}