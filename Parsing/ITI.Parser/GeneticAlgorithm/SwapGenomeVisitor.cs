using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public class SwapGenomeVisitor : NodeVisitor
    {
        public Node Node1 { get; set; }
        public Node Node2 { get; set; }

        private Random _random;

        public SwapGenomeVisitor(Random random)
        {
            _random = random;
        }
    }
}