using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public class GenomeComparer : IComparer<Node>
    {
        public int Compare(Node x, Node y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    return 0;
                }
                return -1;
            }
            return x.Fitness.CompareTo(y.Fitness);
        }
    }
}