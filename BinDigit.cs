using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorXpress
{
    class BinDigit
    {
        private char bit;
        private string Byte;
        private BinDigit bindigit;
        public BinDigit() { } //default
        public BinDigit(char aDigit) //input a bit
        {
            this.bit = aDigit; //parametric
        }
        public BinDigit(string digits)//input a series of bits (Bytes)
        {
            this.Byte = digits;
        }
        public BinDigit(BinDigit aBinDigit)//copy constr.
        {
            this.bindigit = aBinDigit;
        }
        ~BinDigit() { }//destroy
        public void SetBit(char aBit) { this.bit = aBit; }
        public char GetBit() { return (this.bit); }
        public string GetByte() { return this.Byte; }
        public void SetByte(string aByte) { this.Byte = aByte; }

    }
}
