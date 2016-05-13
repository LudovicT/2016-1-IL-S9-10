using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public class UnaryNode : Node
    {

        public UnaryNode( TokenType operatorType, Node right )
        {
            OperatorType = operatorType;
            Right = right;
        }

        public TokenType OperatorType { get; private set; }
        public Node Right { get; private set; }

        [DebuggerStepThrough]
        internal override void Accept( NodeVisitor visitor )
        {
            visitor.Visit( this );
        }

        public override string ToString()
        {
            string op = null;
            switch( OperatorType )
            {
                case TokenType.Plus: op = "+"; break;
                case TokenType.Minus: op = "-"; break;
            }
            return op + "(" + Right.ToString() + ")";
        }

    }
}
