using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public class EvalVisitor : NodeVisitor
    {
        double _currentValue;

        public double Result { get { return _currentValue; } }

        public override void Visit( IfNode n )
        {
            VisitNode( n.Condition );
            VisitNode( _currentValue >= 0 ? n.WhenTrue : n.WhenFalse );
        }

        public override void Visit( BinaryNode n )
        {
            VisitNode( n.Left );
            var left = _currentValue;
            VisitNode( n.Right );
            var right = _currentValue;
            switch( n.OperatorType )
            {
                case TokenType.Mult: _currentValue = left * right; break;
                case TokenType.Div: _currentValue = left / right; break;
                case TokenType.Plus: _currentValue = left + right; break;
                case TokenType.Minus: _currentValue = left - right; break;
            }
        }

        public override void Visit( UnaryNode n )
        {
            VisitNode( n.Right );
            _currentValue = -_currentValue;
        }

        public override void Visit( ConstantNode n )
        {
            _currentValue = n.Value;
        }


    }
}
