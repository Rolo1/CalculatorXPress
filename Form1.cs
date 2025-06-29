using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CalculatorXpress
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            graphicsBox1.Image = new Bitmap(graphicsBox1.Width, graphicsBox1.Height); //for the graph panel
        }
        public string equalSign = " = ";
        public string inverse = "ˉ¹";
        public string chars = "(√LogSinCosTa∩ˉ¹e°Ɩŋ²%↓₂ℓMod^ⁿ+-/*x";
        public string buffer;   //{ "0" }; //'expression original -Calc()
        public string buffer2; //when 
        public double result;     // 'holds the final output of calculation -Calc()
        public string screenDefault = "0·";
        public string register;     //'memory. holds the stored expression -StoreMem()/result
        public List<Input> memList; // list to save expressions to memory
        public Stack<int> memType;
        public string memsum; // As String 'memory sum -Sum_memory()
        public static bool[] flag;  // 'Ten flags to test for different conditions. See reference at the end of class
        public string[] RecentStr;    // 'stores the recent calcs to be displayed in "Recent" listbox
        public int cycle = 0; // 'counts the calculations for most recent
        public bool invert;
        public bool SaveFile;
        public int currentType;
        public int count = 0; //all purpose counter
        public bool Recyp = false;
        //const string CalcSettings = "C:\\Temp\\StringCalc.dat";
        public string message;
        public static int index = 0;   // 'index for results
        public static int ErrorCode = 0;
        public static Validate CheckValid;
        public static Numbers aNumber;
        public static Symbols aSymbol;
        public static Functions aFunction;
        public static Input anInput;
        public static Stack<Input> stack;
        public static Stack<Input> oldValue;
        public Calculator calc;
        public Stack<Input> cloneStk;
        private void Form1_Load(object sender, EventArgs e)
        {
            aNumber = new Numbers();
            aSymbol = new Symbols();
            aFunction = new Functions();
            anInput = new Input();
            stack = new Stack<Input>();
            oldValue = new Stack<Input>();
            CheckValid = new Validate();
            resetFlag();
            OutMessageCode();
            display.ResetText(); //'1/19/15 el display no resetea con entradas
            display.Text = screenDefault;
            SendKeys.Send("{End}");
            display.Focus();
            DecFigures.SelectedIndex = 0;
            buffer = null;
            result = 0;
            invert = false;
            buffer = "";
            buffer2 = "";
            calc = new Calculator();
            cloneStk = new Stack<Input>();
            StreamReader recentCalcs;

            RecentStr = new string[1];   //counts the lines: File.ReadAllLines(file.FileName).Length
            string dirSettings = "";
            string fileName = "";
            /********* To switch between disk or pendrive go to line 921 and comment
            //either WriteToDisk() or WriteToPendrive() ***********/
            dirSettings = Directory.GetCurrentDirectory() + "\\CalcXpress\\";//PD ACTIVE
            fileName = dirSettings + "CalcSettings.dat";//PD ACTIVE
            //dirSettings = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\CalcXpress\\"; //DT
            //fileName = dirSettings + "CalcSettings.dat";  //DT
            //const string fileName = "C:\\CalcXpress\\CalcSettings.dat";//DT ACTIVE
            //string CalcDirectory = "C:\\CalcXpress";
            //Display("Before read filename");
            if (File.Exists(fileName))
            {
                //Display("Binary filename Exists");
                using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
                {
                    SaveFile = reader.ReadBoolean();
                    //CalcDirectory = reader.ReadString();
                }
            }
            if (SaveFile) SaveSettings.Checked = true;

            if (File.Exists(dirSettings + "FunctionTest.txt"))
            {
                //Display("Text file Exists");
                int lineCount = File.ReadAllLines(dirSettings + "FunctionTest.txt").Length;
                //int lineCount = File.ReadAllLines("C:\\CalcXpress\\FunctionTest.txt").Length;
                RecentStr = new string[lineCount];
                //Display("Lines : " + lineCount);
                OutPut("Lines: " + lineCount);
                recentCalcs = File.OpenText(dirSettings + "FunctionTest.txt");
                //recentCalcs = File.OpenText("C:\\CalcXpress\\FunctionTest.txt");
                for (int j = 0; j < lineCount; j++)  //while (recentCalcs.ReadLine() != null)
                {
                    RecentStr[j] = recentCalcs.ReadLine();
                    Recent.Items.Add(new ListViewItem("   " + RecentStr[j] + "\t" + "{" + (j + 1)));
                }

                recentCalcs.Close();
                cycle = lineCount;
                Recent.SelectedIndex = lineCount - 1; // cycle - 1;
                AcceptButton = EqualsTo;
            }

            else RecentStr = new string[cycle];
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            //WriteToDisk();
            Close();
        }
 
        private void WriteToDisk()
        {
            String dirSettings = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\CalcXpress\\";
            string fileSettings = dirSettings + "CalcSettings.dat";
            string fileCalcs = dirSettings + "FunctionTest.txt";
            StreamWriter saveCalc;
            if (!(Directory.Exists(dirSettings))) //Creates directory ALWAYS
            {
                System.IO.Directory.CreateDirectory(dirSettings);
            }
            if (!(File.Exists(fileSettings)))
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(fileSettings, FileMode.Create)))
                {
                    writer.Write(true);
                    writer.Write(fileSettings);
                }
            }

            if (SaveSettings.Checked == true)
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(fileSettings, FileMode.Open)))
                {
                    writer.Write(true);
                    writer.Write(fileSettings);
                }

                if (!(File.Exists(fileCalcs)))
                {
                    saveCalc = File.CreateText(fileCalcs);
                    foreach (string s in RecentStr) saveCalc.WriteLine(s);
                    saveCalc.Close();
                }
                else
                {
                    File.WriteAllLines(fileCalcs, RecentStr);//saveCalc = 
                }

            }
            else
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(fileSettings, FileMode.Create)))
                {
                    writer.Write(false);
                    writer.Write(fileSettings);
                }
            }
        }
        private void WriteToPendrive()
        {
            string dirSettings = Directory.GetCurrentDirectory() + "\\CalcXpress\\";//PD ACTIVE 
            string fileSettings = dirSettings + "CalcSettings.dat";
            string fileCalcs = dirSettings + "FunctionTest.txt";
            StreamWriter saveCalc;
            if (!(Directory.Exists(dirSettings))) //Creates directory ALWAYS
            {
                System.IO.Directory.CreateDirectory(dirSettings);
            }
            if (!(File.Exists(fileSettings)))
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(fileSettings, FileMode.Create)))
                {
                    writer.Write(true);
                    writer.Write(@fileSettings);
                }
            }

            if (SaveSettings.Checked == true)
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(fileSettings, FileMode.Open)))
                {
                    writer.Write(true);
                    writer.Write(@fileSettings);
                }

                if (!(File.Exists(fileCalcs)))
                {
                    saveCalc = File.CreateText(dirSettings + "FunctionTest.txt");
                    foreach (string s in RecentStr) saveCalc.WriteLine(s);
                    saveCalc.Close();
                }
                else
                {
                    File.WriteAllLines(dirSettings + "FunctionTest.txt", RecentStr);//saveCalc = 
                }

            }
            else
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(fileSettings, FileMode.Create)))
                {
                    writer.Write(false);
                    writer.Write(@dirSettings);
                }
            }
        }
        private void Button17_Click(object sender, EventArgs e) //CLEAR BUTTON
        {
            ResetAll();
        }
        void ResetAll()
        {
            display.ResetText();
            display.Refresh();
            display.Text += screenDefault;
            display.BackColor = Color.White;
            OutMessage.ResetText();
            ErrorCode = 0;
            currentType = 0;
            stack.Clear();
            oldValue.Clear();
            CheckValid.clearType();
            //flag = null;
            //resetFlag();
            buffer = "";
            OutMessageCode();
            calc = new Calculator();
            cloneStk = new Stack<Input>();
        }
        void resetFlag()
        {
            flag = new bool[11];
            flag[0] = false; //'radians / degree
            flag[1] = false; //Not in use* 
            flag[2] = false; //'actual display just saved to memory -prevents writing to display
            flag[3] = false; //'display erased after saving to memory -permits appending to display
            flag[4] = false; //Not in use
            flag[5] = false; //Not in use
            flag[6] = false; // Not in use
            flag[7] = false; //Not in use
            flag[8] = false; //Not in use
            flag[9] = false; //Not in use
            flag[10] = false; //Not in use
            //* in this version
        }
        void OutMessageCode()
        {
            OutPut("Comments: StringCalc_Debugger v.1.001.b \n");
            OutPut("Types: 1 - Numeric, 2 - Binary, 3 - Left operator, 4 - Right operator\n");
            OutPut("5 - Dot, 6 - Left brace, 7 - Right brace, 8 - Results, 9 - Constant, \n");
            OutPut("10 - Inverse function, 11 - Toggle sign, 12 - Minus simbol, 13 - Reciprocal\n");
            OutPut("14 - Error message, and 0 - Empty stack.\n");
        }
        public void setMessage(string expr)
        {
            message = expr;
            OutPut(message);
        }
        //STACK I/O STACK I/O STACK I/O STACK I/O STACK I/O STACK I/O STACK I/O STACK I/O STACK I/O STACK I/O 
        //STACK I/O STACK I/O STACK I/O STACK I/O STACK I/O STACK I/O STACK I/O STACK I/O STACK I/O STACK I/O 
        public bool Push(char newChar) //make a new overload to Push(Input Object) - re-use code  //MessageBox.Show("Is a char");
        {
            MemCatch();//clean an entry after saved to memory
            bool pushSuccess = false;
            OutPut("Before Push char" + "(" + newChar + ")" + " -Current type : " + CheckValid.peekType());
            if (CheckValid.validate(newChar, buffer))
            {
                ScreenReset();
                display.Text += newChar;
                aSymbol = new Symbols(newChar);
                anInput = new Input(aSymbol);
                int saveType = currentType;
                currentType = CheckValid.peekType();//persistent currentType, holds value from last until here
                anInput.setType(currentType);
                stack.Push(anInput);
                //Display("Char => " + newChar  + " Type: " + currentType);
                newChar = aSymbol.GetSymbol();
                //OutPut(" Stack: " + (stack.Peek()).getSymbol());
                OutPut("After Push char, New type : " + anInput.getType()); // CheckValid.peekType());
                buffer += newChar;

            }
            else
            {
                //Display(" Input: " + newChar + " Not valid for Current type: " + CheckValid.peekType());
                OutPut(" Input: " + newChar + " Not valid for Current type: " + currentType);
            }
            return pushSuccess;
        }
        public bool Push(string newFunction)
        {
            MemCatch();//clean an entry after saved to memory
            bool pushSuccess = false;
            //Display("Before Push string" + "(" + newFunction + ").  Current type : " + CheckValid.peekType());
            OutPut("Before Push string" + "(" + newFunction + ").  Current type : " + CheckValid.peekType());
            if (newFunction.Length > 0)
            {
                if ((CheckValid.validate(newFunction, buffer)) || CheckValid.validate(newFunction[0], buffer))
                {
                    ScreenReset();
                    display.Text += newFunction;
                    aFunction = new Functions(newFunction);
                    anInput = new Input(aFunction);
                    int saveType = currentType;
                    currentType = CheckValid.peekType(); //persistent currentType
                    //Display("Function => " + newFunction + "  type : " + currentType);
                    if (saveType == 8 && currentType == 4) anInput.setType(3);
                    else anInput.setType(currentType);
                    stack.Push(anInput);
                    //Display("Function => " + newFunction + "  type : " + currentType );
                    OutPut("Push: " + (stack.Peek()).getFunction());
                    OutPut("After Push string -> New type : " + anInput.getType()); //CheckValid.peekType());
                    buffer += newFunction;
                }
                else
                {
                    //Display(" Input: " + newFunction + " Not valid for Current type: " + CheckValid.peekType());
                    OutPut(" Input: " + newFunction + " Not valid for Current type: " + currentType);
                }
            }
            else ///the input string newFunction is empty
            {
                OutPut("The input string is empty, Count : " + Count());
            }
            return pushSuccess;
        }
        public void Pop(int termLength) //Uses Pop() to remove a term from everywhere
        {
            string temp = "";

            for (int i = stack.Count; i > 0; i--)
            {
                temp += stack.Peek().getItem();
                Pop();
                if (temp.Length >= termLength)
                {
                    break;
                }
            }//if the function were String, may return temp
        }
        public void Pop()
        {
            Input inType;
            if (stack.Count > 0)
            {
                currentType = CheckValid.peekType(); //   stack.Peek().getType()
                OutPut("Before Pop. Current type : " + currentType);
                inType = new Input(stack.Peek());
                string entry = inType.getItem();
                if (entry.Length > 0)
                {
                    if (buffer.Length >= entry.Length)
                    {
                        display.Text = buffer.Remove(buffer.Length - entry.Length);
                        buffer = display.Text;
                        OutPut("\nRemove: " + entry);
                    }
                }
                else
                {
                    OutPut("\nRemove: " + buffer);
                }
                stack.Pop();
                CheckValid.popType(); //Display("Pop type count: " + CheckValid.getCount() + " Type : " + CheckValid.peekType());
                if (currentType == 8 && oldValue.Count() > 0) oldValue.Pop();
                if (stack.Count > 0) currentType = stack.Peek().getType(); //CheckValid.peekType();
                else currentType = 0;
                //Display("OldValue count: " + oldValue.Count() + " Current type: " + currentType );
                OutPut("After Pop. Current type : " + currentType);
            }
            else
            {
                display.ResetText();
                currentType = 0;
            }

        }//END POP
        public static Stack<Input> getStack()
        {
            return stack;
        }
        public static int getItemType()
        {
            if (stack.Count > 0) return stack.Peek().getType();
            else return 0;
        }
        public static int getPrior()
        {
            if (stack.Count > 0) return stack.Peek().getType();
            else return 0;
        }
        public static void SetMode(int n)
        {
            if (stack.Count > 0) stack.Peek().setMode(n);
        }
        public static int GetMode()
        {
            if (stack.Count > 0) return stack.Peek().getMode();
            else return 0;
        }
        public string getBuffer()
        {
            return buffer;
        }
        void Display(String input)
        {
            System.Windows.Forms.MessageBox.Show(input);//End test
        }
        public char Peek()
        {
            char testChar = '\0';
            if (stack.Count > 0) testChar = stack.Peek().getChar();
            return testChar;
        }//Peek chars from the stack safely
        public int Count() { return (stack.Count); }
        public string PeekStr()
        {
            string testStr = "";
            if (stack.Count > 0) testStr = stack.Peek().getFunction();
            return testStr;
        }//Peek strings from the stack


        char screenPeek()
        {
            return (display.Text[display.Text.Length - 1]);
        }

        void ScreenReset()
        {
            if (display.Text == screenDefault)
            {
                display.ResetText();
            }
        }
        //END STACK I/O END STACK I/O END STACK I/O END STACK I/O END STACK I/O END STACK I/O END STACK I/O 
        //END STACK I/O END STACK I/O END STACK I/O END STACK I/O END STACK I/O END STACK I/O END STACK I/O 
        public void OutPut(string message)
        {
            OutMessage.AppendText(message + (" "));
            OutMessage.ScrollToCaret();
        }
        void test(Stack<Input> stack)//Display contents of stack from 0 to top (straight)
        {
            string temp = "";//Start test
            foreach (Input guest in stack)
            {
                temp += guest.getItem();
            }
            MessageBox.Show("Stack: " + temp);//End test
        }
        public static void SetFlags(int j, bool k)
        {
            flag[j] = k;
        }
        public static bool FlagStatus(int n)
        {
            return flag[n];
        }
        public static bool ExistsErrors()
        {
            return (ErrorCode > 0);
        }
        public static int getCode() { return ErrorCode; }
        public static void SetError(int EC)
        {
            ErrorCode = EC;
        }
        public void DisplayError()
        {
            string errMsg = GetError(getCode());// (ErrorCode);
            if (ErrorCode > 0) display.BackColor = Color.Red;
            Push(errMsg);
        }
        public string GetError(int code)
        {
            string Message = "";
            switch (code)
            {
                case 1:     //case 0: Means NO ERROR
                    {
                        Message = "<-Error: √root of (-number) yields a complex number, press 'C'";
                        break;
                    }
                case 2:
                    {
                        Message = "<-Error: (-base) ^ (Exp < 1, and divisible by 2), press 'C'";
                        break;
                    }
                case 3:
                    {
                        Message = "<-Error: Log(-number) Function not defined, press 'C'";
                        break;
                    }
                case 4:
                    {
                        Message = "<-Error: Sin_inv(number > 1) not defined. Press 'C'";
                        break;
                    }
                case 5:
                    {
                        Message = "<-Error: Cos_inv(number > 1) not defined. Press 'C'";
                        break;
                    }
                case 6:
                    {
                        Message = "<-Error: Ta∩(Θ = n * 90°) Invalid for Degrees. Press 'C' and select Radians";
                        break;
                    }
                case 7:
                    {
                        Message = "<-Error: Divission by 0 not defined. Press 'C'";
                        break;
                    }
                case 8:
                    {
                        Message = "<-Error: Factorial not defined for(-n), press 'C'";
                        break;
                    }
                case 9:
                    {
                        Message = "<-Error: Missing ')'";
                        break;
                    }
                case 10:
                    {
                        Message = "<-Error: Missing '(' -Press C";
                        break;
                    }
                case 11:
                    {
                        Message = "<-Error: Check '.' Press Backspace 'B'";
                        break;
                    }
                case 12:
                    {
                        Message = "<-Error: Cannot save empty string -Press 'C' ";
                        break;
                    }
                case 13:
                    {
                        Message = "<-Error: Braces missing in memory expression -Press 'MC' ";
                        break;
                    }
                case 14:
                    {
                        Message = "<-Error: No exponent makes base = 0 -Press 'B' or 'C'";
                        break;
                    }
                case 15:
                    {
                        Message = "<-Error: No support for chains of power, use 'X ^ (Y ^ Z)'";
                        break;
                    }
                case 16:
                    {
                        Message = "<-Error: Braces with an empty expression are not allowed -Press 'B' or 'C'";
                        break;
                    }
                case 17:
                    {
                        Message = "<-Error: Function not supported -'BackSpace' or 'Clear'";
                        break;
                    }
                case 18:
                    {
                        Message = "<-Error: Function not allowed for current expression -'BackSpace' or 'Clear'";
                        break;
                    }
                case 19:
                    {
                        Message = "<-Error: Incomplete expression -zero assumed- 'BackSpace' or 'Clear'";
                        break;
                    }
                default: { } break;
            }
            return Message;
        } //Finished GetError (Message)

        private void Inv_Click(object sender, EventArgs e)
        {
            if (invert == true)
            {
                invert = false;
                OutPut("Invert: " + invert);
                Inv.BackColor = Color.MistyRose;
                if (stack.Count > 0)
                {
                    if (PeekStr() == "ˉ¹")
                    {
                        Pop();
                    }
                }
            }
            else
            {
                string screen = display.Text;
                invert = true;   // Turn ON inverter
                OutPut("Invert: " + invert);
                Inv.BackColor = Color.Red;

                Push("ˉ¹");
            }
        }
        private void Button14_Click(object sender, EventArgs e)//TOGGLE SIGN BUTTON
        {
            string screen = display.Text;
            //if (CheckValid.ValidToggle(screen))
            toggleSign();
        }//End Button14 (toggle sign)
        char getSign(string buff)
        {
            char sign = '\0';
            string temp = reverse(getLastTerm(buff));

            if ((buff.Length > temp.Length) && temp.Length > 0) temp = buff.Remove(buff.Length - temp.Length); //term removed so sign is left
            if (temp.Length > 0)
            {
                sign = temp[temp.Length - 1];
            }
            else
            {
                sign = '+';
            }

            return sign;
        }
        private void Zero_Click(object sender, EventArgs e)
        {
            Push('0');
        }
        private void One_Click(object sender, EventArgs e)
        {
            Push('1');
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            Push('2');
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            Push('3');
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Push('4');
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Push('5');
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            Push('6');
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            Push('7');
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            Push('8');
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Push('9');
        }

        private void Dot_Click(object sender, EventArgs e)
        {
            Push('.');
        }

        private void Minus_Click(object sender, EventArgs e)
        {
            Push('-');
        }

        private void Button23_Click(object sender, EventArgs e)
        {
            Push('+');
        }

        private void Button20_Click(object sender, EventArgs e)
        {
            Push('x');
        }

        private void Button18_Click(object sender, EventArgs e)
        {
            Push('/');
        }

        private void Leftbrace_Click(object sender, EventArgs e)
        {
            Push('(');
        }

        private void Rightbrace_Click(object sender, EventArgs e)
        {
            if (ErrorCode == 9)
            {
                erase();
            }
            Push(')');
        }

        private void Sqrt_Click(object sender, EventArgs e)
        {
            Push("√");
        }

        private void Sine_Click(object sender, EventArgs e)
        {
            if (Hyp.Checked == true) Push("Sinh");
            else Push("Sin");
            if (invert)
            {
                Push(inverse);
            }
        }

        private void Cosine_Click(object sender, EventArgs e)
        {
            if (Hyp.Checked == true) Push("Cosh");
            else Push("Cos");
            if (invert)
            {
                Push(inverse);
            }
        }

        private void Tangent_Click(object sender, EventArgs e)
        {
            if (Hyp.Checked == true) Push("Tanh");
            else Push("Ta∩");
            if (invert)
            {
                Push(inverse);
            }
        }
        private void Log10_Click(object sender, EventArgs e)
        {
            Push("Log");
            if (invert)
            {
                Push(inverse);
            }
        }

        private void Pi_Click(object sender, EventArgs e)
        {
            Push("π");   //alt 227
        }

        private void PlusEXP_Click(object sender, EventArgs e)
        {
            Push("E+");
        }

        private void MinusEXP_Click(object sender, EventArgs e)
        {
            Push("E-");
        }

        private void Log2_Click(object sender, EventArgs e)
        {
            Push("ℓog₂");
            if (invert)
            {
                Push(inverse);
            }
        }

        private void Ln_Click(object sender, EventArgs e)
        {
            Push("Ɩŋ");
            if (invert)
            {
                Push(inverse);
            }
        }

        private void euler_Click(object sender, EventArgs e)
        {
            Push("e"); //e˟ (e ^ x) , "e°", ("eⁿ")
        }

        private void Sqr_Click(object sender, EventArgs e)
        {
            Push("²");
        }

        private void Percent_Click(object sender, EventArgs e)
        {
            Push("%");
        }

        private void Factorial_Click(object sender, EventArgs e)
        {
            Push("!");
        }

        private void Modulus_Click(object sender, EventArgs e)
        {
            Push("Mod");
        }

        private void Power_Click(object sender, EventArgs e)
        {
            Push("^");
        }

        private void Backspace_Click(object sender, EventArgs e)
        {
            erase();
        }
        public void erase()
        {
            if ((stack.Count() > 0) && (!(display.Text == screenDefault)))
            {
                Pop();
                if (ErrorCode > 0)
                {
                    display.BackColor = DefaultBackColor;
                    ErrorCode = 0;
                }
            }
            else if (stack.Count() == 0)
            {
                display.ResetText();
                display.Text = screenDefault;
                calc = new Calculator();
                buffer = "";
            }
        }

        /*=========================================================================================================================
         * EQUALS TO = EQUALS TO = EQUALS TO = EQUALS TO = EQUALS TO = EQUALS TO = EQUALS TO = EQUALS TO = EQUALS TO = EQUALS TO = 
         * ========================================================================================================================
         * */
        private void EqualsTo_Click(object sender, EventArgs e)
        {
            string screen = display.Text;   //Need a method to get a string out of the stack object (peek)
            Stack<int> type = CheckValid.getType();
            Calculator calc = new Calculator();
            bool valid = CheckValid.ValidStart(buffer);
            List<Input> aList = stack.ToList();
            //test(stack); //show the stack contents
            if (valid) //(stack.Count > 0)
            {
                result = calc.Start(stack, type);
                //int dec = int.Parse(DecFigures.GetItemText (DecFigures .SelectedItem));
                //string ResultStr = String.Format(result.ToString (), dec);
                //string ResultStr = NumberFor
                if (DecFigures.SelectedIndex > 0)
                {
                    int dec = DecFigures.SelectedIndex;
                    result = Math.Round(result, 15 - dec);
                }

                //Display("Result : " + result + "  Index : " + (15 - DecFigures .SelectedIndex)  );
                screen += equalSign + result.ToString();
                valid = CheckValid.ValidStart(buffer);
                //Display("Valid : " + valid);
                if (!(currentType == 8))//&& type.Peek () == 8), CheckValid.peekType ()
                {
                    if (valid)
                    {
                        //CheckValid.pushType(8);
                        Push(equalSign + result.ToString());
                        //CheckValid.pushType(8);
                        SetResult(result); //push the result into oldValues stack
                        int idx = cycle;// - 1;
                        cycle++;
                        string[] temp;
                        if (!(RecentStr.Length < 1))
                        {
                            temp = RecentStr;
                            RecentStr = new string[cycle];
                            RecentStr[idx] = screen;
                            for (int S = 0; S < temp.Length; S++) RecentStr[S] = temp[S];
                            RecentStr[RecentStr.Length - 1] = screen;
                        }
                        else
                        {
                            RecentStr = new string[cycle];
                            RecentStr[cycle - 1] = screen;
                        }
                        Recent.Items.Add(new ListViewItem("   " + screen + "\t" + "{" + (cycle)));
                        //Display("Idx : " + idx + "  Cycle : " + cycle);
                        Recent.SelectedIndex = idx;
                        WriteToPendrive();
                        //WriteToDisk();
                    }
                    else
                    {
                        DisplayError();
                        OutPut(" Error: " + ErrorCode);
                    }
                }
                //else if (CheckValid.getOpType().Contains(8))
                //{
                //    Validate check = new Validate();
                //    Stack<Input> newStack = new Stack<Input>();
                //    newStack.Push(GetResult());
                //    check.pushType(1);
                //    SetResult(calc.Start(newStack, check.getOpType()));
                //}

            }   //Valid 
            else
            {
                DisplayError();
                OutPut(" Error: " + ErrorCode);
            }
            //Calculator calc = new Calculator();
            //calc.calculate(screen);
        }
        /*=========================================================================================================================
 * EQUALS TO = EQUALS TO = EQUALS TO = EQUALS TO = EQUALS TO = EQUALS TO = EQUALS TO = EQUALS TO = EQUALS TO = EQUALS TO = 
 * ========================================================================================================================
 * */
        public void SetResult(double value) //set static anInput
        {
            Results newValue = new Results(value);
            anInput = new Input(newValue);
            anInput.setType(8); //the result is now a number: used only by calculator
            oldValue.Push(anInput);
            //Display("OldValue: " + oldValue.Peek().getItem() + " Count: " + oldValue.Count());
        }
        public static Input GetResult()
        {
            if (oldValue.Count > 0) return oldValue.Peek();
            else return null;
        } //returns static Input object and pop the stack
        private void display_TextChanged(object sender, EventArgs e)
        {
            //string term = display.Text;
            //display.ResetText();
            //chopTerms(term);
        }

        private void Reciproc_Click(object sender, EventArgs e)//toggle 1 / (X)
        {
            string screen = display.Text;
            string recip = reverse(getLastTerm(screen));
            //Display("Recip : " + recip + " Mode : " + GetMode() );
            if (GetMode() == 13)
            {
                Pop(); //remove type 13 and the last Push ( ")" ) 
                recip = reverse(getLastTerm(display.Text));
                Pop(recip.Length); //remove the original term from the screen
                Pop(3); //remove the trailing "(1/"
                chopTerms(recip); // Push(tmp); //Insert the original expression. The type will record the first char, who knows what.

                OutPut("Reciprocal, Type : " + CheckValid.peekType());
            }
            else if (!(recip == "") && !(recip == screenDefault) && !(currentType == 8) && !(GetMode() == 13))
            {
                Pop(recip.Length);
                chopTerms("(1/");  // Push("(1/");
                chopTerms(recip); // Push(recip);
                //MessageBox.Show("Recip : " + recip + " Type : " + type );
                Push(')');
                SetMode(13);
                OutPut("Out: (1/" + recip + ')');
            }                //MessageBox.Show("Type: " + CheckValid.peekType() + " Recip : " + recip);

        }//End Reciprocal (1 / X ) End Reciprocal (1 / X ) End Reciprocal (1 / X ) End Reciprocal (1 / X )

        private string getLastTerm(string buff)
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
                    else if (CheckValid.IsBinaryOp(buff[count]) || buff[count] == '\0' || buff[count] == ' ')
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

            return (lastTerm);
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
        private string reverse(string str)
        {
            string bitwise = "";
            int size = str.Length - 1;

            for (int i = size; i > -1; i--)
            {
                bitwise += str[i];
            }
            return bitwise;
        }
        //NAVIGATION NAVIGATION NAVIGATION NAVIGATION NAVIGATION NAVIGATION NAVIGATION NAVIGATION NAVIGATION 
        //NAVIGATION NAVIGATION NAVIGATION NAVIGATION NAVIGATION NAVIGATION NAVIGATION NAVIGATION NAVIGATION 
        private void More_Click(object sender, EventArgs e)
        {
            if (Trig.Visible == true)
            {
                Trig.Hide();
                Logs.Show();
            }
            else if (Logs.Visible == true)
            {
                Logs.Hide();
                Stats.Show();
            }
            else if (Stats.Visible == true)
            {
                Stats.Hide();
                SetComputer();
            }
            else if (Computer.Visible == true)
            {
                ResetComputer();
                Converter.Show();
            }
            else if (Converter.Visible == true)
            {
                Converter.Hide();
                Solver.Show();
            }
            else if (Solver.Visible == true)
            {
                Solver.Hide();
                Trig.Show();
            }
        }

        private void Back_Click(object sender, EventArgs e)
        {
            if (Trig.Visible == true)
            {
                Trig.Hide();
                Solver.Show();
            }
            else if (Solver.Visible == true)
            {
                Solver.Hide();
                Converter.Show(); //Shows computer tool set
            }
            else if (Converter.Visible == true)
            {
                Converter.Hide();
                SetComputer(); //Shows computer tool set
            }
            else if (Computer.Visible == true)
            {
                ResetComputer();
                Stats.Show();
            }
            else if (Stats.Visible == true)
            {
                Stats.Hide();
                Logs.Show();
            }
            else if (Logs.Visible == true)
            {
                Logs.Hide();
                Trig.Show();
            }
        }
        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutCalc about1 = new AboutCalc();
            about1.Show();
        }

        private void helpTopicsToolStripMenuItem_Click(object sender, EventArgs e)//Help topics
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)//Computer menu
        {
            SetComputer();
        }

        private void exponentialPanelToolStripMenuItem_Click(object sender, EventArgs e)//Logs menu
        {
            ResetComputer();
            Logs.BringToFront();
            Logs.Show();
        }

        private void trigonometryToolStripMenuItem1_Click(object sender, EventArgs e)//Trig menu
        {
            ResetComputer();
            Trig.BringToFront();
            Trig.Show();
        }
        private void trigonometryToolStripMenuItem_Click(object sender, EventArgs e)//Stats menu item
        {
            ResetComputer();
            Stats.BringToFront();
            Stats.Show();
        }
        private void unitConversionsToolStripMenuItem_Click(object sender, EventArgs e)//Unit converter
        {
            ResetComputer();
            Converter.BringToFront();
            Converter.Show();
        }

        private void systemSolverToolStripMenuItem_Click(object sender, EventArgs e)//System solver
        {
            ResetComputer();
            Solver.BringToFront();
            Solver.Show();
        }
        //END NAVIGATION END NAVIGATION END NAVIGATION END NAVIGATION END NAVIGATION END NAVIGATION END NAVIGATION 
        //END NAVIGATION END NAVIGATION END NAVIGATION END NAVIGATION END NAVIGATION END NAVIGATION END NAVIGATION 

        private void toggleSign()//No good. Too complex. Next: try using the stack
        {
            int type;
            string screen = display.Text;
            string term = reverse(getLastTerm(screen));
            //Display("Term : " + term);
            char sign = getSign(screen);
            char prior = '\0';
            if ((screen.Length - term.Length - 2) > 0) prior = screen[screen.Length - term.Length - 2];
            bool test = false;
            if (term.Length > 0) test = CheckValid.validate(term[term.Length - 1], term);
            type = CheckValid.peekType();
            CheckValid.popType();
            //Display("Term : " + term + " Type : " + type);
            if ((term.Length > 0) && !(term == screenDefault) && !(CheckValid.peekType() == 8) && !(type == 8)) //results NEVER modify
            {
                if (!(sign == '-')) //not signed
                {
                    Pop(term.Length);
                    Push('-');
                    chopTerms(term); // Push(term); //the first char will be the current type, NO GOOD!
                }
                else if (sign == '-') //signed
                {
                    Pop(term.Length);// + 1); (stack.Peek().getChar()) CheckValid.pushType(11);
                    if ((CheckValid.IsMinus(Peek()))) Pop();
                    if (CheckValid.IsNumeric(Peek()) || CheckValid.IsPi(Peek())) Push('+');
                    chopTerms(term); // Push(term);                   
                }
                OutPut("Toggled sign, Type : " + CheckValid.peekType());
            }

        }//FINISHED VOID TOGGLE-SIGN ()                     //MessageBox.Show("Term : " + term + " Peek : " + Peek() + " Sign : " + sign);
        private void toggle()
        {
            List<Input> input = stack.ToList();
            List<Input> temp = getLastTerm(input);
            int j = 0;

            for (int n = temp.Count - 1; n > 0; n--) //n[0] may have the sign
            {
                Pop();
            }
            if (temp.Count > 1)
            {
                if (CheckValid.onlyPositive(temp.ElementAt(0).getItem())) j++; //skip
                //if (temp.ElementAt (1).getChar() == '-')  

            }


            for (int i = j; i < temp.Count; i++)
            {
                if (!(temp[i].getItem() == null)) Push(temp[i].getItem());
                else if (!(temp[i].getChar() == '\0')) Push(temp[i].getChar());
            }
        }
        private List<Input> getLastTerm(List<Input> lst)
        {
            List<Input> last = new List<Input>();

            for (int i = lst.Count - 1; i >= 0; i--)
            {
                if (lst.ElementAt(i).getType() == 1 || lst.ElementAt(i).getType() == 5) last.Add(lst.ElementAt(i));
                else if (lst.ElementAt(i).getType() == 2 || lst.ElementAt(i).getType() == 12)
                {
                    last.Add(lst.ElementAt(i));
                    break;
                }
                else if (lst.ElementAt(i).getType() == 7)
                {
                    last.Concat(nestedTerm(lst));
                    break;
                }
                else
                {
                    last.Add(lst.ElementAt(i));
                    break;
                }
            }

            return last;
        }
        List<Input> nestedTerm(List<Input> list)
        {
            List<Input> lst = new List<Input>();

            return lst;
        }
        private void REC_Click(object sender, EventArgs e)
        {
            //if (HideDebug.Visible == false)
            //{
            //    RecntPanel.Hide();
            //    Recent.Hide();
            //    MemPanel.Show();
            //    HideDebug.Show();
            //    HideGraph.Show();
            //    Step.Show();
            //}
            //else
            //{
            //    RecntPanel.Show();
            //    Recent.Show();
            //    MemPanel.Hide();
            //    HideDebug.Hide();
            //    HideGraph.Hide();
            //    Step.Hide();
            //}
        }

        private void HideDebug_Click(object sender, EventArgs e)
        {
            if (!OutMessage.Visible == true)
            {
                OutPanel.Show();
                OutMessage.Show();
                ResizeRedraw = AutoSize;
            }
            else
            {
                OutPanel.Hide();
                OutMessage.Hide();
                ResizeRedraw = AutoSize;
            }
        }

        private void Recent_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Recent_DoubleClick(object sender, EventArgs e) //Import strings with the intention to process as keypad inputs
        {
            if (display.Text == screenDefault || display.Text == null)
            {
                String temp = RecentStr[Recent.SelectedIndex];
                chopTerms(temp);

            }
        }
        public void chopTerms(String input) //Get every single term, separated by "+, -, x, /". Includes √, ⁿ, ², etc.
        {
            String function = "";
            char number = '\0';
            double aValue = 0;
            //Display("Input : " + input);
            for (int j = 0; j < input.Length; j++)
            {
                if (CheckValid.IsNumeric(input[j]))// || CheckValid.IsConstant(input[j]))
                {
                    number = input[j];
                    Push(number);
                }
                else
                {
                    while (!(CheckValid.IsNumeric(input[j])) || input[j] == ' ') //collect a bunch of chars
                    {
                        function += input[j];
                        if (function == getFunctionName(function))
                        {
                            Push(function);
                            //Display("Function: " + function + " Type : " + currentType );// input[j]);
                            function = "";
                        }
                        j++; //OutPut("function: " + function);
                        if (j > input.Length - 1) break;
                    }//collected a bunch of chars
                    //Display("Function: " + function);
                    if (function.Contains(equalSign))
                    {
                        string next = aNumber.nextNumber(input, j);
                        //Display("Next : " + next);
                        if (next.Length > 0)
                        {
                            aValue = double.Parse(aNumber.nextNumber(input, j));
                            function += aValue;
                            j = aNumber.getIndex();// + 1;//function.Length;
                            Push(function);
                            SetResult(aValue); //Save each result in a stack; retrieve it on reverse (pop)
                        } //get the next number, append it to " = " + number
                        else
                        {
                            SetError(17);
                            DisplayError();
                        }
                    }
                    //Display("Function: " + function);

                    function = "";
                    j--;

                }//else (the input is not numeric)

            }//end main for loop

        }//End chopTerms()
        public bool isChar(String inStr)
        {
            bool aChar = false;
            if (inStr.Length == 1) aChar = true;
            return aChar;
        }
        public String getFunctionName(String fString)// Will need an enumerated class
        {
            String functionName = "";
            switch (fString)
            {
                case "e":
                    functionName = "e";    // e ° ⁿ
                    break;
                case "π":
                    functionName = "π";
                    break;
                case "ℓog₂":
                    functionName = "ℓog₂";
                    break;
                case "Ɩŋ":
                    functionName = "Ɩŋ";
                    break;
                case "Log":
                    functionName = "Log";
                    break;
                case "Mod":
                    functionName = "Mod";
                    break;
                case "Sin":
                    functionName = "Sin";
                    break;
                case "Cos":
                    functionName = "Cos";
                    break;
                case "Ta∩":
                    functionName = "Ta∩";
                    break;
                case "Sinh":
                    functionName = "Sinh";
                    break;
                case "Cosh":
                    functionName = "Cosh";
                    break;
                case "Ta∩h":
                    functionName = "Ta∩h";
                    break;
                case "ˉ¹":
                    functionName = "ˉ¹";
                    break;
                case "^":
                    functionName = "^";
                    break;
                case "(":
                    functionName = "(";
                    break;
                case ")":
                    functionName = ")";
                    break;
                case "!":
                    functionName = "!";
                    break;
                case "-":
                    functionName = "-";
                    break;
                case "+":
                    functionName = "+";
                    break;
                case "x":
                    functionName = "x";
                    break;
                case "/":
                    functionName = "/";
                    break;
                case "²":
                    functionName = "²";
                    break;
                case "√":
                    functionName = "√";
                    break;
                case "E+":
                    functionName = "E+";
                    break;
                case "E-":
                    functionName = "E-";
                    break;
                case "%":
                    functionName = "%";
                    break;
                case ".":
                    functionName = ".";
                    break;
                default:
                    //functionName = fString;
                    break;
            }

            return functionName;
        }//End getFunctionName()

        /* MEMORY FUNCTIONS MEMORY FUNCTIONS MEMORY FUNCTIONS MEMORY FUNCTIONS MEMORY FUNCTIONS MEMORY FUNCTIONS MEMORY FUNCTIONS 
         * MEMORY FUNCTIONS MEMORY FUNCTIONS MEMORY FUNCTIONS MEMORY FUNCTIONS MEMORY FUNCTIONS MEMORY FUNCTIONS MEMORY FUNCTIONS 
         */
        private void MemStore_Click(object sender, EventArgs e)//Store a number or expression to a memory location
        {
            int type = getItemType();
            Input memInput;
            memList = new List<Input>();
            memType = new Stack<int>();
            register = "Mem: ";
            if (type == 8) //Stores the result of an expression
            {
                memInput = new Input(oldValue.Peek());
                memList.Add(memInput);
                memType.Push(memInput.getType());
                register += memInput.getItem();
                M.ForeColor = Color.Red;
                flag[2] = true; //memory active -prevents appending
                flag[3] = true; //memory saved -permit appending
            }
            else if (type == 1 || type == 3 || type == 9 || type == 7) //Stores a number, a constant, or a whole expression
            {
                memList = stack.Reverse().ToList();
                foreach (Input I in memList)
                {
                    register += I.getItem() + " ";
                    memType.Push(I.getType());
                }
                M.ForeColor = Color.Red;
                flag[2] = true; //memory active
                flag[3] = true;
            }
        }
        private void MemCatch()//clean an entry after saved to memory
        {
            if (flag[3])
            {
                MemPanel.BringToFront();
                MemShow.Text = register;
                MemShow.Show();
                ResetAll();
                flag[3] = false;
            }
        }
        private void MemRecall_Click(object sender, EventArgs e)
        {
            if (flag[3]) { //06/15/23
            bool valid = false;
            int type = CheckValid.peekType();
            if (flag[2] && !(type == 1 || type == 8))
            {
                foreach (Input I in memList)
                {
                    valid = Push(I.getItem());
                    if (!valid) ErrorCode = 18; //Function not allowed for the current expression
                }
            }
            else
            {
                ResetAll();
                foreach (Input I in memList)
                {
                    valid = Push(I.getItem());
                    if (!valid) ErrorCode = 18; //Function not allowed for the current expression
                }
            }
                        }   
        }
        private void MemPlus_Click(object sender, EventArgs e) //UPDATE*******UPDATE*******UPDATE MEMORY
        {
            if (stack.Count > 0 && flag[2])//there is a current input, and there is contents in memory
            {
                Stack<Input> memStack = new Stack<Input>();
                Calculator calc = new Calculator();
                Symbols plus = new Symbols('+');
                Input add = new Input(plus);
                add.setType(2);
                memList.Add(add);
                memType.Push(add.getType());
                double currentNumber = calc.Start(stack, CheckValid.getType());
                Results current = new Results(currentNumber);
                Input curr = new Input(current);
                curr.setType(1);
                memList.Add(curr);
                memType.Push(curr.getType());

                foreach (Input I in memList)//convert to stack to call Start
                {
                    memStack.Push(I);
                }
                double memNumber = calc.Start(memStack, memType);
                //Display("Current : " + currentNumber + "  Memory : " + memNumber);
                Functions equal = new Functions(" = " + memNumber);
                Input M_Result = new Input(equal);
                register = "Mem+ : " + M_Result.getItem();
                MemShow.Text = register;
                flag[3] = true;
            }

        }
        private void MemClear_Click(object sender, EventArgs e)
        {
            register = "";
            MemShow.ResetText();
            MemShow.Hide();
            if (MemPanel.Visible == true) MemPanel.SendToBack();
            M.ForeColor = Color.Blue;
            flag[2] = false;
        }
        private void M_Click(object sender, EventArgs e)
        {
            if (flag[2] == true)
            {
                if (MemShow.Visible == true)
                {
                    MemPanel.SendToBack();
                    MemShow.Hide();
                }
                else
                {
                    RecntPanel.SendToBack();
                    MemPanel.BringToFront();
                    MemShow.Text = register;
                    MemShow.Show();
                }
            }
        }
        /* MEMORY FUNCTIONS MEMORY FUNCTIONS MEMORY FUNCTIONS MEMORY FUNCTIONS MEMORY FUNCTIONS MEMORY FUNCTIONS MEMORY FUNCTIONS 
         * MEMORY FUNCTIONS MEMORY FUNCTIONS MEMORY FUNCTIONS MEMORY FUNCTIONS MEMORY FUNCTIONS MEMORY FUNCTIONS MEMORY FUNCTIONS 
         */
        private void SaveRecent_Click(object sender, EventArgs e)
        {
            if (SaveSettings.Checked == true) SaveSettings.Checked = false;
            else SaveSettings.Checked = true;
        }

        private void RecntPanel_Paint(object sender, PaintEventArgs e)
        {

        }
        private void DecFigures_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void DecUpbtn_Click(object sender, EventArgs e)
        {
            int index = DecFigures.SelectedIndex;
            if (index == 0) DecFigures.SelectedIndex = 15;
            else DecFigures.SelectedIndex = index - 1;
        }

        private void DecUpbtn_Click_1(object sender, EventArgs e)
        {
            int index = DecFigures.SelectedIndex;
            if (index == 15) DecFigures.SelectedIndex = 0;
            else DecFigures.SelectedIndex = index + 1;
        }
        private void OutMessage_TextChanged(object sender, EventArgs e)
        {
        }
        private void showPoints()
        {
            if (OutMessage.Visible)
            {
                OutMessage.Hide();
                graphicsBox1.Show();
                using (var g = Graphics.FromImage(graphicsBox1.Image))//an inner class?
                {
                    Pen blackpen = new Pen(Color.Black, 2);
                    Point A = new Point(10, 80);// Points A and B are class System.Drawing, NOT MyClass.Point
                    Point B = new Point(100, 80);//An array of points that result from evaluation at a fixed step
                    g.DrawLine(blackpen, A, B);
                    graphicsBox1.Refresh();
                }
            }
            else
            {
                graphicsBox1.Hide();
                OutMessage.Show();
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void HideGraph_Click(object sender, EventArgs e)
        {
            showPoints();
        }

        private void About_Click(object sender, EventArgs e)
        {
            AboutCalc about1 = new AboutCalc();
            about1.Show();
        }

        private void Hyp_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Radians_CheckedChanged(object sender, EventArgs e)
        {
            if (flag[0] == false) flag[0] = true;
            else flag[0] = false;
        }

        private void SaveSettings_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click_1(object sender, EventArgs e)//STEP THROUGH BUTTON
        {
            //if (cloneStk.Count > 1) result = calc.Start(cloneStk, CheckValid.getType());
            //else result = calc.Start(stack, CheckValid.getType());
            if (!(currentType == 8))
            {
                result = calc.Start(stack, CheckValid.getType());
                Stack<Input> tmpStack = new Stack<Input>();
                List<Input> tmp = calc.stepThrough();
                if (tmp.Count() == stack.Count())//cloneStk.Count())
                {
                    tmp = calc.stepThrough();
                }
                //Display("Compare : " + (tmp.Count() == cloneStk.Count()));
                if (!(stack.Count == 1))//(cloneStk.Count == 1))
                {
                    if (stack.Count > 1)
                    {
                        Push(" ═ "); //<alt> 205 != " = "
                        foreach (Input I in tmp)
                        {
                            if (!(I.getItem() == null)) Push(I.getItem());
                            else Push(I.getOutput().ToString());
                            tmpStack.Push(I);
                        }
                        stack = tmpStack; // cloneStk = tmpStack;
                    }
                    else
                    {
                        stack = new Stack<Input>();
                        calc = new Calculator();
                    }
                }
                else calc = new Calculator();
            }
        }
        void ShowTypes(string message, List<Input> aList)
        {
            string buff = "";
            foreach (Input guest in aList)
            {
                buff += guest.getType() + "  ";
            }
            MessageBox.Show(message + buff);
        }
        //COMPUTER TOOLS COMPUTER TOOLS COMPUTER TOOLS COMPUTER TOOLS COMPUTER TOOLS COMPUTER TOOLS COMPUTER TOOLS 
        //COMPUTER TOOLS COMPUTER TOOLS COMPUTER TOOLS COMPUTER TOOLS COMPUTER TOOLS COMPUTER TOOLS COMPUTER TOOLS 
        private void Computer_Paint(object sender, PaintEventArgs e)
        {
        }
        private void SetComputer()//show all controls
        {
            buffer = display.Text;
            display.ResetText();
            ScreenReset();
            panelComp.BringToFront();
            panelComp.Show();
            Computer.BringToFront();
            Computer.Show();
            base10lbl.Show();
            base10Txt.Show();
            base10Txt.Focus();
            hexTxt.Show();
            hexLbl.Show();
            octLbl.Show();
            octalTxt.Show();
            binaryLbl.Show();
            binaryTxt.Show();
            ClearComp.Show();
            CompBackspace.Show();
            cmpOut.BringToFront();
            cmpOut.Show();
            cmpOutputBin.BringToFront();
            cmpOutputBin.Show();
        }
        private void ResetComputer() //clear all text, hide all controls
        {
            display.Text = buffer;
            display.Show();
            Computer.Hide();
            panelComp.Hide();
            base10lbl.Hide();
            base10Txt.Hide();
            hexTxt.Hide();
            hexLbl.Hide();
            octLbl.Hide();
            octalTxt.Hide();
            binaryLbl.Hide();
            binaryTxt.Hide();
            ClearComp.Hide();
            CompBackspace.Hide();
            cmpOut.Hide();
        }
        private void ClearComp_Click(object sender, EventArgs e)
        {
            CompClr();
        }
        private void CompClr()
        {
            base10Txt.ResetText();
            hexTxt.ResetText();
            octalTxt.ResetText();
            binaryTxt.ResetText();
            LogicReset();

        }
        private void CompBackspace_Click(object sender, EventArgs e)
        {
            if (buffer2.Length > 0)
            {
                buffer2 = buffer2.Remove(buffer2.Length - 1);
                if (buffer2.Length == 0) CompClr();
                else
                {
                    if (Decbtn.Checked == true) base10Txt.Text = buffer2;
                    else if (hexBtn.Checked == true) hexTxt.Text = buffer2;
                    else if (binaryBtn.Checked == true) binaryTxt.Text = buffer2;
                    else if (octBtn.Checked == true) octalTxt.Text = buffer2;
                    CompUpdate();
                }
            }
        }
        public void insert(char input) // Needs Validate
        {
            if (!(LogicOpInTx.Text == "Select"))
            {
                insert2(input);
            }
            else
            {
                NumberBase base2 = new NumberBase();
                if (Decbtn.Checked == true)
                {
                    base10Txt.Text += input;
                    CompUpdate();
                }
                else if (hexBtn.Checked == true)
                {
                    hexTxt.Text += input;
                    CompUpdate();
                }
                else if (binaryBtn.Checked == true)
                {
                    if (binaryTxt.Text.Length % 5 == 0) binaryTxt.Text += ' ';
                    binaryTxt.Text += input;
                    CompUpdate();
                }
                else if (octBtn.Checked == true)
                {
                    octalTxt.Text += input;
                    CompUpdate();
                }
            }
        }
        public void insert2(char input) ///needs to convert bases as per selected base
        {
            if (!(binaryBtn.Checked)) cmpIn2ByType.Text += input;
            else
            {
                cmpInput2Bin.Text += input;
                string bin = cmpInput2Bin.Text;

            }
        }
        public void CompUpdate()
        {
            NumberBase base2 = new NumberBase();
            if (Decbtn.Checked == true)
            {
                buffer2 = base10Txt.Text; //buffer2 will keep the value after exit
                double number = aNumber.convert(buffer2);
                string bin = base2.TenToBin(number);
                binaryTxt.Text = bin;
                string hex = base2.BinToHex(bin);
                hexTxt.Text = hex;
                string oct = base2.BinToOctal(bin);
                octalTxt.Text = oct;
                //Display("Bin : " + bin + "  Hex : " + hex);
            }
            else if (hexBtn.Checked == true)
            {
                buffer2 = hexTxt.Text;
                string bin = base2.HexToBin(buffer2);
                binaryTxt.Text = bin;
                double ten = base2.BinToTen(bin);
                base10Txt.Text = ten.ToString();
                string oct = base2.BinToOctal(bin);
                octalTxt.Text = oct;
            }
            else if (binaryBtn.Checked == true)
            {
                buffer2 = binaryTxt.Text;
                double ten = base2.BinToTen(buffer2);
                base10Txt.Text = ten.ToString();
                string hex = base2.BinToHex(buffer2);
                hexTxt.Text = hex;
                string oct = base2.BinToOctal(buffer2);
                octalTxt.Text = oct;
            }
            else if (octBtn.Checked == true)
            {
                buffer2 = octalTxt.Text;
                string bin = base2.OcToBin(buffer2);
                binaryTxt.Text = bin;
                string hex = base2.BinToHex(bin);
                hexTxt.Text = hex;
                double ten = base2.BinToTen(bin); //?????
                base10Txt.Text = ten.ToString();
            }
            //cmpInput.Text = buffer2;
        }
        private void ResetButtons()
        {
            base10Txt.ResetText();
            binaryTxt.ResetText();
            hexTxt.ResetText();
            octalTxt.ResetText();

            if (Decbtn.Checked == true)
            {
                bA.Enabled = false;
                bB.Enabled = false;
                bC.Enabled = false;
                bD.Enabled = false;
                bE.Enabled = false;
                bF.Enabled = false;
                b2.Enabled = true;
                b3.Enabled = true;
                b4.Enabled = true;
                b5.Enabled = true;
                b6.Enabled = true;
                b7.Enabled = true;
                b8.Enabled = true;
                b9.Enabled = true;
            }
            else if (hexBtn.Checked == true)
            {
                b2.Enabled = true;
                b3.Enabled = true;
                b4.Enabled = true;
                b5.Enabled = true;
                b6.Enabled = true;
                b7.Enabled = true;
                b8.Enabled = true;
                b9.Enabled = true;
                bA.Enabled = true;
                bB.Enabled = true;
                bC.Enabled = true;
                bD.Enabled = true;
                bE.Enabled = true;
                bF.Enabled = true;
            }
            else if (octBtn.Checked == true)
            {
                bA.Enabled = false;
                bB.Enabled = false;
                bC.Enabled = false;
                bD.Enabled = false;
                bE.Enabled = false;
                bF.Enabled = false;
                b8.Enabled = false;
                b9.Enabled = false;
                b2.Enabled = true;
                b3.Enabled = true;
                b4.Enabled = true;
                b5.Enabled = true;
                b6.Enabled = true;
                b7.Enabled = true;

            }
            else if (binaryBtn.Checked == true)
            {
                bA.Enabled = false;
                bB.Enabled = false;
                bC.Enabled = false;
                bD.Enabled = false;
                bE.Enabled = false;
                bF.Enabled = false;
                b2.Enabled = false;
                b3.Enabled = false;
                b4.Enabled = false;
                b5.Enabled = false;
                b6.Enabled = false;
                b7.Enabled = false;
                b8.Enabled = false;
                b9.Enabled = false;
            }

        }

        private void Dec_CheckedChanged(object sender, EventArgs e)
        {
            ResetButtons();
            base10Txt.Focus();
        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            ResetButtons();
            binaryTxt.Focus();
            b1.Enabled = true;
            b0.Enabled = true;
        }

        private void RadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            ResetButtons();
            hexTxt.Focus();
        }

        private void RadioButton4_CheckedChanged(object sender, EventArgs e)
        {
            ResetButtons();
            octalTxt.Focus();
        }

        private void b1_Click(object sender, EventArgs e)
        {
            insert('1');
        }

        private void button15_Click(object sender, EventArgs e)
        {
            insert('0');
        }

        private void b2_Click(object sender, EventArgs e)
        {
            insert('2');
        }

        private void b3_Click(object sender, EventArgs e)
        {
            insert('3');
        }

        private void b4_Click(object sender, EventArgs e)
        {
            insert('4');
        }

        private void b5_Click(object sender, EventArgs e)
        {
            insert('5');
        }

        private void b6_Click(object sender, EventArgs e)
        {
            insert('6');
        }

        private void b7_Click(object sender, EventArgs e)
        {
            insert('7');
        }

        private void b8_Click(object sender, EventArgs e)
        {
            insert('8');
        }

        private void b9_Click(object sender, EventArgs e)
        {
            insert('9');
        }

        private void bA_Click(object sender, EventArgs e)
        {
            insert('A');
        }

        private void bB_Click(object sender, EventArgs e)
        {
            insert('B');
        }

        private void bC_Click(object sender, EventArgs e)
        {
            insert('C');
        }

        private void bD_Click(object sender, EventArgs e)
        {
            insert('D');
        }

        private void button2_Click_1(object sender, EventArgs e)//bE
        {
            insert('E');
        }

        private void bF_Click(object sender, EventArgs e)
        {
            insert('F');
        }

        private void base10Txt_TextChanged(object sender, EventArgs e)
        {
        }
        public string CompLogic(string Oper)
        {
            LogicOper BinaryOp = new LogicOper();
            NumberBase base1 = new NumberBase();
            string upper = binaryTxt.Text;
            string lower = cmpInputBin.Text;
            string result = BinaryOp.LogicBinOp(upper, lower, Oper);
            if (Decbtn.Checked == true)
            {
                double ten = base1.BinToTen(result);
                result = ten.ToString();
            }
            else if (hexBtn.Checked == true)
            {
                result = base1.BinToHex(result);
            }
            else if (octBtn.Checked == true)
            {
                result = base1.BinToOctal(result);
            }

            return result;
        }
        private void cmpInput_TextChanged(object sender, EventArgs e)
        {

        }

        private void AND_Gate_Click(object sender, EventArgs e)
        {
            string Oper = "AND";
            LogicOpInTx.Text = Oper;
        }

        private void OR_Gate_Click(object sender, EventArgs e)
        {
            string Oper = "OR";
            LogicOpInTx.Text = Oper;
            string output = CompLogic(Oper);
            cmpOutputBin.Text = output;
        }

        private void NOT_Gate_Click(object sender, EventArgs e)
        {
            LogicOpInTx.Text = "NOT:";
        }

        private void NAND_Click(object sender, EventArgs e)
        {
            LogicOpInTx.Text = "NAND:";
        }

        private void NOR_Click(object sender, EventArgs e)
        {
            LogicOpInTx.Text = "NOR:";
        }

        private void ShR_Click(object sender, EventArgs e)
        {
            LogicOpInTx.Text = "SHIFT-R:";
        }

        private void ShLf_Click(object sender, EventArgs e)
        {
            LogicOpInTx.Text = "SHIFT-L:";
        }

        private void cmpDivide_Click(object sender, EventArgs e)
        {
            LogicOpInTx.Text = "DIV /:";
        }

        private void cmpMultiply_Click(object sender, EventArgs e)
        {
            LogicOpInTx.Text = "MULT x:";
        }

        private void cmpSubstract_Click(object sender, EventArgs e)
        {
            LogicOpInTx.Text = "SUBS -:";
        }

        private void cmpAdd_Click(object sender, EventArgs e)
        {
            LogicOpInTx.Text = "ADD +:";
        }

        private void LogicOpInTx_TextChanged(object sender, EventArgs e)
        {

        }

        private void LogicOpOutx_TextChanged(object sender, EventArgs e)
        {

        }

        private void LogicDo_Click(object sender, EventArgs e)
        {

        }

        private void cmpReset_Click(object sender, EventArgs e)
        {
            LogicReset();
        }
        private void LogicReset()
        {
            LogicOpInTx.Text = "Select";
            cmpInputBin.ResetText();
            cmpInput2Bin.Text = "";
            cmpOutputBin.ResetText();
        }

        private void cmpInput2_TextChanged(object sender, EventArgs e)
        {
            LogicOper BinaryOp = new LogicOper();
            string upper = binaryTxt.Text;
            string lower = cmpInput2Bin.Text;
            string oper = LogicOpInTx.Text;
            Numbers numb = new Numbers();
            NumberBase BaseConvert = new NumberBase();
            string result = "";
            if (Decbtn.Checked)
            {
                double dec = numb.convert(lower);
                lower = BaseConvert.TenToBin(dec);
                //Display("Lower:  " + lower);
            }
            else if (hexBtn.Checked)
            {
                string hex = BaseConvert.HexToBin(lower);
                lower = hex;
                //Display("Lower:  " + lower);
            }
            else if (octBtn.Checked)
            {
                string oct = BaseConvert.OcToBin(lower);
                lower = oct;
            }
            else
            {
                lower = binaryTxt.Text;
            }
            result = BinaryOp.LogicBinOp(upper, lower, oper);

            cmpOutputBin.Text = FormatBin(result);
        }

        private string FormatBin(string result)
        {
            int count = 0;
            string temp = "";
            foreach (char I in result)
            {
                if (I == ' ') continue;
                else if (count % 4 == 0) temp += " ";
                temp += I;
                count++;
            }
            return temp;
        }

        private void binaryTxt_TextChanged(object sender, EventArgs e)
        {
            cmpInputBin.Text = binaryTxt.Text;
            if (Decbtn.Checked)
            {
                cmpInByType.Text = base10Txt.Text;
                typIn.Text = "d";
            }
            else if (hexBtn.Checked)
            {
                cmpInByType.Text = hexTxt.Text;
                typIn.Text = "h";
            }
            else if (octBtn.Checked)
            {
                cmpInByType.Text = octalTxt.Text;
                typIn.Text = "oct";
            }
        }

        private void cmpIn2ByType_TextChanged(object sender, EventArgs e)
        {

        }

        private void eq_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        //END COMPUTER TOOLS END COMPUTER TOOLS END COMPUTER TOOLS END COMPUTER TOOLS END COMPUTER TOOLS 
        //END COMPUTER TOOLS END COMPUTER TOOLS END COMPUTER TOOLS END COMPUTER TOOLS END COMPUTER TOOLS 




        //                                              DO NOT WRITE BELOW THIS LINE
    }//END PUBLIC PARTIAL CLASS FORM 1

}  //END NAMESPACE(CALCULATORXPRESS)
