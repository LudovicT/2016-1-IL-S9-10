using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public class VariableSetVisitor : NodeVisitor
    {
        private string _variableName;
        private double _value;

        public int VariableOccurence { get; set; }

        public void SetVariable(Node n, string variableName, double value)
        {
            VariableOccurence = 0;
            _variableName = variableName;
            _value = value;
            VisitNode(n);
        }

        public override void Visit(VariableNode n)
        {
            if (n.Name == _variableName)
            {
                n.Value = _value;
                VariableOccurence++;
            }
        }
    }
}