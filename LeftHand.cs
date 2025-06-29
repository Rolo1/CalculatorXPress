using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorXpress
{

    public class LeftHand 
    {
        //private String leftOperator;
        private Compute functionGroup;
        private Numbers numb;
        private double result;
        public LeftHand()
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
        public List<Input> StripLeft(List<Input> inputLst) //, Stack<int> T)
        {
            Stack<Input> RippedL = new Stack<Input>(); 
            double Lvalue = 0;
            double value = 0;
            bool Lop = false;
            string LeftOp = "";
            //test(inputLst, "On start");

            for (int i = inputLst .Count - 1; i > -1; i--)
            {
                //System.Windows.Forms.MessageBox.Show("Last: " + inputLst.ElementAt(inputLst.Count - 1));
                if (inputLst.ElementAt(i).getType() == 1 || inputLst.ElementAt(i).getType() == 8)
                        {
                            if (Lop)
                                {
                                    //System.Windows.Forms.MessageBox.Show("Guest #(Lop): " + inputLst.ElementAt(i).getItem());
                                    if(inputLst.ElementAt(i).NotNull()) value = inputLst.ElementAt(i).getOutput();
                                    else value = numb.convert(inputLst.ElementAt(i).getItem());// 
                                    Lvalue = calculate(value, LeftOp);
                                    //System.Windows.Forms.MessageBox.Show("Value: " + value + " Left Oper : " + LeftOp );
                                    Results output = new Results(Lvalue);
                                    Input input = new Input(output);
                                    input.setType(1);   //the result from a left operator and a number = double
                                    RippedL.Push(input); //reversed 
                                    LeftOp = "";
                                    Lop = false;      
                                }
                            else
                                {
                                    RippedL.Push(inputLst.ElementAt(i)); //copy the number
                                }
                    }
                    else if(inputLst.ElementAt(i).getType() == 3) //pop this type off the type<int> stack
                        {
                            Lop = true;
                            LeftOp += inputLst.ElementAt(i).getItem();
                    //System.Windows.Forms.MessageBox.Show("Lop " + LeftOp);
                }
                    else //the item is not a left operator
                        {
                            Lop = false;
                            RippedL.Push(inputLst.ElementAt(i)); //copy remaining items in reverse to another stack//   25 + 2² 
                        }
                }
            //test(RippedL, "At the end");
            return RippedL.ToList();
        }
        public void test(List<Input> lst, string comment)
        {
            string test = "";
            foreach (Input guest in lst)
            {
                test += guest.getItem();
            }
            System.Windows.Forms.MessageBox.Show("List left: " + test + " " + comment);
        }
        public void test(Stack<Input> stk, string comment)
        {
            string test = "";
            foreach (Input guest in stk)
            {
                test += guest.getItem();
            }
            System.Windows.Forms.MessageBox.Show("Stack Left: " + test + " " + comment);
        }
        public double calculate(double number, String Left)
        {
            switch (Left)
            {
                case "!":
                    setResult(functionGroup.FactorialFcn(number));
                    break;
                case "%":
                    setResult(functionGroup.PercentFcn(number));
                    break;
                case "²":
                    setResult(functionGroup.SquareFcn(number));
                    break;
                case "√":   //from here, Right Operators -Operating on results from previous operation become 'left operators'
                    setResult(functionGroup.SquarerrotFcn(number));
                    break;
                case "Sin":
                    setResult(functionGroup.SinFcn(number));
                    break;
                case "ˉ¹Sin":
                    setResult(functionGroup.SinInvFcn(number));
                    break;
                case "Sinh":
                    setResult(functionGroup.SinhFcn(number));
                    break;
                case "ˉ¹Sinh":
                    setResult(functionGroup.SinhInvFcn(number));
                    break;
                case "Cos":
                    setResult(functionGroup.CosFcn(number));
                    break;
                case "ˉ¹Cos":
                    setResult(functionGroup.CosInvFcn(number));
                    break;
                case "Cosh":
                    setResult(functionGroup.CoshFcn(number));
                    break;
                case "ˉ¹Cosh":
                    setResult(functionGroup.CoshInvFcn(number));
                    break;
                case "Ta∩":
                    setResult(functionGroup.TanFcn(number));
                    break;
                case "ˉ¹Ta∩":
                    setResult(functionGroup.TanInvFcn(number));
                    break;
                case "Ta∩h":
                    setResult(functionGroup.TanhFcn(number));
                    break;
                case "ˉ¹Ta∩h":
                    setResult(functionGroup.TanhInvFcn(number));
                    break;
                case "Log": //log base 10
                    setResult(functionGroup.Log10Fcn(number));
                    break;
                case "ˉ¹Log":
                    setResult(functionGroup.Log10InvFcn(number));
                    break;
                case "ℓog₂":
                    setResult(functionGroup.Log2Fcn(number));
                    break;
                case "ˉ¹ℓog₂":
                    setResult(functionGroup.Log2InvFcn(number));
                    break;
                case "Ɩŋ": //no need inverse (e)
                    setResult(functionGroup.LognatFcn(number));
                    break;
                case "e":  // eⁿ = Ɩŋˉ¹
                    setResult(functionGroup.EulerFcn(number));
                    break;
            }
            Form1.SetError(functionGroup.GetError());
            return result;
        }
    }
}
