using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public abstract class NodeVisitor
    {
        public Node VisitNode(Node n)
        {
            return n.Accept(this);
        }

        public virtual Node Visit(BinaryNode n)
        {
            var left = VisitNode(n.Left);
            var right = VisitNode(n.Right);
            return left != n.Left || right != n.Right
                ? new BinaryNode(n.OperatorType, left, right)
                : n;
        }

        public virtual Node Visit(ConstantNode n)
        {
            return n;
        }

        public virtual Node Visit(VariableNode n)
        {
            return n;
        }

        public virtual Node Visit(ErrorNode n)
        {
            return n;
        }

        public virtual Node Visit(IfNode n)
        {
            var condition = VisitNode(n.Condition);
            var whenTrue = VisitNode(n.WhenTrue);
            var whenFalse = VisitNode(n.WhenFalse);
            return condition!= n.Condition || whenTrue != n.WhenTrue || whenFalse != n.WhenFalse
                ? new IfNode(condition,whenTrue,whenFalse):
                n;
        }

        public virtual Node Visit(UnaryNode n)
        {
            var right = VisitNode(n.Right);
            return right != n.Right
                ? new UnaryNode(n.OperatorType, right)
                : n;
        }
    }
}