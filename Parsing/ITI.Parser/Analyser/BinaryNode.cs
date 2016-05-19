using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public class BinaryNode : Node
    {
        public BinaryNode(TokenType operatorType, Node left, Node right)
        {
            OperatorType = operatorType;
            Left = left;
            Right = right;
        }

        public TokenType OperatorType { get; internal set; }
        public Node Left { get; internal set; }
        public Node Right { get; internal set; }

        [DebuggerStepThrough]
        internal override void Accept(NodeVisitor visitor) => visitor.Visit(this);

        public override int Count => Left.Count + Right.Count + 1;
        public override int Depth => Math.Max(Left.Depth, Right.Depth) + 1;

        public override string ToString()
        {
            string op = null;
            switch (OperatorType)
            {
                case TokenType.Div: op = " / "; break;
                case TokenType.Mult: op = " * "; break;
                case TokenType.Plus: op = " + "; break;
                case TokenType.Minus: op = " - "; break;
            }
            return $"({Left}{op}{Right})";
        }
    }
}