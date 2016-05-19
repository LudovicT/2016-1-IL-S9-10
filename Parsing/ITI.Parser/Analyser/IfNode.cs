using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public class IfNode : Node
    {
        public IfNode(Node condition, Node whenTrue, Node whenFalse)
        {
            Condition = condition;
            WhenTrue = whenTrue;
            WhenFalse = whenFalse;
        }

        public Node Condition { get; internal set; }
        public Node WhenTrue { get; internal set; }
        public Node WhenFalse { get; internal set; }

        public override int Count => Condition.Count + WhenTrue.Count + WhenFalse.Count + 1;
        public override int Depth => Math.Max(Math.Max(Condition.Depth, WhenTrue.Depth), WhenFalse.Depth) + 1;

        [DebuggerStepThrough]
        internal override void Accept(NodeVisitor visitor) => visitor.Visit(this);

        public override string ToString()
        {
            return $"({Condition} ? {WhenTrue} : {WhenFalse})";
        }
    }
}