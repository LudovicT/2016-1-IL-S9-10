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
        bool _reverse;
        public GenomeComparer(bool reverse)
        {
            _reverse = reverse;
        }

        public int Compare(Node x, Node y)
        {
            var a = _reverse ? y : x;
            var b = _reverse ? x : y;

            if (a == null)
            {
                if (b == null)
                {
                    return 0;
                }
                return -1;
            }

            if ((double.IsInfinity(a.Fitness) || double.IsNaN(a.Fitness)) && (double.IsInfinity(b.Fitness) || double.IsNaN(b.Fitness)))
            {
                return 0;
            }
            if (double.IsInfinity(a.Fitness) || double.IsNaN(a.Fitness))
            {
                return -1;
            }
            if (double.IsInfinity(b.Fitness) || double.IsNaN(b.Fitness))
            {
                return 1;
            }

            return a.Fitness.CompareTo(b.Fitness);
        }
    }
}