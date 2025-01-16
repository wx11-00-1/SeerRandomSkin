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
            var tmp = speed.ToString();
            File.WriteAllText(MainClass.SPEED_CONFIG_FILE, tmp);
            MainClass.SetGameSpeed(speed);
            textBox1.Text = tmp;
        }

        private void FormChangeSpeed_Load(object sender, EventArgs e)
        {
            try
            {
                textBox1.Text = File.ReadAllText(MainClass.SPEED_CONFIG_FILE);
            }
            catch
            {
                textBox1.Text = "1.0";
            }
        }
    }
}
