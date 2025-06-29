using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 * This class is posed to include each and every function in one place,
 * so every operation will need an object of this. It includes all types,
 * left-hand, right-handed, binary, unary etc.
 * The parenthesys are not included.
 * 
 * */
namespace CalculatorXpress
{
    public class Compute
    {
        private double result;
        private int error;
        public Compute()
        {
            this.error = 0;
            this.result = 0;
        }
        public double GetResult()
        {
            return result;
        }
        public int GetError()
        {
            return error;
        }
        public void SetError(int err)
        {
            this.error = err;
            Form1.SetError(err);
            //display("Error: " + err);
        }

        public double FactorialFcn(double number)   // n! type 3 //Try: there is a factorial extension, Gamma function
        {
            double value = 1;
            int count = 0;
            if (number < 0)
            {
                SetError(8);
            }
            else
            {
                count = Convert.ToInt32(Math.Floor(number));
                do
                {
                    if (number > 1)
                    {
                        value *= number;
                        number--;
                    }
                    //else if (number < 1 && number > 0)//use the remaining fraction to smooth the curve
                    //{
                    //    //if(number > .5) value *= Math.Pow(number, 2) / (number - (1 -number));
                    //    //else value *= number / Math.Pow(1 - number, 2);
                    //}
                    count--;
                } while (count > -1);
            }


            return value;
        }
        public double PercentFcn(double number) // % type 3
        {
            return (number/100);
        }
        public double SquareFcn(double number)  // ² type 3
        {
            return Math.Pow(number, 2);
        }
        public double SquarerrotFcn(double number)  // √ type 4
        {
            double value = 0;
            if (number < 0) SetError(1);
            else value = Math.Sqrt(number);
            return value;
        }
        public double SinFcn(double number) // Sin √ type 4
        {
            double n = number / 180;
            bool rad = Form1.FlagStatus(0);
            double value = 0;
            if (number % 180 == 0 && !(n % 2 == 0)) value = 0;
            else if (!(rad)) value = Math.Sin(number * (Math.PI / 180 )); //Degrees
            else value = Math.Sin(number); //Radians
            //display("Value: " + value + " Nuber: " + number + " Rad?: " + rad);
            return value;
        }
        public double SinInvFcn(double number)  // Sinˉ¹ √ type 4
        {
            double value = 0;
            bool rad = Form1.FlagStatus(0);
            if (number > 1) SetError(5);
            else value = Math.Asin(number);
            if (!rad) value *= (180 / Math.PI);
            return value;
        }
        public double SinhFcn(double number)    // Sinh √ type 4 . Need verification of constraints, function domain, range, etc.
        {
            double value = 0;
            bool rad = Form1.FlagStatus(0);
            value = Math.Sinh(number * (Math.PI / 180));
            //display("Number :  " + number + "  Sinh(number) : " + value + "  Radians? : " + rad);
            if (!rad) value *= 180 / Math.PI;
            return value;
        }
        public double SinhInvFcn(double number) // Sinhˉ¹ √ type 4. Need verification of constraints, function domain, range, etc.
        {
            double value = 0;
            value = Math.Log (number + Math.Sqrt(number * number + 1));
            return value;
        }
        public double CosFcn(double number) // Cos √ type 4
        {
            double n = number / 90;
            bool rad = Form1.FlagStatus(0);
            double value = 0;
            if (number % 90 == 0 && !(n % 2 == 0) ) value = 0;
            else if (!(rad)) value = Math.Cos(number * (Math.PI / 180));//Degrees
            else value = Math.Cos(number);//Radians
            //display("Nuber: " + number + " Number % 90: " + number % 90 + " N % 2: " + n % 2 + " Value: "+ value);
            return value;
        }
        public double CosInvFcn(double number)  // Cosˉ¹ √ type 4
        {
            double value = 0;
            bool rad = Form1.FlagStatus(0);
            if (number > 1) SetError(5);
            else value = Math.Acos(number); //radians
            if (!rad) value *= (180 / Math.PI); //degrees
            //display("Number : " + number + " Value : " + value);
            return value;
        }
        public double CoshFcn(double number)    // Cosh √ type 4
        {
            double value = 0;
            bool rad = Form1.FlagStatus(0);
            value = Math.Cosh(number * (Math.PI / 180));
            if (!rad) value *= 180 / Math.PI;
            return value;
        }
        public double CoshInvFcn(double number) // Coshˉ¹ √ type 4
        {
            double value = 0;
            value = Math.Log(number + Math.Sqrt(number * number - 1));
            return value;
        }
        public double TanFcn(double number) // Ta∩ - alt 239 '∩' != 'n' √ type 4
        {
            double value = 0;
            bool rad = Form1.FlagStatus(0);
            double n = number / 90;
            if (number % 90 == 0 && n % 2 == 0 && !rad) value = 0; //Tangent of Sin n * 90(0) / Cos n * 90(1) = 0
            else if (number % 90 == 0 && !(n % 2 == 0)) SetError(6); //Tan of sin n * 90 (1) / cos 2 + n * 90 (0)=> div by 0 
            else if (!rad) value =  Math.Tan(number * (Math.PI / 180));
            else value = Math.Tan(number);
            return value;
        }
        public double TanInvFcn(double number)  // Ta∩ˉ¹ √ type 4
        {
            double value = 0;
            bool rad = Form1.FlagStatus(0);
            value = Math.Atan(number);
            if (!rad) value *= 180 / Math.PI;
            return value;
        }
        public double TanhFcn(double number)    // Ta∩h √ type 4
        {
            double value = 0;
            bool rad = Form1.FlagStatus(0);
            value = Math.Tanh(number * (Math.PI / 180));
            if (!rad) value *= 180 / Math.PI;
            return value;
        }
        public double TanhInvFcn(double number) // Ta∩hˉ¹ √ type 4
        {
            double value = 0;
            value = Math.Log((1 + number) / (1 - number)) / 2;
            return value;
        }
        public double Log10Fcn(double number)   // Log √ type 4
        {
            double value = 0;
            if (number == 0) SetError(14);
            else if (number < 0) SetError(3);
            else value = Math.Log10(number);
            return value;
        }
        public double Log10InvFcn(double number)    // Logˉ¹ √ type 4
        {
            double value = 0;
            value = Math.Pow(10, number);
            return value;
        }
        public double Log2Fcn(double number)    // ℓog₂ √ type 4
        {
            double value = 0;
            if (number == 0) SetError(14);
            else if (number < 0) SetError(3);
            else value = Math.Log10(number) / Math.Log10(2);
            return value;
        }
        public double Log2InvFcn(double number) // ℓog₂ˉ¹ √ type 4
        {
            double value = 0;
            value = Math.Pow(2, number) ;
            return value;
        }
        public double LognatFcn(double number)  // Ɩŋ √ type 4
        {
            double value = 0;
            if (number == 0) SetError(14);
            else if (number < 0) SetError(3);
            else value = Math.Log(number);
            return value;
        }
        public double EulerFcn(double number)   // eⁿ = Ɩŋˉ¹ √ type 4
        {
            return Math.Pow (Math.E, number);
        }
        public double PowerFcn(double LeftNumber, double RightNumber)   //  ^ type 15
        {
            double value = 0;
            if (RightNumber < 1 && RightNumber % 2 == 0 && LeftNumber < 0) SetError(2);
            else value = Math.Pow(LeftNumber, RightNumber);
            return value;
        }
        public double EEminusFcn(double LeftNumber, double RightNumber)   // E- type 15
        {
            double value = 0;
            value = LeftNumber * Math.Pow(10, RightNumber);
            return value;
        }
        public double EEplusFcn(double LeftNumber, double RightNumber)   // E- type 15
        {
            double value = 0;
            value = LeftNumber * 1 / (Math.Pow(10, RightNumber));
            return value;
        }
        public double ModFcn(double LeftNumber, double RightNumber)   // Mod  type 15
        {
            double value = 0;
            if (RightNumber == 0) SetError(7);
            else value = LeftNumber % RightNumber;
            return value;
        }
        public double AddFcn(double LeftNumber, double RightNumber)   // + type 2
        {
            double value = 0;
            value = LeftNumber + RightNumber;
            return value;
        }
        public double SubstractFcn(double LeftNumber, double RightNumber)   // - type 2
        {
            double value = 0;
            value = LeftNumber - RightNumber;
            return value;
        }
        public double MultiplyFcn(double LeftNumber, double RightNumber)   // * = x  type 2
        {
            double value = 0;
            value = LeftNumber * RightNumber;
            return value;
        }
        public double DivideFcn(double LeftNumber, double RightNumber)   // '/' type 2
        {
            double value = 0;
            if (RightNumber == 0) SetError(7);
            else value = LeftNumber / RightNumber;
            return value;
        }
        public double SumFcn(double LeftNumber, double RightNumber)   // Σx 
        {
            double value = 0;

            return value;
        }
        public double SumSqrFcn(double number)   // Σx² 
        {
            double value = 0;

            return value;
        }
        public double RegressionFcn(double number)   // Reg→
        {
            double value = 0;

            return value;
        }
        public double AverageFcn(double[] number)   // Reg→  Σ (alt 228)
        {
            int i;
            double value = 0;
            for (i = 0; i < number.Length; i++)
                value += number[i];
            return value / i;
        }
        public double PiFcn()   // π - PI type 9
        {
            return (Math.PI);
        }
        void display(String input)
        {
            System.Windows.Forms.MessageBox.Show(input);//End test
        }
    }
}
/*type 0 = nothing, 1 = numeric, 2 = binaryOp, 3 = leftOp, 4 = rightOp, 5 = dot, 6 = leftBrace, 7 = rightBrace,
 * 8 = result, 9 = Pi, 10 = invert, 11 = toggle, 12 = validMinus, 13 = reciprocal, 14 = error , 15 = Binary Functions*/
