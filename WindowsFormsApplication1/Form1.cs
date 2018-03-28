using System;
using System.Collections;
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
    public partial class Form1 : Form
    {

        public static int state;
        public static List<Process> processes = new List<Process>();
        public static int Quantum;
        public static double AvgWT;
        public int counter=0;
        int btime = 0, min = 0, L = 1;
        int t1, t2;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label5.Visible = false; 
            textBox5.Visible = false;
            processes.Clear();            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)//table
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)//rr
        {
            label5.Visible = true;
            textBox5.Visible = true;
            state = 6;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)//fcfs
        {
            label5.Visible = false;
            textBox5.Visible = false;
            state = 1;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)//sjfp
        {
            label5.Visible = false;
            textBox5.Visible = false;
            state = 2;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)//sjfnp
        {
            label5.Visible = false;
            textBox5.Visible = false;
            state = 3;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)//pp
        {
            label5.Visible = false;
            textBox5.Visible = false;
            state = 4;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)//pnp
        {
            label5.Visible = false;
            textBox5.Visible = false;
            state = 5;
        }

        private void button1_Click(object sender, EventArgs e)// Add button
        {
            int n;
            dataGridView1.Rows.Clear();
            n = Convert.ToInt32(textBox1.Text);
            for (int i = 0; i < n; i++) {
                string[] data = { "P" + (i + 1).ToString() };
                dataGridView1.Rows.Add(data);
            }


        }

        private void button2_Click(object sender, EventArgs e)//Start button
        {
            /*Form2 Form2 = new Form2();
            Form2.Show();
            Hide();*/
            
            int n = Convert.ToInt32(textBox1.Text);
            Process[] processArray = new Process[n];

            for (int i = 0; i < n; i++)
            {
                string name = dataGridView1.Rows[i].Cells[0].Value.ToString();
                int arrivalTime = Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value.ToString());
                int burstTime = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                if (state == 4 || state == 5)
                {
                    int priority = Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value.ToString());
                    processArray[i] = new Process(name, arrivalTime, burstTime, priority);
                }
                else
                {
                    processArray[i] = new Process(name, arrivalTime, burstTime);
                }
                processes.Add(processArray[i]);
            }
            if(state==1)//first come first served
            {
                fcfs();
            }
            else if (state == 2) //shortest job first preemptive
            {
                sjfp();
            }
            else if (state == 3)//shortest job first non preemptive
            {
                sjfnp();
            }
            else if (state == 4)//priority preemptive
            {
                prp();
            }
            else if (state == 5)//priority non preemptive
            {
                prnp();
            }
            else if (state == 6)//round robin
            {
                Quantum = Convert.ToInt32(textBox5.Text);
                rr();
            }
            textBox2.Text = AvgWT.ToString();

        }
       


        /*--------------scheduling algorithms---------------*/

        public void fcfs() //first come first served
        {
            Process temp;
            for (int k = 0; k < processes.Count; k++)
            {
                for (int i = k + 1; i < processes.Count; i++)
                {
                    if (processes[k].arrivalTime > processes[i].arrivalTime || (processes[k].arrivalTime == processes[i].arrivalTime && processes[k].burstTime > processes[i].burstTime))
                    // sort by arrival time ascendengly if equal arrival time compare by the smaller burst time 
                    {
                        temp = processes[i];
                        processes[i] = processes[k];
                        processes[k] = temp;
                    }
                }
            }
            int clock = 0 /*current time*/, totalwait = 0;
            for (int i = 0; i < processes.Count; i++)
            {
                if (processes[i].arrivalTime > clock)  
                {
                    processes[i].start = processes[i].arrivalTime;
                    clock += processes[i].start - processes[i].arrivalTime;
                    clock += processes[i].burstTime;
                }
                else
                {
                    if (i > 0) 
                        processes[i].start = processes[i - 1].end;
                    clock += processes[i].burstTime;
                }
                if (processes[i].start > processes[i].arrivalTime)
                    processes[i].wait = processes[i].start - processes[i].arrivalTime;
                else processes[i].wait = 0; //if arrival time equals start time -> no waiting time
                processes[i].end = processes[i].start + processes[i].burstTime;
                totalwait += processes[i].wait;
            }
            AvgWT = (double)totalwait / (double)processes.Count;
            
        }
        
        public void sjfnp()//shortest job first non preemptive
        {
            Process temp;
            for (int k = 0; k <processes.Count; k++)
            {
                for (int i = k + 1; i < processes.Count; i++)
                {
                    if ((processes[k].arrivalTime > processes[i].arrivalTime) || (processes[k].arrivalTime == processes[i].arrivalTime && processes[k].burstTime > processes[i].burstTime))
                    {
                        temp = processes[i];
                        processes[i] = processes[k];
                        processes[k] = temp;
                    }
                }
            }
            for (int k = 0; k < processes.Count - 1; k++)
            {
                btime = btime + processes[k].burstTime;
                min = processes[L].burstTime;
                for (int i = k + 1; i < processes.Count; i++)
                {
                    if (btime >= processes[i].arrivalTime && processes[i].burstTime < min)
                    {
                        temp = processes[i];
                        processes[i] = processes[L];
                        processes[L] = temp;
                    }
                }
                L++;
            }


            int clock = 0, totalwait = 0;
            for (int i = 0; i < processes.Count; i++)
            {
                if (processes[i].arrivalTime > clock)
                {
                    processes[i].start = processes[i].arrivalTime;
                    clock += processes[i].start - processes[i].arrivalTime;
                    clock += processes[i].burstTime;
                }
                else
                {
                    if (i > 0)
                        processes[i].start = processes[i - 1].end;
                    clock += processes[i].burstTime;
                }
                if (processes[i].start > processes[i].arrivalTime)
                    processes[i].wait = processes[i].start - processes[i].arrivalTime;
                else processes[i].wait = 0;
                processes[i].end = processes[i].start + processes[i].burstTime;
                totalwait += processes[i].wait;
            }
            AvgWT = (double)totalwait / (double)processes.Count;
        }
        
        public void sjfp() //shortest job first preemptive
        {
            Process temp;
            for (int k = 0; k < processes.Count; k++)
            {
                for (int i = k + 1; i < processes.Count; i++)
                {
                    if (processes[k].arrivalTime >= processes[i].arrivalTime && processes[k].burstTime > processes[i].burstTime)
                    {
                        //sort by burst time ascendengly
                        temp = processes[i];
                        processes[i] = processes[k];
                        processes[k] = temp;
                    }
                }
            }
            int clock = 0, totalwait = 0;
            for (int i = 0; i < processes.Count; i++)
            {
                for (int j = 0; j < processes.Count; j++)
                {
                    Process temp2;
                    processes[i].remainTime = clock - processes[i].end;
                    if (processes[i].remainTime > processes[j].burstTime)
                    {
                        temp2 = processes[i];
                        processes[i] = processes[j];
                        processes[j] = temp2;
                    }
                    if (processes[i].arrivalTime > clock)
                    {
                        processes[i].start = processes[i].arrivalTime;
                        clock += processes[i].start - processes[i].arrivalTime;
                        processes[i].end = clock + processes[i].burstTime;
                        clock += processes[i].burstTime;
                    }
                    else
                    {
                        if (i > 0)
                            processes[i].start = processes[i - 1].end;
                        clock += processes[i].burstTime;
                    }
                    if (processes[i].start > processes[i].arrivalTime)
                        processes[i].wait = processes[i].start - processes[i].arrivalTime;
                    else processes[i].wait = 0;
                    totalwait += processes[i].wait;
                }
            }
            AvgWT = (double)totalwait / (double)processes.Count;
        }
        
        public void prnp() //priority non preemptive
        {
            Process temp;
            for (int k = 0; k < processes.Count; k++)
            {
                for (int i = k + 1; i < processes.Count; i++)
                {
                    //sort by priority ascendengly
                    if ((processes[k].priority > processes[i].priority) && (processes[k].arrivalTime >= processes[i].arrivalTime))
                    {
                        temp = processes[i];
                        processes[i] = processes[k];
                        processes[k] = temp;
                    }
                }
            }
            for (int k = 0; k < processes.Count - 1; k++)
            {
                btime = btime + processes[k].priority;
                min = processes[L].priority;
                for (int i = k + 1; i < processes.Count; i++)
                {
                    if (btime >= processes[i].arrivalTime && processes[i].priority < min)
                    {
                        temp = processes[i];
                        processes[i] = processes[L];
                        processes[L] = temp;
                    }
                }
                L++;
            }
            int clock = 0, totalwait = 0;
            for (int i = 0; i < processes.Count; i++)
            {
                if (processes[i].arrivalTime > clock)
                {
                    processes[i].start = processes[i].arrivalTime;
                    clock += processes[i].start - processes[i].arrivalTime;
                    clock += processes[i].burstTime;
                }
                else
                {
                    if (i > 0)
                        processes[i].start = processes[i - 1].end;
                    clock += processes[i].burstTime;
                }
                if (processes[i].start > processes[i].arrivalTime)
                    processes[i].wait = processes[i].start - processes[i].arrivalTime;
                else processes[i].wait = 0;
                processes[i].end = processes[i].start + processes[i].burstTime;
                totalwait += processes[i].wait;
            }
            AvgWT = (double)totalwait / (double)processes.Count;
        }
        
        public void prp() //priority preemptive
        {
            Process temp;
            for (int k = 0; k < processes.Count; k++)
            {
                for (int i = k + 1; i < processes.Count; i++)
                {
                    if (processes[k].priority > processes[i].priority)
                    {
                        temp = processes[i];
                        processes[i] = processes[k];
                        processes[k] = temp;
                    }
                }
            }
            int clock = 0, totalwait = 0;
            for (int i = 0; i < processes.Count; i++)
            {
                for (int j = 0; j < processes.Count; j++)
                {
                    Process temp2;
                    processes[i].remainTime = clock - processes[i].end;
                    if (processes[i].remainTime > processes[j].burstTime)
                    {
                        temp2 = processes[i];
                        processes[i] = processes[j];
                        processes[j] = temp2;
                    }
                    if (processes[i].arrivalTime > clock)
                    {
                        processes[i].start = processes[i].arrivalTime;
                        clock += processes[i].start - processes[i].arrivalTime;
                        processes[i].end = clock + processes[i].burstTime;
                        clock += processes[i].burstTime;
                    }
                    else
                    {
                        if (i > 0)
                            processes[i].start = processes[i - 1].end;
                        clock += processes[i].burstTime;
                    }
                    if (processes[i].start > processes[i].arrivalTime)
                        processes[i].wait = processes[i].start - processes[i].arrivalTime;
                    else processes[i].wait = 0;
                    processes[i].end = processes[i].start + processes[i].burstTime;
                    totalwait += processes[i].wait;
                }
            }
            AvgWT = (double)totalwait / (double)processes.Count;
        }
        
        public void rr() //round robin
        {
            Queue<Process> q = new Queue<Process>();
            for (int i = 0; i < processes.Count; i++) q.Enqueue(processes[i]);

            Process temp;
            for (int k = 0; k < processes.Count; k++)
            {
                for (int i = k + 1; i < processes.Count; i++)
                {
                    if (processes[k].arrivalTime > processes[i].arrivalTime || (processes[k].arrivalTime == processes[i].arrivalTime && processes[k].burstTime > processes[i].burstTime))
                    {
                        temp = processes[i];
                        processes[i] = processes[k];
                        processes[k] = temp;
                    }
                }
            }
            bool fin = false;
            for (int i = 0; i < processes.Count; i++) processes[i].remainTime = processes[i].burstTime;
        rep:
            for (int i = 0; i < processes.Count; i++)
            {
                if (processes[i].remainTime == 0) fin = true;
                if (processes[i].remainTime > 0)
                {
                    fin = false;
                    Process pq = q.Dequeue();
                    for (int count = Quantum; count >= 0; count--) processes[i].remainTime--;
                    q.Enqueue(pq);
                    processes[i].wait += Quantum;
                }
            }
            if (fin == false) goto rep;
            int totalwait = 0;
            for (int i = 0; i < processes.Count; i++) totalwait += processes[i].wait;
            AvgWT = (double)totalwait / (double)processes.Count;
        }

        private void chart1_Click(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)//Reset button
        {
            textBox1.Text = string.Empty;
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            dataGridView1.Rows.Clear();
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            radioButton5.Checked = false;
            radioButton6.Checked = false;
            label5.Visible = false;
            textBox5.Visible = false;
            textBox5.Text = string.Empty;
            AvgWT = 0;
            processes.Clear();            
        }

       
       
    }

    public class Process
    {

        public string name;
        public int arrivalTime;
        public int burstTime;
        public int remainTime;
        public int priority;
        public int wait;
        public int end;
        public int start;
        public Process(string name, int arrivalTime, int burstTime, int priority) //constructor
        {
            this.name = name;
            this.arrivalTime = arrivalTime;
            this.burstTime = burstTime;
            this.priority = priority;
        }
        public Process(string name, int arrivalTime, int burstTime) //constructor
        {
            this.name = name;
            this.arrivalTime = arrivalTime;
            this.burstTime = burstTime;
        }
        
        
    }


}

