using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorXpress
{
    class LogicOper
    {
        public LogicOper() { }
        
        public string UnaryOut(string inStr, string OpStr)
        {
            string result = "";
            switch (OpStr)
            {
                case "NOT":
                    foreach(char d in inStr)
                    {
                        if (d == '1') result += '0';
                        else result += '1';
                    }
                    break;
                case "SHIFT-R":
                    result = "0";
                    for(int i = 0, j = i + 1; j < inStr.Length; j++)
                    {
                        result += inStr[j];
                    }
                    break;
                case "SHIFT-L":
                    for (int i = 0, j = i + 1; j < inStr.Length; j++)
                    {
                        result += inStr[j];
                    }
                    result += "0";
                    break;
            }

            return result;
        }
        public string LogicBinOp(string up, string low, string OpStr)
        {
            string result = "";
            string upper = "";
            string lower = "";
            int size = 0;
            int threshold = 0;
            for (int n = 0; n < up.Length; n++)//take spaces off the string
            {
                if (up[n] == ' ') continue;
                else upper += up[n];
            }
            for (int m = 0; m < low.Length; m++)
            {
                if (low[m] == ' ') continue;
                else lower += low[m];
            }
            if (upper.Length - lower.Length >= 0)
            {
                size = upper.Length;
                threshold = (size - lower.Length) - 1;
                lower = normalize(lower, size);
            }
            else
            {
                size = lower.Length;
                threshold = (size - upper.Length)- 1;
            }
            //System.Windows.Forms.MessageBox.Show("Threshold : " + threshold);
            string reverse = "";
            switch (OpStr)
            {
                case "AND":
                    for(int i = size - 1; i >= 0; i--)
                    {
                        if (upper[i] == '1' && lower[i] == '1') result += '1';
                        else result += '0';
                    }
                    for (int N = result.Length - 1; N > -1; N--) reverse += result[N];
                    //display("Result : " + reverse + "  Upper : " + upper + "  Lower : " + lower);
                    return reverse;
                case "OR":
                    //System.Windows.Forms.MessageBox.Show("Upper : " + upper + "  Lower : " + lower);
                    for (int i = size - 1; i >= 0; i--)
                    {
                        if (upper[i] == '1' || lower[i] == '1') result += '1';
                        else result += '0';
                        //System.Windows.Forms.MessageBox.Show("Result : " + result);
                    }
                    for (int N = result.Length - 1; N > -1; N--) reverse += result[N];
                    return reverse;
                case "NAND":
                    for (int i = size - 1; i >= 0; i--)
                    {
                        if (upper[i] == '0' && lower[i] == '0') result += '1';
                        else result += '0';
                    }
                    for (int N = result.Length - 1; N > -1; N--) reverse += result[N];
                    return reverse;
                case "NOR": //0 0  1, 0 1  0, 1 0  0, 1 1  0 
                    for (int i = size - 1; i >= 0; i--)
                    {
                        if (upper[i] == '1' || lower[i] == '1') result += '0';
                        else result += '1';
                    }
                    for (int N = result.Length - 1; N > -1; N--) reverse += result[N];
                    return reverse;
                case "XOR":
                    for (int i = size - 1; i >= 0; i--)
                    {
                        if (upper[i] == lower[i]) result += '0';
                        else result += '1';
                    }
                    for (int N = result.Length - 1; N > -1; N--) reverse += result[N];
                    return reverse;
                case "ADD +":   //functions with carry
                    string temp = "";
                    bool carry = false;
                    for (int k = 0; k <= size - threshold; k++) temp += '0'; //make a string with shifted right
                    if(!(upper.Length == lower.Length))//so both terms are of same length (zero filled)
                    {
                        if (size == upper.Length)
                        {
                            temp += lower;
                            lower = temp;
                        }
                        else
                        {
                            temp += upper;
                            upper = temp;
                        }
                    }
                    for (int i = size - 1; i > -1; i--)
                    {
                        if (upper[i] == '1' && lower[i] == '1')
                        {
                            if (carry) result += '1';
                            else
                            {
                                result += '0';
                                carry = true;
                            }
                        }
                        else if (upper[i] == '0' && lower[i] == '0')
                        {
                            if (carry)
                            {
                                result += '1';
                                carry = false;
                            }
                            else result += '0';                            
                        }
                        else if(!(upper[i] == lower[i]))
                        {
                            if (carry) result += '0';
                            else result += '1';
                        }                                
                    }
                    break;
                case "SUBS -":
                    break;
                case "MULT x":
                    break;
                case "DIV /":
                    break;
            }

            return result;
        }
        public string normalize(string right, int length)//returns a zero filled higher byte 
        {
            string normal = "";
            int size = length - right.Length;
            for (int i = 0; i < size; i++) normal += '0';
            normal += right;

            return normal;
        }
        public void display(string message)
        {
            System.Windows.Forms.MessageBox.Show(message);
        }
    }
}
