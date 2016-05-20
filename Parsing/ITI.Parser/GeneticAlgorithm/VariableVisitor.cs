using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public class VariableVisitor : NodeVisitor
    {
        private string _variableName;

        public int VariableOccurence { get; set; }

        public void SetVariable(Node n, string variableName)
        {
            VariableOccurence = 0;
            _variableName = variableName;
            VisitNode(n);
        }

        public override Node Visit(VariableNode n)
        {
            if (n.Name == _variableName)
            {
                VariableOccurence++;
            }
            return n;
        }
    }
}