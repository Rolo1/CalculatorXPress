using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorXpress
{
    public class BinaryOp
    {
        private Compute functionGroup;
        private Numbers numb;
        private double result;

        public BinaryOp()
        {
            functionGroup = new Compute();
            numb = new Numbers();
            result = 0.0;
        }
        
        public List<Input> StripBin(List<Input> bList) //Resulting List may still contain operators
        {
            List<Input> StrippedBin = new List<Input>();
            int left = 0;
            Input temp = new Input();
            bool foundBin = false;
            double value = 0;
            double L = 0;
            string BinOp = "";
            //test(bList, "Bin functions At start");
            //types(bList, "functions On start");
            foreach (Input guest in bList)
            {
                double R = 0;
                int type = guest.getType();
                if (!(type == 2 && !(guest.getItem() == "+" || guest.getItem() == "-") || type == 1 || type == 8))
                {
                    foundBin = false;
                    Results output = new Results(value);
                    temp = new Input(output);
                    temp.setType(1);
                    if (left > 0) StrippedBin.Add(temp);
                    StrippedBin.Add(guest);
                    left = 0;
                }
                else if (type == 2)
                {
                    foundBin = true;
                    BinOp = guest.getItem();
                }
                else if (type == 1 || type == 8)
                {
                    if (foundBin)
                    {
                        if (guest.NotNull()) R = guest.getOutput();
                        else R = numb.convert(guest.getItem());
                        //display("Found Binary : " + BinOp + " result : " + value + " R : " + R);
                        switch (BinOp)
                        {
                            case "x":
                                value = value * R;
                                break;
                            case "/":
                                if (!(R == 0)) value = value / R;
                                else Form1.SetError(7);
                                break;
                            default:
                                break;
                        }
                        Results output = new Results(value);
                        temp = new Input(output);
                        temp.setType(1);
                    } //input is number and function on the left
                    else //input is number BUT other type function was found
                    {
                        if (guest.NotNull()) L = guest.getOutput();
                        else L = numb.convert(guest.getItem());//numb.convert();
                        value = L;
                        left++; //number on the left
                        temp = guest;
                    }
                }
            }
            if (left > 0) StrippedBin.Add(temp);
            //test(StrippedBin, "Bin functions At the end");
            return StrippedBin;
        }
        public List<Input> StripBin2(List<Input> bList)//Resulting List may NOT contain other than plain numbers
        {
            List<Input> StrippedBin = new List<Input>();
            bool foundBin = false;
            double value = 0;
            double L = 0;
            string BinOp = "";
            //test(StrippedBin, "Bin functions At Start");

            foreach (Input guest in bList)//12 + 2 - 3 x 11 / 16, -6, 740, 2.5E+6 x 4
            {
                double R = 0;
                if (guest.getType() == 2)
                {
                    foundBin = true;
                    BinOp = guest.getItem();
                }
                else if (guest.getType() == 1 || guest.getType() == 8)
                {
                    if (foundBin)
                    {
                        foundBin = false;
                        if (guest.NotNull()) R = guest.getOutput();
                        else R = numb.convert(guest.getItem());
                        //display("Found Binary => result : " + value + " R : " + R);
                        switch (BinOp)
                        {
                            case "-":
                                value = value - R;
                                break;
                            case "+":
                                value = value + R;
                                break;
                        }
                    }
                    else //no binary operator found
                    {
                        if (guest.NotNull()) L = guest.getOutput();
                        else L = numb.convert(guest.getItem());//numb.convert();
                        value = L;
                        //display("Guest bin not found: " + result);
                        continue;
                    }
                } //end if(type == 1 || type == 8)
            }//end foreach

            Results output = new Results(value);
            Input newInput = new Input(output);
            newInput.setType(1);
            StrippedBin.Add(newInput);
            //test(StrippedBin, "Bin functions At the end");
            return StrippedBin;
        }

        public List<Input> StripBinFunc(List<Input> bList)//Resulting List may contain lower precedence operators
        {
            List<Input> StrippedBinF = new List<Input>();
            int left = 0; 
            Input temp = new Input();
            bool foundBin = false;
            double value = 0;
            double L = 0;
            string BinOp = "";
            //test(bList, "Bin funtions At start");
            //types(bList, "funtions On start");
            foreach (Input guest in bList)
            {
                //display("Guest: " + guest.getItem() + "   Left: " + left + "  Value : " + value + "   Temp : " + temp.getItem ());
                double R = 0;
                int type = guest.getType();
                if (!(type == 15 || type == 1 || type == 8))//not a binary function or number
                {
                    //display("Left : " + left + "  Guest :  " + guest.getItem() + "  Temp : " + temp.getItem());
                    foundBin = false;
                    Results output = new Results(value);
                    temp = new Input(output);
                    temp.setType(1);
                    if (left > 0) StrippedBinF.Add(temp);
                    StrippedBinF.Add(guest);
                    left = 0; //reset left after storing it
                    //test(bList, "   Loop");
                }
                else if (type == 15)
                {
                    foundBin = true;
                    BinOp = guest.getItem();
                }
                else if (type == 1 || type == 8)
                {
                    if (foundBin)
                    {
                        if (guest.NotNull()) R = guest.getOutput();
                        else R = numb.convert(guest.getItem());
                        switch (BinOp)
                        {
                            case "^":
                                if (R < 1 && R % 2 == 0 && value < 0) Form1.SetError(2);
                                else value = Math.Pow(value, R); //Power series need extra routines
                                break;
                            case "Mod":
                                if (R == 0) Form1.SetError(7);
                                else value = value % R;
                                break;
                            case "E+":
                                value = value * Math.Pow(10, R);
                                break;
                            case "E-":
                                value = value * (1 / Math.Pow(10, R));
                                break;
                            default:
                                break;
                        }
                        //display("Found Binary : " + BinOp + " Value : " + value + " R : " + R + "   Temp : " + temp.getItem());
                        Results output = new Results(value);
                        temp = new Input(output);
                        temp.setType(1);
                    } //end foundBin -fetch R-operand, compute, save temp
                    else //not foundBin -save L-operand to value and to temp
                    {
                        if (guest.NotNull()) L = guest.getOutput();
                        else L = numb.convert(guest.getItem());//numb.convert();
                        value = L;
                        left++; //number on the left
                        temp = guest;
                        //display("Before finding a function, L = " + L);
                    }//The guest is a number BUT Not found a function
                }//End if(number)
            }//End foreach()
            if (left > 0) StrippedBinF.Add(temp);
            //test(StrippedBinF, "Bin funtions At the end");
            return StrippedBinF;
        }
        public double calculate(double L, string BinOp, double R)
        {
            double value = 0;
            switch (BinOp)
            {
                case "^":
                    value = Math.Pow(L, R);
                    break;
                case "Mod":
                    value = L % R;
                    break;
                case "E+":
                    value = L * Math.Pow(10, R);
                    break;
                case "E-":
                    value = L * 1 / (Math.Pow(10, R));
                    break;
                case "-":
                    value = L - R;
                    break;
                case "+":
                    value = L + R;
                    break;
                case "x":
                    value = L * R;
                    break;
                case "/":
                    if (!(R == 0)) value = L / R;
                    else Form1.SetError(7);
                    break;
            }

            return value;
        }
        void test(List<Input> list, string comment)      //Display contents of stack
        {
            string temp = "";//Start test
            foreach (Input guest in list)
            {
                temp += guest.getItem();
                temp += "  ";
            }
            System.Windows.Forms.MessageBox.Show("List: " + temp + " items: " + list.Count + " " + comment);//End test
        }
        void types(List<Input> list, string comment)
        {
            string temp = "";//Start test
            foreach (Input guest in list)
            {
                temp += guest.getType();
                temp += "  ";
            }
            System.Windows.Forms.MessageBox.Show("Types in List: " + temp  + comment);//End test
        }
        void display(string message)
        {
            System.Windows.Forms.MessageBox.Show(message);
        }
        public List<Input> StripArith(List<Input> aList)
        {
            List<Input> StrippedArith = new List<Input>();
            bool signed = false;
            bool foundBin = false;

            double L = 0;
            string BinOp = "";
            //test(bList, "Arith At start");
            //types(bList, "On start");
            foreach (Input guest in aList)//12 + 2 - 3 x 11 / 16, -6, 740, 2.5E+6 x 4
            {
                double R = 0;
                if (guest.getType() == 2)
                {
                    foundBin = true;
                    BinOp = guest.getItem();

                }
                else if (guest.getType() == 12) signed = true;
                else if (guest.getType() == 1 || guest.getType() == 8)
                {
                    if (signed) guest.SignResult();
                    if (foundBin)
                    {
                        foundBin = false;
                        if (guest.NotNull()) R = guest.getOutput();
                        else R = numb.convert(guest.getItem());
                        //System.Windows.Forms.MessageBox.Show("Found Binary => L : " + L + " R : " + R);
                        switch (BinOp)
                        {
                            case "-":
                                result -= R;
                                break;
                            case "+":
                                result += R;
                                break;
                        }

                    }
                    else
                    {
                        if (guest.NotNull()) L = guest.getOutput();
                        else L = numb.convert(guest.getItem());
                        result = L;
                        //System.Windows.Forms.MessageBox.Show("On loop, L = " + L);
                        continue;
                    }
                    //L = guest.getOutput();
                }
            }

            Results output = new Results(result);
            Input newInput = new Input(output);
            StrippedArith.Add(newInput);
            //test(StrippedBin, "Arith At the end");
            return StrippedArith;

        }
    }
}
