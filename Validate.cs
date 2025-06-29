using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
namespace CalculatorXpress { };

//Class Validate: validate inputs. Members: digits, constants(π, e, etc.), dots, left-operator, right-operator, binary-operator,
//binary-functions, braces, and terms. Ex. no numbers should input after a result, no number may contain more than 1 dot, 
//not valid a right brace when there is no left, shouldn't be allowed a right-operator following another of the same type (repeated)
//and no expression can be evaluated with a missing matching brace- Must validate 1. the input char and 2. the buffer string
//Used as Validate(argument), Validate.ValidNumber, etc. Class methods should detect an input char on the call, and return a boolean
//Structure is switch(inputChar) -> case(bool???) -> validateType -> bool_out, Or if(IsOfType(inputChar)) { } elseif, elseif, elseif
namespace CalculatorXpress
{
    public partial class Validate
    {
        private const String equalSign = " = ";
        private const string equals = " ═ ";
        private const string ScreenDefault = "0·";
        private const string ArithOp = "+x/*-";
        private const string BinaryOp = "+-x*/"; // _^Mod"; //validates external. The minus sign means substraction
        private const string BinaryFunc = "E^Mod";
        private const string numeric = "0123456789"; //validates external
        private const string Alpha = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string error = "<-Error:"; //Length 8
        private const string RightOp = "√Log₂SinCosTa∩hⁿ˟Ɩŋℓ"; //validates external
        private const string LeftOp = "²%↓!"; //validates external
        private const string Trig = "SinCosTa∩h";
        private const string Logs = "LgƖŋℓ₂";
        private const string positiveOnly = "LgƖŋℓ₂√";  
        private const string inverse = "ˉ¹";
        private const string plusE = "E+";
        public const string negE = "E-";
        private const string constant = "πe";
        private const char dot = '.'; //validates external
        private const char pi = 'π';
        private const char e = 'e'; 
        private const char minus = '-';//This minus means sign (▬) = Alt 22 
        private string chars = "(√LogSinCosTa∩ˉ¹eⁿƖŋ²%↓₂ℓMod^+-/*x";
        private int[] T;
        public Stack<int[]> opType;
        //public Queue<int> typeOp;
        /*type 0 = emptyBuffer, 1 = numeric, 2 = binaryOp, 3 = leftOp, 4 = rightOp, 5 = dot, 6 = leftBrace, 7 = rightBrace,
         * 8 = result, 9 = Pi, 10 = invert, 11 = toggle, 12 = validMinus, 13 = reciprocal, 14 = error , 15 = Binary Functions*/


        public Validate() { // default constructor. put initial value so is never empty
            opType = new Stack<int[]>();
            T = new int[4];
            for (int i = 0; i < 4; i++) T[i] = 0;
        }

        ~Validate() { } //Destructor

        public void setType(int t) { this.T[0] = t; }
        public void setMode(int m) { this.T[1] = m; }
        public void setPrior(int p) { this.T[2] = p; }
        public Stack<int> getType()
        {
            Stack<int> types = new Stack<int>();
            foreach(int[] I in opType) { types.Push(I[0]); }
            return types;
        }
        public int current() { return Form1.getItemType(); }
        public int getMode() {
            if (getCount() > 0) { return this.opType.Peek()[1]; }
            else return 0; }
        public int getPrior() {
            if (getCount() > 0) { return this.opType.Peek()[2]; }
            else return 0; }
        public int peekType() {
            if (getCount() > 0){ return (this.opType.Peek()[0]); }  //getter get last input. User: ??
            else return 0;}
        public int getCount() { return (this.opType.Count); } //get the stack count
        public void pushType(int[] aType) => this.opType.Push(aType); //insert an external entry (array[4])
        public void popType() { if (getCount() > 0) this.opType.Pop(); } //setter remove last entry. User: Form1.Backspace_click
        public void clearType() { this.opType.Clear(); } //setter erase all. User: Form1.Clear_click
        public Stack<int[]> getOpType() { return (this.opType); }

        bool BuffEmpty(string buff)
        {
            if (buff.Length < 1)
            { return true; }
            else
            { return false; }
        }
        bool IsNull(char input) { return (input == '\0'); }
        public bool IsNumeric(char input)
        {
            bool isNumber = numeric.Contains(input);
            return (isNumber);
        }
        public bool IsNumeric(string input) //overload for numeric strings
        {
            bool isNumber = numeric.Contains(input);
            return (isNumber);
        }
        public bool IsArith(char input)
        {
            bool Arithmetic = ArithOp.Contains(input);
            return (Arithmetic);
        }
        public bool IsBinaryOp(char input)
        {
            bool bin = BinaryOp.Contains(input);
            return (bin);
        }
        public bool IsBinaryFunction(string input)
        {
            bool bFunct = BinaryFunc.Contains(input);
            return bFunct;
        }
        public bool IsAlpha(char input)
        {
            bool isAlphabetic = Alpha.Contains(input);
            return (isAlphabetic);
        }
        public bool IsError(string input)
        {
            bool eMssg = false ; //"<-Error: "
            string test = "";
            if (input.Length > error.Length)
            {
                for (int j = 0; j < error.Length; j++)
                {
                    test += input[j];
                }
            }
            if (test == error) eMssg = true;
           // MessageBox.Show("Test : " + test + " Error: " + error + "   Tested: " + eMssg);
            return (eMssg);
        }
        public bool IsRightOp(char input)
        {
            bool right = RightOp.Contains(input);
            return (right);
        }
        public bool IsLeftOp(char input)
        {
            bool left = LeftOp.Contains(input);
            return (left);
        }
        public bool onlyPositive(string input)
        {
            bool P = positiveOnly.Contains(input);
            return P;
        }
        public bool IsTrig(char input)
        {
            bool Trigon = Trig.Contains(input);
            return (Trigon);
        }
        public bool IsLog(char input)
        {
            bool isLogs = Logs.Contains(input);
            return (isLogs);
        }
        public bool IsInverse(char input)
        {
            bool Inv = inverse.Contains(input);
            return (Inv);
        }
        public bool IsRightBrace(char input)
        {
            bool Rbrace = (input == ')');
            return (Rbrace);
        }
        public bool IsMinus(char input)
        {
            bool neg = (input == minus);
            return (neg);
        }
        public bool IsLeftBrace(char input)
        {
            bool Lbrace = (input == '(');
            return (Lbrace);
        }
        public bool IsDot(char input)
        {
            bool aDot = (input == dot);
            return (aDot);
        }
        public bool IsPi(char input)
        {
            bool aPi = (input == pi);
            return (aPi);
        }
        public bool IsConstant(char input)
        {
            bool aConst = constant .Contains (input);
            return (aConst);
        }
        public bool IsResult(char input)
        {
            bool result = (equalSign.Contains(input));
            return result;
        }
        public bool IsToggle(char input)
        {
            bool toggle = (peekType() == 11);
            return toggle;
        }
        public bool isReciprocal(char input)
        {
            bool recip = (peekType() == 13);
            return recip;
        }
        private bool CountBraces(string buff)   //COMPARE PARENTHESYS L >  R FOR INPUT (R)
        {
            int L = 0;
            int R = 0;
            int size = buff.Length - 1;
            if (size < 0) { return false; }
            for (int i = size; i >= 0; i--)
            {
                if (buff[i] == '(')
                { L++; }
                else if (buff[i] == ')')
                { R++; }
            }
            return L > R;
        }
        private bool MatchBraces(string buff)
        {
            int L = 0;
            int R = 0;
            int size = buff.Length - 1;
            if (size == 0) { return true; }
            for (int i = size; i >= 0; i--)
            {
                if (buff[i] == '(')
                { L++; }
                else if (buff[i] == ')')
                { R++;
                    if (i > 0)
                    {if (buff[i - 1] == '(')
                        {
                            Form1.SetError(16); }
                    }
                }
            }
            return (L == R);
        }
        public bool MatchDots(string buff) //gets 1 term at a time
        {
            if (!(buff.Contains(dot)))
            {return true;}
            int dots = 0;
            int size = buff.Length - 1;
            for (int i = size; i >= 0; i--)
            {
                if (buff[i] == '.')
                { dots++; }
                if((i == 0) && buff[i] == '.') { return false; }
            }

            return (!(dots > 1));
        }
        public string LastNumber(string buff)    //Returns the first character plus the number
        {                                       //this first char is function ID
            string Last = "";
            int size = buff.Length - 1;
            if (size > -1)
            {
                for (int i = size; i >= 0; i--)
                {
                    if(!(IsNumeric(buff[i])) && !(IsDot(buff[i])))
                    {
                        Last += buff[i];
                        return(Last);
                    }
                    else
                    {
                        Last += buff[i];
                    }
                }
            }
                return (Last);
        }
        public string LastTerm(string buff)
        {
            string lastTerm = "";
            int size = buff.Length - 1;
            bool last = false;

            if (size > -1)
            {
                int count = size;
                while (count > -1 && !last)
                {
                    if (buff[count] == ')')
                    {
                        lastTerm += nestedTerm(buff, count);
                        return lastTerm;
                    }
                    else if (IsBinaryOp(buff[count]) || buff[count] == '\0' || buff[count] == ' ')
                    {
                        last = true;
                        break;
                    }
                    else if (chars.Contains(buff[count]))
                    {
                        return lastTerm;
                    }
                    else
                    {
                        lastTerm += buff[count];
                    }
                    count -= 1;
                }
            }

            return lastTerm;
        }
        private string nestedTerm(string buff, int index)
        {
            string subTerm = "";
            int i = index;
            while (i >= 0)
            {
                if (buff[i] == '(')
                {
                    subTerm += buff[i];
                    return (subTerm);
                }
                else
                { subTerm += buff[i]; }
                i -= 1;
            }
            return subTerm;
        }
        private char getLastChar(string buff)
        {
            char current = '\0';
            if (buff.Length > 0) current = buff[buff.Length - 1];
            return current;
        } 
        private char getFirstChar(string buff) //buff > 0
        {
            char first = '\0';
            int size = buff.Length - 1;
            //string last = LastNumber(buff);
            if (size > -1)
            first = buff[0];// last[0];
            return (first);
        }
        public bool ValidDot(string buff)  //"(.)" 
        {
            int type = current();
            char last = '\0';
            if(BuffEmpty(buff))
            {
                setType(5);
                setPrior(0);
                pushType(T);
                return true;
            }
            string term = LastNumber(buff);

            bool valid = false;
            last = term[term.Length - 1];
            //display("Type: " + type);

            if (type == 3 || type == 7 || type == 8 || type == 9 || type == 13 || term.Contains(dot)) // the term has dot already
            {
                valid = false;
            }
            else
            {
                valid = true;
                setType(5);
                setPrior(type);
                pushType(T);
            }

            return valid;
        }
        /*type 0 = nothing, 1 = numeric, 2 = binaryOp, 3 = leftOp, 4 = rightOp, 5 = dot, 6 = leftBrace, 7 = rightBrace,
         * 8 = result, 9 = Pi, 10 = invert, 11 = toggle, 12 = validMinus, 13 = reciprocal, 14 = error, 15 = Binary Functions*/
        public bool ValidNumber(string buff)   // "(0123456789)"
        {
            int type = current();
            //display("Buffer : " + buff + " Type : " + type);
            if (BuffEmpty(buff))
            {
                setType(1);
                setPrior(0);//mode is 0
                pushType(T);
                return true;
            }
            //char last = getLastChar(buff);
            
            bool valid = false;
            if (type == 3 || type == 7 || type == 8 || type == 9 || type == 13)//The right operator pushed a 'left operator'= Form1.MutedStatus())
            {
                valid = false;
            }
            else
            {
                valid = true;
                setType(1);
                setPrior(type);
                pushType(T);
            }
            return valid;
        }
        public bool ValidLeftOp(string buff)   //"²%↓!"
        {
            if (BuffEmpty(buff))
            { return false; }
            int type = current();
            int prior = Form1.getPrior();
            if (prior == 12 && onlyPositive(buff)) return false;
            bool valid = false;
            //char last = getLastChar(buff);
            if (type == 1 || type == 7 || type == 8 || type == 9 || type == 11 || type == 13)
            {
                valid = true;
                setType(3);
                //setMode(getPrior());//////
                setPrior(type);
                pushType(T);
            }
            else 
            {
                valid = false;
            }
            return valid;
        }
        public bool ValidRightOp(string buff)  //(√Log₂SinCosTa∩ˉ¹e°˟Ɩŋℓ)
        {
            int type = current();
            //display("Buffer : " + buff + " Type : " + type);
            if (type == 0)
            {
                setType(4);
                setPrior(type);
                pushType(T);
                return true;
            }
            bool valid = false;
            //char last = buff[buff.Length - 1];

            if (type == 0 || type == 2 || type == 6 || type == 8)
            {
                valid = true;
                if (type == 8) setType(3);
                else setType(4);
                if (onlyPositive(buff)) setMode(39); //log, √, only possitive
                setPrior(type);
                pushType(T);
            }
            else
            {
                valid = false;
            }
            return valid;
        }
        public bool ValidBinary(string buff)   //( +-x*/_^ⁿd ) -> Split binary functions from arithmetics (lower precedence)
        {
            int type = current();
            //MessageBox.Show("Type ? : " + type);
            if (type == 0) return false;   //BuffEmpty(buff)
            bool valid = false;
            
            if (type == 1 || type == 3 || type == 7 || type == 8 || type == 9 || type == 13)
            {
                valid = true;
                setType(2);
                setPrior(type);
                pushType(T);
            }
            else
            {
                valid = false;
            }
            return valid;
        }
        public bool ValidBinaryFunc(string buff)   //(Mod, ^, E+, E- ) -> Sam as above, only higher precedence
        {
            int type = current();
            //MessageBox.Show("Type ? : " + type);
            if (type == 0) return false;   //BuffEmpty(buff)
            bool valid = false;

            if (type == 1 || type == 3 || type == 7 || type == 8 || type == 9 || type == 13)
            {
                valid = true;
                setType(15);
                setPrior(type);
                pushType(T); 
            }
            else
            {
                valid = false;
            }
            return valid;
        }
        public bool ValidMinus(string buff)    // '-'
        {
            int type = current();
            if (type == 0)
            {
                setType(12);
                setPrior(type);
                pushType(T);
                return true;
            }
            bool valid = false;
            //char last = (buff[buff.Length - 1]);
            //bool badFcn = (last == '√' || Logs.Contains(last));
            bool badFcn = (getMode() == 39);
            if (badFcn || type == 5 || type == 12) //IsMinus(last))
            {
                valid = false;
            }
            else
            {
                valid = true;

                if (type == 0 || type == 2 || type == 4 || type == 6 || type == 15) setType(12); //decide binary or unary. 2 = binary, 12 = unary
                else setType(2);
                setMode(12);
                setPrior(type);
                pushType(T);
            }
            return valid;
        }
        public bool Sign(string buff)    // '-', 1 = Sign, 0 = Substract operator
        {
            int type = current();
            bool sign = false;

                if (type == 0 || type == 2 || type == 4 || type == 6 || type == 15) sign = true; //true = Sign (signed number)
                else sign = false;//False = Substraction operator
            //display("Input: " + buff + " Type: " + type + " Valid: " + sign);
            return sign;
        }
        public bool ValidToggle(string buff)   // '+ / -' True on the first char, false everywhere else
        {
            int type = current();
            if (BuffEmpty(buff)) return false; 
            bool valid = false;
            string term = LastTerm(buff); //this is a reversed str

            if (buff.Length - term.Length > 0)
            {
                char test = buff[(buff.Length - term.Length) - 1];
                if (test == 'E' || test == '√' || Logs.Contains(test))
                    return false;
                if ((test == '-' || test == '+') && (buff.Length - term.Length - 2 > 0))
                {
                    test = buff[buff.Length - term.Length - 2];
                    if (test == 'E')
                        return false;
                }
            }

            char first = getLastChar(term); //term[0] reversed
            char last = getLastChar(buff);// ; buff[size - 1] 
            bool badFcn = (first == '√' || Logs.Contains(first));//|| last == '√' || Logs.Contains (last));
            if (!(badFcn) && (IsNumeric(last) || (IsPi(last)) || IsBinaryOp(last) || last == '°' || last == ')'))
            {
                //display("First : " + first + " Last : " + last + " Term : " + term);
                valid = true;
            }
            else
            {
                valid = false;
            }

            return valid;
        }
        /*type 0 = nothing, 1 = numeric, 2 = binaryOp, 3 = leftOp, 4 = rightOp, 5 = dot, 6 = leftBrace, 7 = rightBrace,
         * 8 = result, 9 = Pi, 10 = invert, 11 = toggle, 12 = validMinus, 13 = reciprocal, 14 = error, 15 = Binary Functions */
        public bool ValidLeftBrace(string buff)    // '('
        {
            int type = current();
            if (BuffEmpty(buff))
            {
                setType(6);
                setPrior(type);
                pushType(T);
                return true;
            }
            bool valid = false;
            //char last = getLastChar(buff);

            if (type == 1 || type == 5 || type == 7 || type == 8)
            {
                valid = false;
            }
            else
            {
                valid = true;
                setType(6);
                setPrior(type);
                pushType(T);
            }
            return valid;
        }
        public bool ValidRightBrace(string buff)   // ')'
        {
            int type = current();
            if (BuffEmpty(buff))
            { return false; }
            bool valid = false;
            bool even = CountBraces(buff);
            if (even && !(type == 5 || type == 2 || type == 15 || type == 4 || type == 6) )
            {
                valid = true;
                setType(7);
                setPrior(type);
                pushType(T);
            }
            else
            {
                valid = false;
            }
            return valid;
        }
        public bool ValidInvert(string buff)   // Sinˉ¹, Logˉ¹, etc.
        {
            int type = current();
            if (BuffEmpty(buff) || buff == ScreenDefault)
            { return false; }
            bool valid = false;
            char last = buff[buff.Length - 1]; // getLastChar(buff);

            if (Trig.Contains(last) || IsLog(last))
            {
                valid = true;
                int prior = getPrior();
                if (prior == 8) setType(3);
                else setType(4);
                //setType(10);
                setPrior(type);
                pushType(T);
            }
            else
            {
                valid = false;
            }
            return valid;
        }
        public bool ValidConstant(string buff) // 'π', or 'e'. Implicit multiplication: number|constant
        {
            int type = current();
            if (type == 0)
            {
                setType(9);
                setPrior(type);
                pushType(T);
                return true;
            }
            bool valid = false;

            if (type == 1 || type == 2 || type == 4 || type == 5 || type == 6 
                || type == 12 || type == 15 || type == 88 ) //88 new alt equalSign (205) to step
            {
                valid = true;
                setType(9);
                setPrior(type);
                pushType(T);
            }
            else
            {
                valid = false;
            }
            return valid;
        }
        public bool validEquals(string buff)
        {
            int type = current();
            bool valid = true;
            setType(88);
            setPrior(type);
            pushType(T);
            return valid;
        }
        public bool validate(string input, string buff) //ONLY USED BU PUSH()
        {
            bool valid = false;
            //display("Input(String) : " + input + "  Buffer : " + buff + "  Type : " + peekType ());
            switch (input)
            {
                case "Sin":
                    return ValidRightOp(buff);
                case "Cos":
                    return ValidRightOp(buff);
                case "Ta∩":
                    return ValidRightOp(buff);
                case "Sinh":
                    return ValidRightOp(buff);
                case "Cosh":
                    return ValidRightOp(buff);
                case "Ta∩h":
                    return ValidRightOp(buff);
                case "Log":
                    return ValidRightOp(buff);
                case "ℓog₂":
                    return ValidRightOp(buff);
                case "Ɩŋ":
                    return ValidRightOp(buff);
                case "Mod":
                    return ValidBinaryFunc(buff);
                case "ˉ¹":
                    return ValidInvert(buff);
                case "E+":
                    return ValidBinaryFunc(buff);
                case "E-":
                    return ValidBinaryFunc(buff);
                case "^":
                    return ValidBinaryFunc(buff);
                case "-": 
                    //display("String: " + Sign(input));
                    if (Sign(input)) return ValidMinus(buff);//sets type to 12
                    else return ValidBinary(buff);//sets the type to 2
                case ".": return ValidDot(buff);
                case " ═ ": return validEquals(buff); //" ═ ":Equals != equalSign(" = ")!(" ═ ")
                default:
                    break;
            }
            int type = current();
            if (input.Contains(equalSign))
            {
                valid = ValidStart(buff);
                if (valid)
                {
                    setType(8);
                    setPrior(type);
                    pushType(T);
                }
            }
            if (IsError(input))
            {
                valid = true;
                setType(14);
                setPrior(type);
                pushType(T);
            }
            return valid;
        }
        /*type 0 = nothing, 1 = numeric, 2 = binaryOp, 3 = leftOp, 4 = rightOp, 5 = dot, 6 = leftBrace, 7 = rightBrace,
         * 8 = result, 9 = Pi, 10 = invert, 11 = toggle, 12 = validMinus, 13 = reciprocal, 14 = error , 15 = Binary Functions*/
        public bool validate(char input, string buff) //sorting the input for auto-validation
        {
            //display("Input(Char) : " + input + "  Buffer : " + buff  + "  Type : " + peekType ());

            bool valid = false;
            bool A = IsNumeric(input);
            bool B = IsRightOp(input);
            bool C = IsLeftOp(input);
            bool D = IsDot(input);
            bool E = IsConstant(input);
            bool F = IsRightBrace(input);
            bool G = IsLeftBrace(input);
            //bool H = IsInverse(input);
            bool I = IsMinus(input);
            bool J = IsBinaryOp(input);//IsBinaryOp(input);
            bool K = (input == ' ');
            bool L = IsBinaryFunction(input.ToString ());

            if (A) { return (ValidNumber(buff)); }

            else if (B) { return (ValidRightOp(buff)); }//This also includes the inverse simbol: "ˉ¹"

            else if (C) { return (ValidLeftOp(buff)); }

            else if (D) { return (ValidDot(buff)); }

            else if (E) { return (ValidConstant(buff)); }

            else if (F) { return (ValidRightBrace(buff)); }

            else if (G) { return (ValidLeftBrace(buff)); }

            //else if (H) { return (ValidInvert(buff)); }

            else if (I)
            {
                string inp = "";
                inp += input;
                if (Sign(inp)) return ValidMinus(inp);
                else return ValidBinary(inp);
            }
            else if (J) { return (ValidBinary(buff)); }

            else if (K) { return true; } // ValidStart(buff);  }

            else if (L) { return ValidBinaryFunc(buff); }

            else
            {
                //MessageBox.Show("Is else. Buffer: " + buff + "  Input: " + input);
                return valid; //valid = false
            }
            
        }
        public int getType(string input)
        {
            if (input.Contains(equalSign)) return 8;
            else if (input.Contains("(1/)")) return 13;

            switch (input)
            {
                case "Sin": return 4;
                case "Cos": return 4;
                case "Ta∩": return 4;
                case "Sinh": return 4;
                case "Cosh": return 4;
                case "Ta∩h": return 4;
                case "Log": return 4;
                case "ℓog₂": return 4;
                case "Ɩŋ": return 4;
                case "Mod": return 15;
                case "^": return 15;
                case "E+": return 15;
                case "E-": return 15;
                case " ═ ": return 88; //<alt>205 != equalSign
                case "ˉ¹": return current();
                case "-": { if (Sign(input)) return 12; else return 2;}
                default: return getType(input[input.Length - 1]);
            }//When no match is found, default sends the last char to the overload (*below)
        }// END String GetType()
        public int getType(char input)
        {
            int peek = current();
            //display("Peek type: " + peek);
            if (IsNull(input)) return 0;
            else if (IsNumeric(input)) return 1;
            else if (IsLeftOp(input)) return 3;
            else if (IsRightOp(input)) return 4;
            else if (IsDot(input)) return 5;
            else if (IsLeftBrace(input)) return 6;
            else if (IsRightBrace(input)) return 7;
            else if (IsResult(input)) return 8;
            else if (IsConstant(input)) return 9;
            else if (IsInverse(input)) return 10;
            else if (IsToggle(input)) return 11;
            else if (IsBinaryOp(input)) return 2;
            else if (isReciprocal(input)) return 13;
            else if (IsMinus(input))
            {
                string inp = "";
                inp += input;
                //display("Char: " + Sign(inp));
                if (Sign(inp)) return 12;
                else return 2;
            }
            else if (IsBinaryFunction(input.ToString ())) return 15;
            else return 20; //code for Error
        }//END Char GetType()
        public bool ValidStart(string buff)    //Re-Check the string for(keyboard) alterations. Get any error codes
        {
            int type = current();
            bool valid = false;
            if (BuffEmpty(buff) || Form1.ExistsErrors() || buff == "0·")
            {
                //MessageBox.Show("Valid ? : " + valid);
                return false; }
            
            bool dotValid = false;
            bool biggerLeft = CountBraces(buff);
            bool equalBraces = MatchBraces(buff);
            if (!(equalBraces))
                {
                    valid = false;
                    //Form1.SetFlags(9, true);
                    if (biggerLeft)
                    { Form1.SetError(9); } //missing right
                    else
                    { Form1.SetError(10); } //missing left
                return false;
                }
            string BuffCopy = buff;
            string term;
            int size = BuffCopy.Length - 1;
            int i = size;
            while(i >= 0) //start to check for bad keyboard input (or copy pasted)
            {     
            term = LastNumber(BuffCopy);
            dotValid = MatchDots(term);

                if (!(dotValid))
                {
                    Form1.SetError(11);
                    valid = false;
                    return valid;              
                }
                //MessageBox.Show("Buff :" + BuffCopy + " Length : " + BuffCopy.Length + " - " + term.Length);
                BuffCopy = BuffCopy.Remove(BuffCopy.Length - term.Length);
                if (BuffCopy.Length < 1)
                    break;
                i = BuffCopy.Length;
            }
            if (type == 2 || type == 5 || type == 6 || type == 15) Form1.SetError(19);            
            if (dotValid && equalBraces && !Form1.ExistsErrors())
            {
                valid = true;
                //if(!(peekType () == 8)) pushType(8);
                //MessageBox.Show("Valid=> type : " + type + " Count : " + getCount ());
                //MessageBox.Show("Valid ? : " + valid);
                return valid;
            }
            
            return valid;
        }
        void display(String input)
        {
            System.Windows.Forms.MessageBox.Show(input);//End test
        }
    }
}//End Class
