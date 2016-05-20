using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public class EvalVisitor : NodeVisitor
    {
        private Dictionary<string, double> _values = new Dictionary<string, double>();

        public EvalVisitor()
        {

        }

        public Node EvalWithVariable(Node n, Dictionary<string, double> values)
        {
            _values = values;
            return VisitNode(n);
        }

        public override Node Visit(IfNode n)
        {
            var c = VisitNode(n.Condition);
            ConstantNode cN = c as ConstantNode;
            if( cN != null )
            {
                return cN.Value >= 0 ? VisitNode(n.WhenTrue) : VisitNode(n.WhenFalse);
            }

            var t = VisitNode(n.WhenTrue);
            var f = VisitNode(n.WhenFalse);

            return c != n.Condition || t != n.WhenTrue || f != n.WhenFalse
                ? new IfNode(c, t, f) :
                n;
        }

        public override Node Visit(BinaryNode n)
        {
            var left = VisitNode(n.Left);

            var right = VisitNode(n.Right);

            var vLeft = left as VariableNode;
            var vRight = right as VariableNode;

            if( vLeft != null && vRight != null && vRight.Name == vLeft.Name )
            {
                switch (n.OperatorType)
                {
                    case TokenType.Div: return new ConstantNode(1);
                    case TokenType.Plus: return new BinaryNode(TokenType.Mult, new ConstantNode(2), vLeft );
                    case TokenType.Minus: return new ConstantNode(0);
                }
            }

            var cLeft = left as ConstantNode;
            var cRight = right as ConstantNode;

            if (cLeft != null && cRight != null)
            {
                switch (n.OperatorType)
                {
                    case TokenType.Mult: return new ConstantNode(cLeft.Value * cRight.Value);
                    case TokenType.Div: return new ConstantNode(cLeft.Value / cRight.Value);
                    case TokenType.Plus: return new ConstantNode(cLeft.Value + cRight.Value);
                    case TokenType.Minus: return new ConstantNode(cLeft.Value - cRight.Value);
                }
            }

            return left != n.Left || right != n.Right
                ? new BinaryNode(n.OperatorType, left, right)
                : n;
        }

        public override Node Visit(UnaryNode n)
        {
            Node right = VisitNode(n.Right);

            var uRight = right as UnaryNode;
            if( uRight != null )
            {
                if( n.OperatorType == TokenType.Minus )
                {
                    return Visit( uRight.OperatorType == TokenType.Minus 
                        ? new UnaryNode(TokenType.Plus, uRight.Right) 
                        : new UnaryNode(TokenType.Minus, uRight.Right) );
                }
                else if (n.OperatorType == TokenType.Plus)
                {
                    return Visit(uRight.OperatorType == TokenType.Minus
                        ? new UnaryNode(TokenType.Minus, uRight.Right)
                        : new UnaryNode(TokenType.Plus, uRight.Right));
                }
            }

            var cRight = right as ConstantNode;
            if (cRight != null)
            {
                return n.OperatorType == TokenType.Minus ? new ConstantNode(-cRight.Value) : new ConstantNode(cRight.Value);
            }
            return right != n.Right
                ? new UnaryNode(n.OperatorType, right)
                : n;
        }

        public override Node Visit(ConstantNode n)
        {
            return n;
        }

        public override Node Visit(VariableNode n)
        {
            double value;
            if (_values.TryGetValue(n.Name, out value))
            {
                return new ConstantNode(_values[n.Name]);
            }
            return n;
        }
    }
}