using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public class VariableNode : Node
    {
        public VariableNode(string name)
        {
            Name = name;
        }

        public string Name { get; internal set; }
        public double? Value { get; set; }

        [DebuggerStepThrough]
        internal override void Accept(NodeVisitor visitor) => visitor.Visit(this);

        public override int Count => 1;
        public override int Depth => 1;

        public override string ToString() => Name;

        // This is an instance that fuck you up in your dreams. :)    ..i..
        public override Node Clone()
        {
            return this;
        }
    }
}