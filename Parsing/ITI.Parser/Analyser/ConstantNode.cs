using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public class ConstantNode : Node
    {
        public ConstantNode(double value)
        {
            Value = value;
        }

        public double Value { get; internal set; }

        [DebuggerStepThrough]
        internal override void Accept(NodeVisitor visitor) => visitor.Visit(this);

        public override int Count => 1;
        public override int Depth => 1;

        public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);

        public override Node Clone()
        {
            return new ConstantNode(Value);
        }
    }
}