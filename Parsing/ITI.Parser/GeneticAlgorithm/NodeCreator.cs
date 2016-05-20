using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace ITI.Parser
{
    public class NodeCreator
    {
        private readonly Random _random;
        private List<string> _variableNames;
        public int MaxVariable { get; }

        public NodeCreator(int maxVariable = 0)
            : this(new Random(), maxVariable)
        {
        }

        public NodeCreator(int seed, int maxVariable = 0)
            : this(new Random(seed), maxVariable)
        {
        }

        public NodeCreator(Random random, int maxVariable = 0)
        {
            _random = random;
            MaxVariable = maxVariable;
            _variableNames = Enumerable.Range('A', maxVariable).Select(x => (char)x + string.Empty).ToList();
        }

        public Node RandomNode(int maxDepth, int maxSize)
        {
            if (maxDepth == 1 || maxSize == 1)
            {
                return RandomConstantNode();
            }

            Node n;

            int kind = _random.Next(4);
            switch (kind)
            {
                case 0:
                    n = RandomBinaryNode(maxDepth - 1, maxSize - 1);
                    break;

                case 1:
                    n = RandomConstantNode();
                    break;

                case 2:
                    n = RandomUnaryNode(maxDepth - 1, maxSize - 1);
                    break;

                case 3:
                    n = MaxVariable == 0 ? RandomConstantNode() : RandomVariableNode();
                    break;

                default:
                    throw new InvalidOperationException("Invalid node creation.");
            }

            return n;
        }

        public Node RandomUnaryNode(int maxDepth, int maxCount)
        {
            return new UnaryNode(RandomUnaryNodeOperator(), RandomNode(maxDepth, maxCount));
        }

        public Node RandomIfNode(int maxDepth, int maxCount)
        {
            return new IfNode(RandomNode(maxDepth, maxCount), RandomNode(maxDepth, maxCount), RandomNode(maxDepth, maxCount));
        }

        public ConstantNode RandomConstantNode()
        {
            return new ConstantNode(_random.Next(1,101));
        }

        private Node RandomVariableNode()
        {
            int variableIndex = _random.Next(MaxVariable);
            return new VariableNode(_variableNames[variableIndex]);
        }

        public BinaryNode RandomBinaryNode(int maxDepth, int maxCount)
        {
            return new BinaryNode(RandomBinaryNodeOperator(), RandomNode(maxDepth, maxCount), RandomNode(maxDepth, maxCount));
        }

        public TokenType RandomBinaryNodeOperator()
        {
            int operatorType = _random.Next(4);
            switch (operatorType)
            {
                case 0:
                    return TokenType.Plus;

                case 1:
                    return TokenType.Minus;

                case 2:
                    return TokenType.Mult;

                case 3:
                    return TokenType.Div;

                default:
                    throw new InvalidOperationException("Invalid operator type");
            }
        }

        public TokenType RandomUnaryNodeOperator()
        {
            int operatorType = _random.Next(2);
            switch (operatorType)
            {
                case 0:
                    return TokenType.Plus;

                case 1:
                    return TokenType.Minus;

                default:
                    throw new InvalidOperationException("Invalid operator type");
            }
        }
    }
}