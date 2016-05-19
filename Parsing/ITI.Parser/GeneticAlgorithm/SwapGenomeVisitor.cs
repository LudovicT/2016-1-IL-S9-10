using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public class SwapGenomeVisitor : NodeVisitor
    {
        private int _currentExplorationCount;

        private TargetMode Mode { get; set; }

        private Node InteractingNode { get; set; }

        private int TargetIndex { get; set; }

        public Node SwapGenome(Node from, Node to)
        {
            Random r = new Random();
            return SwapGenome(from, to, r.Next(from.Count), r.Next(to.Count));
        }

        public Node SwapGenome(Node from, Node to, int fromIndex, int toIndex)
        {
            _currentExplorationCount = 0;
            InteractingNode = null;
            Random r = new Random();
            TargetIndex = fromIndex;
            Mode = TargetMode.Get;
            VisitNode(from);

            Node result = new Analyser().Analyse(new StringTokenizer(to.ToString()));

            Mode = TargetMode.Set;
            TargetIndex = toIndex;
            VisitNode(result);

            return result;
        }

        public override void Visit(BinaryNode n)
        {
            AquireTargetIfPossible(n);
            if (Mode == TargetMode.Get && InteractingNode != null)
            {
                _currentExplorationCount = 0;
                return;
            }

            var next = SetTarget();
            n.Left = next ?? n.Left;
            if (next != null)
            {
                _currentExplorationCount = 0;
                return;
            }
            VisitNode(n.Left);

            next = SetTarget();
            n.Right = next ?? n.Right;
            if (next != null)
            {
                _currentExplorationCount = 0;
                return;
            }
            VisitNode(n.Right);
        }

        public override void Visit(ConstantNode n)
        {
            AquireTargetIfPossible(n);
        }

        public override void Visit(VariableNode n)
        {
            AquireTargetIfPossible(n);
        }

        public override void Visit(IfNode n)
        {
            AquireTargetIfPossible(n);
            if (Mode == TargetMode.Get && InteractingNode != null)
            {
                _currentExplorationCount = 0;
                return;
            }

            var next = SetTarget();
            n.Condition = next ?? n.Condition;
            if (next != null)
            {
                _currentExplorationCount = 0;
                return;
            }
            VisitNode(n.Condition);

            next = SetTarget();
            n.WhenTrue = next ?? n.WhenTrue;
            if (next != null)
            {
                _currentExplorationCount = 0;
                return;
            }
            VisitNode(n.WhenTrue);

            next = SetTarget();
            n.WhenFalse = next ?? n.WhenFalse;
            if (next != null)
            {
                _currentExplorationCount = 0;
                return;
            }
            VisitNode(n.WhenFalse);
        }

        public override void Visit(UnaryNode n)
        {
            AquireTargetIfPossible(n);
            if (Mode == TargetMode.Get && InteractingNode != null)
            {
                _currentExplorationCount = 0;
                return;
            }

            var next = SetTarget();
            n.Right = next ?? n.Right;
            if (next != null)
            {
                _currentExplorationCount = 0;
                return;
            }
            VisitNode(n.Right);
        }

        private void AquireTargetIfPossible(Node n)
        {
            _currentExplorationCount++;
            if (Mode == TargetMode.Get && TargetIndex == _currentExplorationCount)
            {
                InteractingNode = n;
            }
        }

        private Node SetTarget()
        {
            if (Mode == TargetMode.Set && TargetIndex == _currentExplorationCount + 1)
            {
                return InteractingNode;
            }
            return null;
        }

        private enum TargetMode
        {
            Get,
            Set
        }
    }
}