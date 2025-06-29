using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorXpress
{
    public class SubTerm
    {
        private Compute comp;
        private Numbers numb;
        private int index;

        public SubTerm()
        {
            comp = new Compute();
            numb = new Numbers();
            index = 0;
        }
        public void setIndex(int idx) 
        {
            this.index = idx;
        }
        public int getIndex()
        {
            return index;
        }
        /*type 0 = nothing, 1 = numeric, 2 = binaryOp, 3 = leftOp, 4 = rightOp, 5 = dot, 6 = leftBrace, 7 = rightBrace,
 * 8 = result, 9 = Pi, 10 = invert, 11 = toggle, 12 = validMinus, 13 = reciprocal, 14 = error */

        public List<Input> GetInner(List<Input> InList, int start)//returns an inner list of subterm at a time
        {
            List<Input> RippedTerm = new List<Input>();
            int stop = 0;

            for (int i = start; i < InList.Count; i++)
            {
               if (InList.ElementAt(i).getType() == 6) start = i + 1;
               if (InList.ElementAt(i).getType() == 7)
                {
                    stop = i;
                    setIndex(i);
                    break;
                }
            }

                for (int j = start; j < stop; j++)
                {
                    RippedTerm.Add(InList.ElementAt(j));
                }

            return RippedTerm;
        }
    }
}
