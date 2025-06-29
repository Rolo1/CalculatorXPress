using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorXpress
{
    public class RightHand
    {
        private Compute functionGroup;
        private Numbers numb;
        private double result;
        public RightHand()//default
        {
            functionGroup = new Compute();
            numb = new Numbers();
            result = 0.0;
        }
        public void setResult(double value)
        {
            this.result = value;
        }

        /*type 0 = nothing, 1 = numeric, 2 = binaryOp, 3 = leftOp, 4 = rightOp, 5 = dot, 6 = leftBrace, 7 = rightBrace,
 * 8 = result, 9 = Pi, 10 = invert, 11 = toggle, 12 = validMinus, 13 = reciprocal, 14 = error , 15 = Binary Functions*/

        public List<Input> StripRight(List<Input> inputLst) //, Stack<int> T)
        {
            Stack<Input> RippedR = new Stack<Input>();
            double Rvalue = 0;
            bool Rop = false;
            bool signed = false;
            string RightOp = "";
            //test(inputLst, "Right, at the Start");

            foreach (Input guest in inputLst)
            {
                if (guest.getType() == 12)//the sign is BEFORE THE OPERATOR: -√, -Log, etc.
                {
                    signed = true; // guest.SignResult(); // Or unsign it if < 0
                    continue;
                }
                if (guest.getType() == 1 || guest.getType() == 8)
                {
                    if (Rop)
                    {
                        if(guest.NotNull() ) result = guest.getOutput();// numb.convert(guest.getItem());//
                        else result = numb.convert(guest.getItem());
                        Rvalue = calculate(result, RightOp);
                        if (signed) Rvalue = -Rvalue;
                        //display("Rvalue : " + Rvalue + " Rop : " + RightOp);
                        Results output = new Results(Rvalue);
                        Input input = new Input(output);
                        input.setType(1);   //the result from a Right operator and a number = double
                        RippedR.Push(input); //reversed 
                        Rop = false;
                        signed = false;
                        RightOp = "";
                    }
                    else
                    {
                        RippedR.Push(guest); //copy the number
                    }
                }
                else if (guest.getType() == 4) //pop this type off the type<int> stack
                {
                    Rop = true;
                    RightOp += guest.getItem();
                }
                else //the item is not a right operator
                {
                    Rop = false;
                    RippedR.Push(guest); //copy remaining items in reverse to another stack//   √3 + Log60 
                }
            }
            List<Input> temp = RippedR.ToList();
            temp.Reverse();
            //test(temp, "Right at the end");
            return temp;
        }
        public void test(List<Input> lst, string comment)
        {
            string test = "";
            foreach (Input guest in lst)
            {
                test += guest.getItem();
            }
            System.Windows.Forms.MessageBox.Show("Result R: " + test + " Items: " + lst.Count + " " + comment);
        }
        public double calculate(double number, string rOper) 
        {

            switch (rOper)
            {
                case "√":   //Right Operators -Operating on results are also 'left operators'
                    setResult(functionGroup.SquarerrotFcn(number));
            break;
                case "Sin":
                    setResult(functionGroup.SinFcn(number));
            break;
                case "Sinˉ¹":
                    setResult(functionGroup.SinInvFcn(number));
            break;
                case "Sinh":
                    setResult(functionGroup.SinhFcn(number));
            break;
                case "Sinhˉ¹":
                    setResult(functionGroup.SinhInvFcn(number));
            break;
                case "Cos":
                    setResult(functionGroup.CosFcn(number));
            break;
                case "Cosˉ¹":
                    setResult(functionGroup.CosInvFcn(number));
            break;
                case "Cosh":
                    setResult(functionGroup.CoshFcn(number));
            break;
                case "Coshˉ¹":
                    setResult(functionGroup.CoshInvFcn(number));
            break;
                case "Ta∩":
                    setResult(functionGroup.TanFcn(number));
            break;
                case "Ta∩ˉ¹":
                    setResult(functionGroup.TanInvFcn(number));
            break;
                case "Ta∩h":
                    setResult(functionGroup.TanhFcn(number));
            break;
                case "Ta∩hˉ¹":
                    setResult(functionGroup.TanhInvFcn(number));
            break;
                case "Log": //log base 10
                    setResult(functionGroup.Log10Fcn(number));
            break;
                case "Logˉ¹":
                    setResult(functionGroup.Log10InvFcn(number));
            break;
                case "ℓog₂":
                    setResult(functionGroup.Log2Fcn(number));
            break;
                case "ℓog₂ˉ¹":
                    setResult(functionGroup.Log2InvFcn(number));
            break;
                case "Ɩŋ": //no need inverse (e)
                    setResult(functionGroup.LognatFcn(number));
            break;
                case "e":  // eⁿ = Ɩŋˉ¹
                    setResult(functionGroup.EulerFcn(number));
            break;
                case "ˉ¹":

                    break;

        }

            //Form1.SetError(functionGroup.GetError());

                return result;
        }
        void display(String input)
        {
            System.Windows.Forms.MessageBox.Show(input);//End test
        }
    }
}
