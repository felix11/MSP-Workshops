using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ZPA;
using System.Collections.Concurrent;

namespace GUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            timer1.Tick += new EventHandler(timer1_Tick);
        }

        private ConcurrentQueue<bool> loaders = new ConcurrentQueue<bool>();

        private void buttonStartSimulation_Click(object sender, EventArgs e)
        {
            Series s1 = chart1.Series.First();

            loaders = new ConcurrentQueue<bool>();
            s1.Points.Clear();

            SimZPA.Simulation sim = new SimZPA.Simulation();

            timer1.Interval = 100;
            timer1.Start();
            
            sim.start((int)numericUpDownStudents.Value, loaders, textBoxOutput);
        }

        void timer1_Tick(object sender, EventArgs e)
        {
            Series s1 = chart1.Series.First();
            s1.Points.AddY(loaders.Count);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }
    }
}
