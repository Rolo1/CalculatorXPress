using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculatorXpress
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
        public void Paint( PaintEventArgs e)
        {
            Pen blackpen = new Pen(Color.Black, 4);
            Point A = new Point(20, 20);
            Point B = new Point(46, 52);
            e.Graphics.DrawLine(blackpen, A, B);
        }

        private void button1_Click(object sender, EventArgs h)
        {
            using (var g = Graphics.FromImage(pictureBox1.Image))//an inner class?
            {
                Pen blackpen = new Pen(Color.Black, 4);
                Point A = new Point(20, 20);
                Point B = new Point(46, 52);
                g.DrawLine(blackpen, A, B); //(Pens.Blue, 10, 10, 100, 100);
                pictureBox1.Refresh();
            }
        }
    }
}
