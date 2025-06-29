using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorXpress
{
    public class Results
    {
        private double output;

        public Results() { output = 0.0; } //default
        public Results(double value) { this.output = value; }//parametric
        public Results(Results aResult){this.output = aResult.output;}//copy(clone)
        ~Results() { } //destroy
        public void SetOutput(double result)
        {
            this.output = result;
        }
        public double GetOutput()
        {
            return this.output;
        }
        public string OutToString()//override
        {
            return (output.ToString());
        }
    }
}
