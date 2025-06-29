using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorXpress
{
    public partial class Functions //: Input
    {
        private string function;
        public Functions() { function = null; } //default

        public Functions(string aFunction){function = aFunction;} //parametric

        public Functions(Functions newFunction){this.function = newFunction.function;}//clone

        ~Functions() { }    //Destructor
        public void setFunction(string aFunction){this.function = aFunction;}//setter

        public string GetFunction(){return (function);}//getter

    }
}
