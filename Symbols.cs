using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Stores characters, or array[char]
 * */
namespace CalculatorXpress
{
    public partial class Symbols //: Input
    {
        private char symbol;
        //protected const int entry = 1;

        public Symbols() { this.symbol = ('\0'); }//default

        public Symbols(char aSymbol){symbol = aSymbol;}//parametric
        public Symbols(Symbols newSymbol){this.symbol = newSymbol.symbol;}//clone
        ~Symbols() { }  //Destructor
        public void SetSymbol(char aSymbol)
        {
            this.symbol = aSymbol;
        }

        public char GetSymbol()
        {
            return (symbol);
        }
        //public static int inputType()
        //{
        //    return (entry);
        //}
    }
}
