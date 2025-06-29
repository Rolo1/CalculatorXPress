using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorXpress
{
    class InputBinary : Input //setType, getType, setMode, getItem, etc.
    {
        private BinDigit Bit;
        public void SetInput(BinDigit aBit)
        {
            this.Bit = aBit;
        }
        public BinDigit GetBit()//return the object (BinDigit)
        {
            return this.Bit;
        }
        public char GetItem()//return the actual char
        {
            return Bit.GetBit();
        }
    }
}
