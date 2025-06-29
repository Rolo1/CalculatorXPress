using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorXpress
{
    public partial class Input
    {
        private Symbols symbol;
        private Functions function;
        private Results output;
        private Input input;
        private bool sym = false;
        private bool fcn = false;
        private int[] type;
        public Input()                              //DEFAULT
        { 
            this.symbol = (null);
            this.function = (null);
            this.output = (null);
            this.input = (null);
            this.sym = false;
            this.fcn = false;
            this.type = new int[4];
            for (int t = 0; t < 4; t++) this.type[t] = 0;
        }
        public Input(int[] aType) { this.type = aType; }
        public Input(Symbols aSymbol){setInput(aSymbol);}//parameter for symbol input
        public Input(Functions aFunction){setInput(aFunction); }//parameter for function
        public Input(Results aResult){setInput(aResult); }//parameter for result (output)
        public Input(Input anInput){setInput(anInput); }//clone an input 
        ~Input() { }//destructor

        public void setInput(Symbols newInput)
        {
            this.symbol = newInput;
            this.type = new int[4];
            for (int t = 0; t < 4; t++) this.type[t] = 0;
            if (!(symbol.GetSymbol() == '\0')) sym = true;
        }
        public void setInput(Functions newInput)
        {
            this.function = newInput;
            this.type = new int[4];
            for (int t = 0; t < 4; t++) this.type[t] = 0;
            if (!(function.GetFunction () == null)) fcn = true;
        }
        public void setInput(Results newResult)
        {
            this.type = new int[4];
            for (int t = 0; t < 4; t++) this.type[t] = 0;
            this.output = newResult;
        }
        public void SignResult() //Toggle sign of Result output
        {
            double value = output.GetOutput();
            output.SetOutput(-value);            
        }
        public void setInput(Input newInput)
        {
            this.input = newInput;
            this.symbol = (newInput.symbol);
            this.function = (newInput.function);
            this.output = (newInput.output);
            this.sym = newInput.sym;
            this.fcn = newInput.fcn;
            this.type = newInput.type;
        }
        public void setType(int t)
        {
            this.type[0] = t;
        }
        public void setMode(int t)
        {
            this.type[1] = t;
        }
        public void setPrior(int t)
        {
            this.type[2] = t;
        }
        public int getType()
        {
            return (this.type[0]);
        }
        public int[] typeArray()
        {
            return (this.type);
        }
        public int getMode() 
        {
            return (this.type[1]);
        }
        public int getPrior()
        {
            return (this.type[2]);
        }
        public char getSymbol()
        {
            if (!(symbol == null))
            {
                return symbol.GetSymbol();
            }
            else
            {
                return ('\0');
            }
        }
        public string getFunction()
        {
            if (!(function == null))
            {
                return function.GetFunction();
            }
            else
            {
                return ("");
            }
        }
        public double getOutput()
        {
            //bool notOutput = output == null;
            //System.Windows.Forms.MessageBox.Show("Output? :  " + notOutput);
            if (!(output == null)) return output.GetOutput();
            else return 0;
        }
        public string getItem()
        {
            if(!(fcn == false)) //(!(function.GetFunction() == null))
            {
                return function.GetFunction();
            }
            else if(!(sym == false)) //(!(symbol.GetSymbol() == '\0'))
            {
                return symbol.GetSymbol().ToString ();
            }
            else if(!(output == null))
            {
                return output.OutToString();
            }
            else
            {
                return ("");
            }
        }
        public bool NotNull() 
        {
            return (!(output == null));
        }
        public char getChar()
        {
            string str = getItem();
            int size = str.Length - 1;
            if (size > -1)
            {
                return (str[size]);
            }
            else
                return ('\0');
        }

    }//END CLASS

}//END NAMESPACE
