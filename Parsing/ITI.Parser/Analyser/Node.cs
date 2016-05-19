using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public abstract class Node
    {
        internal abstract void Accept(NodeVisitor visitor);

        public abstract int Count { get; }
        public abstract int Depth { get; }
        public virtual double Fitness { get; set; }
    }
}