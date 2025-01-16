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
            SetSpeed();
        }

        private void SetSpeed()
        {
            double.TryParse(textBox1.Text, out var speed);
            if (speed < 1) speed = 1;
            var tmp = speed.ToString();
            File.WriteAllText(@"file\dll\speed\x64\speed.txt", tmp);
            MainClass.SetGameSpeed(speed);
            textBox1.Text = tmp;
        }

        private void FormChangeSpeed_Load(object sender, EventArgs e)
        {
            try
            {
                textBox1.Text = File.ReadAllText(@"file\dll\speed\x64\speed.txt");
            }
            catch
            {
                textBox1.Text = "1.0";
            }
            SetSpeed();
        }
    }
}
