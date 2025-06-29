using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorXpress
{
    public class NumberBase
    {
        public string buffer;
        public NumberBase() { buffer = ""; }
        public NumberBase(string buff) { SetBuffer(buff); }
        public void SetBuffer(string bf) { this.buffer = bf; }
        public string GetBuff() { return buffer; }

        public double BinToTen(string binStr)
        {
            double base10 = 0;
            double bin = 0;
            string binary = "";

            for (int n = 0; n < binStr.Length; n++)
            {
                if (binStr[n] == ' ') continue;
                else binary += binStr[n];
            }

            int power = binary.Length - 1;
            //display("binary : " + binary + "  Length :  " + binary.Length + "  Power : " + power);

            for (int i = 0; i < binary.Length; i++)
            {
                bin = binary[i] - '0';
                base10 += bin * Math.Pow(2, power);
                //display("bin : " + bin + "  Power : " + power + "   Base10 : " + base10);
                power--;
            }
            return base10;
        }
        public string BinToOctal(string binStr)
        {
            string base8 = "";
            string oct = "";
            int power = 0;
            double number = 0;
            double oc = 0;
            double octal = 0;

            for (int i = binStr.Length - 1; i >= 0; i--) //0100 = 000 100oct; 1011 => 1101 : 1 x 2 ^0 + ...reversed string
            {
                if (binStr[i] == ' ') continue;
                else oct += binStr[i];
            }
            //display("Oct : " + oct);
            for (int j = 0; j < oct.Length; j++)
            {
                number = oct[j] - '0';
                oc += number * Math.Pow(2, power);
                power++;

                if ((j + 1) % 3 == 0 || j == oct.Length - 1)
                {
                    if (oc > 7) base8 += (oc % 8 + 1).ToString();//octal += 9 + (oc % 8 + 1);   
                    base8 += oc;                                      
                    oc = 0;
                    power = 0;
                }
            }
            string temp = "";
            for (int k = base8.Length - 1; k >= 0; k--) temp += base8[k];
            base8 = temp;
            octal = double.Parse(base8);
            base8 = octal.ToString();
            return base8;
        }
        public string BinToHex(string binStr) //Needs to add subsequent sets of 4 binary digits
        {
            int power = 3;// binStr .Length - (binStr.Length % 4) - 1;
            string base16 = "";
            string hex = "";
            double hx = 0;
            int j = 0;

                hex = ""; //clear old value

            for (j = 0; j < binStr.Length; j++) //(j = i - 1; j < i + 3; j++)
            {
                if (binStr[j] == ' ') continue;
                else hex += binStr[j];//get all bits
            }
            int format = 4 - hex.Length % 4;
            if(!(format == 0))
            {
                string temp = "";
                //display("Format : " + format + "   Length : " + hex.Length);
                while (!(format == 0))
                {
                    temp += '0';
                    format--;
                }
                temp += hex;
                hex = temp;
            }

            //display("binary string : " + hex + "  format :  " + format ); //eg. 0100 1011 = 75d = 0x4B
            double number = 0;

            for (int n = 0; n < hex.Length; n++)
            {
                number = hex[n] - '0';
                //display("Number : " + number + "  power : " + power + "  n % 4 :  " + (n + 1) % 4);
                hx += number * Math.Pow(2, power);
                power--;
                if ((n + 1) % 4 == 0)
                {
                    base16 += GetHex(hx);
                    //display("GetHex : " + base16 + "  Number :  " + hx);
                    hx = 0;
                    power = 3;
                }
            }
            //display("Number :  " + hx + "  Hex : " + hex + "  Base16 : " + base16 );
            //display("Base 16 : " + base16) ;
            return base16;
        }
        public string TenToBin(double ten)// XXXX ???? ???? ????
        {
            string bin = "";
            double value = 0;
            double temp = 0;
            int size = ten.ToString().Length; // Convert.ToInt32( Math.Pow(2, ten.ToString().Length) + 1);
            //while (Math.Pow(2, size) > ten) size--;
            //display("Size : " + size + " value : " + value);
            for (int i = (size * 4) - 1; i > -1; i--)
            {
                temp = Math.Pow(2, i);
                //display("Value : " + value + "   bin :  " + bin + "    i : " + i + "    temp :  " + temp);
                if (value + temp <= ten)
                {
                    value += temp;
                    bin += '1';
                }
                else bin += '0';
                
                if (i % 4 == 0) bin += " ";
            }

            return bin;
        }
        public string OcToBin(string octal) // ????????? Convert to binary
        {
            string bin = "";
            double power = 2;
            double number = 0;
            double value = 0;

            int size = octal.Length;  //0 1 2 3 4 5 6 7 10 11 12 13 14 15 20
            //                                      
            for (int i = 0; i < size; i++) //001 000 = 8 = octal
            {
                number = octal[i] - '0';

                for (int j = 0; j < 3; j++)
                {

                    if ((Math.Pow(2, power) + value) <= number)
                    {
                        //display("2 ^ power + value : " + (Math.Pow(2, power) + value) + "  value : " + value + "  bin :  " + bin);
                        value += Math.Pow(2, power);
                        bin += '1';
                    }

                    else bin += '0';
                    power--;
                }//end for j
                bin += " ";
                power = 2;
                value = 0;
            }//end for i

            return bin;
        }
        public string HexToBin(string hex)
        {
            string bin = "";
            List<string> hexed = new List<string>();
            string numbers = "0123456789";
            char digit = '\0';
            //display("Hex : " + hex);
            for (int n = 0; n < hex.Length ; n++)
            {
                digit = hex[n];
                if (!(numbers.Contains(digit)))
                {
                    switch (digit) //Values are reversed
                    {
                        case 'A':
                            hexed.Add( "1010 ");   //"0101"; //
                            break;
                        case 'B':
                            hexed.Add("1011 ");   //"1101";  //
                            break;
                        case 'C':
                            hexed.Add("1100 "); //"0011"; //
                            break;
                        case 'D':
                            hexed.Add("1101 ");   //"1011"; //
                            break;
                        case 'E':
                            hexed.Add("1110 ");   //"0111"; //
                            break;
                        case 'F':
                            hexed.Add("1111 "); //"1111";
                            break;
                        default: break;
                    }
                }
                else
                {
                    double number = digit - '0';
                    hexed.Add(TenToBin(number));
                }
            }//end for
            for (int j = 0; j < hexed.Count; j++) bin += hexed.ElementAt(j);
            //display(" bin : " + bin + ":");
            return bin;
        }

    string GetHex(double number)
        {
            string hex = "";
            if (number < 10) hex = number.ToString();
            else if (number < 16)
            {
                switch (number)
                {
                    case 10:
                        hex = "A";
                        break;
                    case 11:
                        hex = "B";
                        break;
                    case 12:
                        hex = "C";
                        break;
                    case 13:
                        hex = "D";
                        break;
                    case 14:
                        hex = "E";
                        break;
                    case 15:
                        hex = "F";
                        break;
                    default:
                        break;
                }
            }
            else hex = (number % 16 + 1).ToString();
            return hex;
        }
        void display(string message)
        {
            System.Windows.Forms.MessageBox.Show(message);
        }
    }
}
