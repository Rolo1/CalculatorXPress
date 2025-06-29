using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*Sol 1: this class should store the numeric strings, operator chars, function names
 * as units, so when it Push, it stores a number, arithmetic operator, or function name.
 * When it Pop, it should check the type of input: if a number removes 1, if a function name ("Cos"),
 * it then removes the whole name. Before action, it should peek either type: numeric, or char.
 * If is a char, it should delete the whole set.
 * Sol 2: It stores everything as string, Pushes a string, and Pops a string, even for a single digit.
 * */
namespace CalculatorXpress
{

    class Stack 
    {
        private Numbers aDigit;
        private Symbols aChar;
        private Functions aString;

        public Stack() { }

        public Stack(Numbers aNumber, Symbols aChars, Functions aStrings)
        {
            this.aDigit = aNumber;
            this.aChar = aChars;
            this.aString = aStrings;
        }

        public Stack(Stack newStack)
        {
            this.aDigit = newStack.aDigit;
            this.aChar = newStack.aChar;
            this.aString = newStack.aString;
        }

        void PushNumber()
        {

        }
    }
}
