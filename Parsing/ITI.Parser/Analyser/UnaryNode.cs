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
        public UnaryNode(TokenType operatorType, Node right)
        {
            if (operatorType != TokenType.Minus && operatorType != TokenType.Plus) throw new ArgumentException(nameof(operatorType));
            OperatorType = operatorType;
            Right = right;
        }

        public TokenType OperatorType { get; internal set; }

        public Node Right { get; internal set; }

        [DebuggerStepThrough]
        internal override void Accept(NodeVisitor visitor) => visitor.Visit(this);

        public override int Count => Right.Count + 1;
        public override int Depth => Right.Depth + 1;

        public override string ToString()
        {
            string op = null;
            switch (OperatorType)
            {
                case TokenType.Plus: return Right.ToString();
                case TokenType.Minus: return $"-({Right})";
            }
            return $"{op}({Right})";
        }

        public override Node Clone()
        {
            return new UnaryNode(OperatorType, Right.Clone());
        }
    }
}