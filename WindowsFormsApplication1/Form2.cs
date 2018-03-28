using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int arrivalTime = Convert.ToInt32(textBox3.Text.ToString());
            int burstTime = Convert.ToInt32(textBox2.Text.ToString());
            Process process;
            if (Form1.state == 4 || Form1.state == 5)
            {
                int Priority = Convert.ToInt32(textBox1.Text.ToString());
                process = new Process("name", arrivalTime, burstTime, Priority);
            }
            else
                process = new Process("name", arrivalTime, burstTime);
            Form1.processes.Add(process);
            var principalForm = Application.OpenForms.OfType<Form1>().Single();
            if (Form1.state == 1)//first come first served
                principalForm.fcfs();
            else if (Form1.state == 2) //shortest job first preemptive
                principalForm.sjfp();
            else if (Form1.state == 3)//shortest job first non preemptive
                principalForm.sjfnp();
            else if (Form1.state == 4)//priority preemptive
                principalForm.prp();
            else if (Form1.state == 5)//priority non preemptive
                principalForm.prnp();
            else if (Form1.state == 6)//round robin
                principalForm.rr();
            textBox4.Text = Form1.AvgWT.ToString();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 restart = new Form1();
            restart.Show();
            Hide();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            textBox4.Text = Form1.AvgWT.ToString();
        }
    }

}
