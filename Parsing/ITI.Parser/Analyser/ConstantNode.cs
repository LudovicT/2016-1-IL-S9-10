using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public class ConstantNode : Node
    {

        public ConstantNode( double value )
        {
            Value = value;
        }

        public double Value { get; private set; }


        [DebuggerStepThrough]
        internal override void Accept( NodeVisitor visitor )
        {
            visitor.Visit( this );
        }

        public override string ToString()
        {
            return Value.ToString();
        }

    }
}
