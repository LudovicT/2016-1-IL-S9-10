using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public class DeepCopyVisitor : NodeVisitor
    {
        private Node _val;

        public Node Copy(Node n)
        {
            VisitNode(n);
            return _val;
        }
    }
}