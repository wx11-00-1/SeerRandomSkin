using System;
using System.IO;
using System.Windows.Forms;

namespace SpeedhackWrapper
{
    public partial class FormChangeSpeed : Form
    {
        public FormChangeSpeed()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double.TryParse(textBox1.Text, out var speed);
            if (speed < 1) speed = 1;
            File.WriteAllText(@"file\dll\speedhack\x64\speedhack.txt", speed.ToString());
            MainClass.ResetSpeed();
        }

        private void FormChangeSpeed_Load(object sender, EventArgs e)
        {
            try
            {
                textBox1.Text = File.ReadAllText(@"file\dll\speedhack\x64\speedhack.txt");
            }
            catch
            {
                textBox1.Text = "1.0";
            }
        }
    }
}
