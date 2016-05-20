using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public class EvalVisitor : NodeVisitor
    {
        private double _currentValue;
        private Dictionary<string, double> _values;

        public double Result => _currentValue;

        public void EvalWithVariable(Node n, Dictionary<string, double> values)
        {
            _values = values;
            VisitNode(n);
        }

        public override Node Visit(IfNode n)
        {
            VisitNode(n.Condition);
            VisitNode(_currentValue >= 0 ? n.WhenTrue : n.WhenFalse);
            return n;
        }

        public override Node Visit(BinaryNode n)
        {
            VisitNode(n.Left);
            var left = _currentValue;
            VisitNode(n.Right);
            var right = _currentValue;
            switch (n.OperatorType)
            {
                case TokenType.Mult: _currentValue = left * right; break;
                case TokenType.Div: _currentValue = left / right; break;
                case TokenType.Plus: _currentValue = left + right; break;
                case TokenType.Minus: _currentValue = left - right; break;
            }
            return n;
        }

        public override Node Visit(UnaryNode n)
        {
            VisitNode(n.Right);
            _currentValue = n.OperatorType == TokenType.Minus ? -_currentValue : _currentValue;
            return n;
        }

        public override Node Visit(ConstantNode n)
        {
            _currentValue = n.Value;
            return n;
        }

        public override Node Visit(VariableNode n)
        {
            _currentValue = _values[n.Name];
            return n;
            //if (n.Value == null) throw new InvalidOperationException($"Variable {n.Name} wasn't set.");
            //_currentValue = n.Value.Value;
        }
    }
}