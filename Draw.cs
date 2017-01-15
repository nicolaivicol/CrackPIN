using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrackPIN
{
    public class Draw
    {
        //int k;
        public int[] elements;
        public int[] count;

        //public Draw(int[] elements)
        public Draw(int k, int n)
        {
            //this.k = k;
            elements = new int[k];
            count = new int[n];
        }

        public void Set(int pos, int val)
        {
            elements[pos] = val;
        }

        public void Count()
        {
            foreach (int el in elements)
            {
                count[el]++;
            }
        }

        public override string ToString()
        {
            string s = "";
            foreach (int el in elements)
            {
                s += el;
            }
            return s;
        }

    }
}
