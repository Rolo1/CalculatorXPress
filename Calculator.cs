using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorXpress
{
    public class Calculator
    {
        private Validate check;
        private Numbers Number;
        private Step step;

        public Calculator()
        {
            check = new Validate();
            Number = new Numbers();
            step = new Step();
        }
        public List<Input> stepThrough()
        {
            List<Input> temp = new List<Input>();
            temp = step.stepLists();
            return temp;
        }

        /*type 0 = nothing, 1 = numeric, 2 = binaryOp, 3 = leftOp, 4 = rightOp, 5 = dot, 6 = leftBrace, 7 = rightBrace,
         * 8 = result, 9 = Pi, 10 = invert, 11 = toggle, 12 = validMinus, 13 = reciprocal, 14 = error, 15 = Binary Functions */
        public double Start(Stack<Input> cStack, Stack<int> cType)
        {
            double result = 0;
            List<Input> cList = cStack.ToList();
            List<int> TypeLst = cType.ToList();
            List<Input> RippedOff = new List<Input>();
            cList.Reverse();
            string types = TypesToString(cList);
            //test(cList, " Received"); //Display the received stack
            if (types.Contains('8'))//&& !(cList.Last().getType()  == 8)contains results from prior operation
            {
                //test(cList, "On Entry Results");
                //ShowTypes("Types at start results: ", cList);
                RippedOff = RipResult(cList);//results are now type 1, numbers are grouped, constant chars removed
                //ShowTypes("Types at the end results: ", RippedOff);
                //test(RippedOff, "On Exit Results");
            }//end if
            else
            {
                //test(cList, "On Entry Numbers");
                //ShowTypes("Types at start numbers: ", cList);
                RippedOff = RipNumbers(cList);//numbers grouped, const chars removed
                //ShowTypes("Types at the end numbers: ", RippedOff);
                //test(RippedOff, "On Exit Numbers");
            }
            cList = RippedOff;
            TypeLst = RefreshTypes(cList);
            result = Start1(cList, TypeLst); //This is the main calling function. Executes once.
            
            return result;
        }//end start
        public double Start1(List<Input> cList, List<int> cType)
        {
            double result = 0;
            List<Input> RippedOff = new List<Input>();

            if (cType.Contains(7))
            {
                //test(cLst, "On Entry SubTerms");
                RippedOff = RipTerms(cList, cType);
                cList = RippedOff; //subterms are now numbers, no braces bellow this
                //step.addRow(cList);
                //test(cLst, "On Exit SubTerms");
            }
            result = Calc(cList, cType); //This is the second function. Executes once.
            return result;
        }   //End Start1

        public double Calc(List<Input> cList, List<int> cType) 
        {
            double result = 0;
            List<Input> RippedOff = new List<Input>();
            //test(cList, " Starting Calc");
            if (cType.Contains(12))// -√3 - Sin(60) -(Log(1000) - 50) -1/30
            {
                //test(cList, "On Entry sign");
                //ShowTypes("Types at start Signed: ", cList);
                RippedOff = RipSign(cList);
                cList = RippedOff;
                cType = RefreshTypes(cList);
                step.addRow(cList);
                //ShowTypes("Types On Exit Signed: ", RippedOff);
                //test(cList, "On Exit sign");
            }
            if (cType.Contains(3)) //left operation
            {
                //test(cList, "On Entry Left Operator");
                LeftHand leftOp = new LeftHand();
                RippedOff = leftOp .StripLeft(cList);
                cList = RippedOff; //terms with left hand ops are now numbers, no left hand operators below
                step.addRow(cList);
                //test(cList, "On Exit Left Operator");
            }
            if (cType.Contains(4)) //right operation
            {
                //test(cList, "Before Call Right");
                RightHand rightOp = new RightHand();
                RippedOff = rightOp.StripRight(cList);
                cType = RefreshTypes(RippedOff);
                //if(cType.Contains(12)) cList = RipSign(RippedOff); //The signs that go before the operator
                cList = RippedOff; //No signed operators
                step.addRow(cList);
                //test(cList, "After Call Right");
            }
            if (cType.Contains(15)) //subtypes: 3. binary functions(Mod, ^, E+, E-), may output a String
            {
                //test(cList, "Before Binary Function");
                //ShowTypes("Types at start Bi-Function: ", cList);
                BinaryOp binOp = new BinaryOp();
                RippedOff = binOp.StripBinFunc(cList);
                cList = RippedOff; //terms with binary ops are now numbers, no more operators of any kind
                step.addRow(cList);
                //ShowTypes("Types On Exit Bi-Function: ", cList);
                //test(cList, "After Binary Function");
            }
            if (cType.Contains(2) ) // subtypes: 1. arithmetic (x, /), may Output a String
            {
                //ShowTypes("Types at start Binary: ", cList);
                //test(cList, "Before Arithmetic(x, /)");
                BinaryOp binOp = new BinaryOp();
                RippedOff = binOp.StripBin(cList);
                //cList = RippedOff; //terms with binary ops are now numbers, no more operators of any kind
                //RippedOff = binOp.StripArith(cList);
                cList = RippedOff;
                step.addRow(cList);
                //ShowTypes("Types On Exit Binary: ", cList);
                //test(cList, "After Arithmetic(x, /)");
            }
            if (cType.Contains(2)) // subtypes: 2. arithmetic (+, -), Outputs a number
            {
                //ShowTypes("Types at start Binary: ", cList);
                //test(cList, "Before Arithmetic(+, -)");
                BinaryOp binOp = new BinaryOp();
                RippedOff = binOp.StripBin2(cList);
                //cList = RippedOff; //terms with binary ops are now numbers, no more operators of any kind
                //RippedOff = binOp.StripArith(cList);
                cList = RippedOff;
                step.addRow(cList);
                //ShowTypes("Types On Exit Binary: ", cList);
                //test(cList, "After Arithmetic(+, -)");
            }
            //test(cList);
            //foreach(Input I in cList) { buff += I.getItem() + " "; }

            if (cList.Count > 0) result = cList.Last().getOutput(); //cList.ElementAt(0).getOutput();     //cLIst.Last().getOutput();
            //display("Result: " + buff + " List items: " + cList.Count);
            return result;
        }//End Calc()
        public List<Input> RipResult(List<Input> cLst) //Change type 8 to 1; removes the equal sign
        {
            List<Input> buffer = new List<Input>();
            int start = 0;
            string result = "";
            //test(cLst, "Entry List");
            //ShowTypes("At start: ", cLst);

            for (int i = cLst .Count - 1; i >= 0; i--)
            {
                if (cLst.ElementAt(i).getType() == 8)
                {
                    result = cLst.ElementAt(i).getItem ();
                    start = i;
                    break;
                }
            }
            string rslt = "";
            for (int j = 3; j < result.Length; j++) rslt += result[j]; //
            double number = Number.convert(rslt);
            //display("rslt : " + rslt);
            Results aResult = new Results(number);
            Input H = new Input(aResult);
            H.setType(1);
            //display("Item : " + H.getItem());
            start++;
            List<Input> temp = new List<Input>();
            temp.Add(H);
            for (int j = start; j < cLst.Count; j++)
            {
                temp.Add(cLst.ElementAt(j));
            }
            //test(temp, " ! Temp !");
            temp = RipNumbers(temp);    //Rips remaining numbers on the list
            foreach (Input I in temp) buffer.Add(I);
            //test(buffer, "Exit List");
            //ShowTypes("At the end: ", cLst);
            return (buffer);
        }

        public List<Input> RipNumbers(List<Input> nLst) // not all need to start here
        {
            List<Input> tmpLst = new List<Input>();
            //test(nLst);
            tmpLst = AddDouble(nLst);
            //test(tmpLst);

            return tmpLst;
        }//End RipNumbers
        public List<Input> RipTerms(List<Input> tLst, List<int> cType)  //UUT -TEST ACTIVE
        {
            List<Input> inputList = new List<Input>();      //easier to work with index than type stack
            List<Input> RippedTerm = new List<Input>();
            SubTerm sub = new SubTerm();

            inputList = tLst;  //tLst.ToList();
            int T = CountT(inputList);
            int L = T;

            //test(inputList );      //Display contents of stack or list

            while (T > 0) //the expression (inputList) contains "(" repeat until TS = 0
            {
                List<Input> tempList = new List<Input>();
                for (int i = 0; i < inputList.Count; i++) //Scan through the expression
                {
                    if (inputList.ElementAt(i).getType() == 6)
                    {
                        L--;

                        if (L == 0)
                        {
                            T--;
                            List<Input> tmpLst = new List<Input>();
                            tmpLst = sub.GetInner(inputList, i);
                            //test(tmpLst);
                            Calculator calc = new Calculator();
                            double value = calc.Calc(tmpLst, cType);
                            string buffer = value.ToString();
                            //display("Buffer " + value);
                            Results results = new Results(value);
                            Input input = new Input(results);
                            input.setType(1);
                            tempList.Add(input);
                            step.addRow(tempList);
                            i = sub.getIndex();
                        }
                        else
                        {
                            tempList.Add(inputList.ElementAt(i));
                        }
                    }
                    else
                    {
                        tempList.Add(inputList.ElementAt(i));
                        //test(inputList);
                    }
                }//End for

                inputList = tempList;
                //test(inputList);
                L = T = CountT(inputList);

            }  //End do-while

            return inputList;    
        }                                        //END RipTerms()
        public List<Input> RipSign(List<Input> sLst) // handle signed numbers
        {
            //test(sLst, "Ripp On entry");
            string guess = "";
            List<Input> RippedSign = new List<Input>();
            bool signed = false;
            foreach (Input guest in sLst) 
            {
                if (guest.getType() == 12)
                {
                    signed = true;
                    continue;
                }
                else if (guest.getType() == 1)
                {
                    guess = (guest.getItem());
                    Input temp = loadDigits(guess);
                    RippedSign.Add(temp);
                    //System.Windows.Forms.MessageBox.Show("Guess double: " + temp.getOutput() + " Guest string: " + temp.getItem());
                    //RippedSign.Add(guest);
                    if (signed)
                    {
                        /*if (temp.getOutput () > 0)*/ temp.SignResult();
                        //System.Windows.Forms.MessageBox.Show("Signed: " + temp.getOutput()); 
                        signed = false;
                    }
                    continue;
                }
                else
                {
                    if(signed)
                    {
                        signed = false;
                        guess = "-";
                        Input temp = makeInput(guess, 12);

                        RippedSign.Add(temp);
                    }
                }
                RippedSign.Add(guest);
            }
            //test(RippedSign, "Ripp On exit");
            return RippedSign;
        }

            public List<Input> AddDouble(List<Input> lst)  //store the numbers as double, save numeric string as 1 item
        {
            List<Input> temp = new List<Input>();  
            bool digits = false;
            string buffer = "";
            Functions function = new Functions();
            Results result = new Results();
            string sign = "";
            //test(lst, "Add double On entry");
            //ShowTypes("At start: ", lst);

            foreach (Input guest in lst)
            {
                //System.Windows.Forms.MessageBox.Show("Guest: " + guest.getItem());
                if (!(isNumber(guest)))
                {
                    sign = guest.getItem(); //capture a neg sign, renew on each run
                    //if (sign == "-") continue;
                    if (digits) //Means the item before this is a number. Automatic multplication by coefficient: 2π² + 4e, etc.
                    {
                        digits = false;
                        temp.Add(loadDigits(buffer));
                        //System.Windows.Forms.MessageBox.Show("Buffer 2: " + buffer);
                        buffer = "";  //clear to start a new
                        if (guest.getType () == 9)
                        {
                            if (guest.getItem() == "π")
                            {
                                temp.Add(makeInput("x", 2));
                                result = new Results(Math.PI);
                            }
                            else if (guest.getItem() == "e")
                            {
                                temp.Add(makeInput("x", 2));
                                result = new Results(Math.E);
                            }
                            Input input = new Input(result);
                            input.setType(1);
                            temp.Add(input);
                            continue;
                        }

                    }
                    else if (guest.getType() == 9)//Convert π or e to number: 7 + π 
                    {
                        if (guest.getItem() == "π")
                        {
                            result = new Results(Math.PI);
                        }
                        else if (guest.getItem() == "e")
                        {
                            result = new Results(Math.E);
                        }
                            //System.Windows.Forms.MessageBox.Show("Guest: " + guest.getItem());
                            Input input = new Input(result);
                            input.setType(1);
                            temp.Add(input);
                            continue;
                        
                    }

                        temp.Add(guest);
                        //System.Windows.Forms.MessageBox.Show("Guest: " + guest.getItem());
                        //test(temp);
                      
                }
                else //the item is a numeric string or constant char
                {
                    buffer += guest.getItem();                 
                    digits = true;
                }

            }
            if(digits) temp.Add(loadDigits(buffer));
            //test(temp, "Add double on Exit" );      //Display contents of stack
            //ShowTypes("At the end: ", lst);
            return temp;
        }   //End AddDouble()

        public Input loadDigits(string buff)
        {
            double value = 0;
            //System.Windows.Forms.MessageBox.Show("String to Input.result: " + buff);
            value = Number .convert (buff);
            //System.Windows.Forms.MessageBox.Show("Number: " + value);
            Results aNumber = new Results(value);
            Input inputNumb = new Input(aNumber);
            inputNumb.setType(1);   //now this numeric string is stored as double
            //System.Windows.Forms.MessageBox.Show("New Input: " + inputNumb.getItem());
            return inputNumb;
        }
        public Input makeInput(string buff, int type)
        {
            //System.Windows.Forms.MessageBox.Show("Buffer to Input.String: " + buff);
            Functions function = new Functions(buff);
            Input inp = new Input(function);
            inp.setType(type);
            //System.Windows.Forms.MessageBox.Show("Input.String: " + inp.getItem());
            return inp;
        }
        public string Reverse(string buff)
        {
            string temp = "";
            for(int i = buff.Length - 1; i > -1 ; i--)
            {
                temp += buff[i];
            }
            return temp;
        }
        public Stack<Input> push(Stack<Input> stack, double value)
        {
            Stack<Input> temp = stack;
            string buffer = value.ToString();
            Functions function = new Functions(buffer);
            Input input = new Input(function);
            temp.Push(input);
            return temp;
        }
        public Stack<Input> Reverse(Stack<Input>  stack)
        {
            Stack<Input> tmp = new Stack < Input >();
            foreach(Input I in stack)//for (int i = 0; i < stack.Count; i++)
            {
                tmp.Push(I); //if(stack.Count > 0) tmp.Push(stack.Pop());
            }

            return tmp;
        }
        public String Peek(Stack<Input> astack)
        {
            String input = "";
            if (!(astack.Count <= 0)) input = astack.Peek().getItem();
            return input;
        }
        public string Pop(Stack<Input> stack)
        {
            string item = "";

            if (stack.Count > 0)
            {
                item = stack.Peek().getItem();
                stack.Pop();
            }
            return item;
        }
        public int getType(Stack<Input> aStack)
        {
            int t = 0;
            if (!(aStack.Count <= 0)) t = aStack.Peek().getType();
            return t;
        }
        public bool isNumber(Input item) //including decimal point
        {
            bool isNumber = (item.getType() == 1 || item.getType() == 5);
            return isNumber;
        }
        public int CountT(List<Input> inLst) //count how many of a type
        {
            int L = 0;
            foreach (Input i in inLst)
            {
                if (i.getType() == 6) L++; //7 = right, 6 = left parenthesis
            }
            return L;
        }//end CountT
        public int CountR(Stack<int> TLst) //count how many of a type 8
        {
            int R = 0;
            foreach (int i in TLst)
            {
                if (i == 8) R++; //count results
            }
            return R;
        }//end CountT
        Stack<int> CountTypes(Stack<Input> aStack)//updates remaining types in stack
        {
            Stack<int> types = new Stack<int>();

            foreach(Input guest in aStack)
            {
                types.Push(guest.getType());
            }
            return types;
        }
        void ShowTypes(string message, List<Input> aList)
        {
            string buff = "";
            foreach (Input guest in aList)
            {
                buff += guest.getType() + "  " ;
            }
            display(message + buff);
        }
        List<int> RefreshTypes(List<Input> aList) 
        {
            List<int> types = new List<int>();
            foreach (Input guest in aList)
            {
                types.Add(guest.getType ());
            }
            return types;
        }
        string TypesToString(List<Input> aList)
        {
            string types = "";
            foreach (Input guest in aList)
            {
                types += guest.getType().ToString ();
            }
            return types;
        }
        public Stack<Input> ToStack(List<Input> aList)
        {
            Stack<Input> stack = new Stack<Input>();
            foreach(Input I in aList)
            {
                stack.Push(I);
            }
            return stack;
        }
        void test(Stack<Input> stack)//Display contents of stack from 0 to top (straight)
        {
            string temp = "";//Start test
            foreach (Input guest in stack)
            {
                temp += guest.getItem();
            }
            System.Windows.Forms.MessageBox.Show("Stack: " + temp);//End test
        }
        public void test(List<Input> list, string comment)      //Display contents of stack
        {
            string temp = "";//Start test
            foreach (Input guest in list)
            {
                temp += guest.getItem();
                temp += "  ";
            }
            System.Windows.Forms.MessageBox.Show("List: " + temp + " items: " + list.Count + " " + comment) ;//End test
        }
        void display(String input)
        {
            System.Windows.Forms.MessageBox.Show(input);//End test
        }
    }

}
