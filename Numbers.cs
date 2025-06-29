using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; //Just to show messagebox. REMOVE after tested
/*
 * Stores numeric characters only, one by one. Methods to extract fractions and EE notation.
 * Should include other methods to get numeric strings, overloaded functions to exclude EE, 
 * and to return a double type (EE included)
 * */
namespace CalculatorXpress
{
    public partial class Numbers //: Input
    {
        //private double number;
        private int index;
        private static Validate check;

        public Numbers() {
            //number = 0.0;
            index = 0;
            check = new Validate();
        }

        //public void setNumber(double aNumber)
        //{
        //    this.number = aNumber;
        //}

        //public double GetNumber()
        //{
        //    return (number);
        //}
        public void setIndex(int idx)
        {
            this.index = idx;
        }
        public int getIndex()
        {
            return (this.index);
        }

        public double convert(String numberStr)//BIG FAT RAT - SIGNED NUMBERS ! --Fixed
        {
            setIndex(0);
            double value = 0;
            double fraction = 0;
            int size = numberStr.Length;
            bool noNumbers = (numberStr == null);
            //display("Numbers on start: " + numberStr);
            if (noNumbers) return value; //This is for test only. REMOVE after work is complete
            while (index < size) 
            {
                if (check.IsNumeric(numberStr[index])) //returns with a double (number + fraction)
                {
                    value = getNumber(numberStr, ref index);
                }
                else if (numberStr[index] == '.') //returns with a double (fraction only)
                {
                    index++;
                    fraction = getFraction(numberStr, ref index);
                    value += fraction;
                }
                else if (numberStr[index] == '-')
                {
                    index++;
                    value = -(getNumber(numberStr, ref index));
                }

                else if (numberStr[index] == 'E') //returns a double (next number + fraction after the 'E')
                {
                    index++;
                    int sign = index;
                    index++;
                    //System.Windows.Forms.MessageBox.Show("E: " + number + " Starting index: " + index);
                    double E = getNumber(numberStr, ref index);

                    if (numberStr[sign] == '+')
                    {
                        value = value * Math.Pow(10, E);
                    }
                    else
                    {
                        value = value / Math.Pow(10, E);
                    }
                }
                else if (numberStr[index] == 'π') return Math.PI;
                else // no more numbers, no dot, no E
                {
                    break;
                }

                //System.Windows.Forms.MessageBox.Show("While: " + number + " End Index: " + index);
            }//End while
            //display("Numbers at the end: " + value);
            return value;
        }
        public double getNumber(String buff, ref int n)//should return a double(int + fraction), NO E's at this point
        {
            double value = 0;
            
            while (n < buff.Length)// need to check if number while under buffer length
            {
                if(check.IsNumeric(buff[n]))
                {
                    value = (value * 10) + (buff[n] - '0');
                    n++;
                }
                else if(buff[n] == '.')
                {
                    n++;
                    value += getFraction(buff, ref n);
                    break;
                }
                else //anything not a number or dot (including E)
                {
                    break;
                }
                //System.Windows.Forms.MessageBox.Show("Number: " + value);
            }//end while

            return value;
        }
        public double getFraction(String buff, ref int n)
        {
            double fraction = 0;
            double rate = 1;

                while (n < buff.Length) //if n is already > length: @&%!!
                {
                    if (check.IsNumeric(buff[n]))
                    {
                        rate /= 10;
                        fraction = (fraction) + (buff[n] - '0') * rate;
                        n++;
                    }
                    else
                    {
                    break;
                    }                   
                }
            //System.Windows.Forms.MessageBox.Show("Fraction: " + buff);
            return fraction;
        }

        public String nextNumber(String buff, int n){ //Service to external parts. Gets the next numeric string starting at n
            String next = "";                         // may be useful before convert
            
            while (n < buff.Length)
            {
                if (check.IsNumeric(buff[n]) || check.IsDot (buff[n]))
                {
                    next += buff[n];
                    n++;                   
                }
                else if (buff[n] == 'E')
                {
                    next += buff[n];
                    n++;
                    next += buff[n]; //get the sign for power of ten
                    n++;
                }
                else
                {
                    break;
                }
            }
            setIndex(n);
            return next;
        }//End String nextNumber()
        void display(string message)
        {
            System.Windows.Forms.MessageBox.Show(message);
        }
    }
}
