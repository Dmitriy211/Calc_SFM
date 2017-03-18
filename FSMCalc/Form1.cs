using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FSMCalc
{
    public partial class Form1 : Form
    {
        States s = new States();

        void ChangeTextBox(string text)
        {
            textBox1.Text = text;
        }
        public Form1()
        {
            InitializeComponent();
            s.invoker = ChangeTextBox;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            char item = btn.Text[0];
            int a = ((int)item % 48) * 32 + 700;
            if (btn.Text[0] >= 48 && btn.Text[0] <= 57)
                Console.Beep(a, 100);
            else
                Console.Beep();
            s.Process(item);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }        
    }
}
