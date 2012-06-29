using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ZPASim;
using System.Collections.Concurrent;

namespace LoadFrontend
{
    public partial class Form1 : Form
    {
        private DateTime starttime;
        private ConcurrentQueue<bool> loaders = new ConcurrentQueue<bool>();

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonStartSimulation_Click(object sender, EventArgs e)
        {
            if (buttonStartSimulation.Text.StartsWith("Stop"))
            {
                buttonStartSimulation.Text = "Start Simulation";
                timer1.Stop();
            }
            else
            {
                buttonStartSimulation.Text = "Stop Simulation";

                Series s1 = chart1.Series.First(); // time
                Series s2 = chart2.Series.First(); // data
                Series s3 = chart3.Series.First(); // loader

                s1.Points.Clear();
                s2.Points.Clear();
                s3.Points.Clear();

                starttime = DateTime.Now;
                labelTime.Text = "0 msec";
                timer1.Tick += new EventHandler(timer1_Tick);
                timer1.Start();
                textBoxOutput.Text = "";

                Simulation sim = new Simulation();
                sim.start(Convert.ToInt32(numericUpDownStudentCount.Value), s1.Points, s2.Points, loaders);
            }
        }

        void timer1_Tick(object sender, EventArgs e)
        {
            Series s3 = chart3.Series.First(); // loader
            s3.Points.AddY(loaders.Count);

            labelTime.Text = (DateTime.Now - starttime).TotalMilliseconds.ToString() + " msec, Loader:" + loaders.Count.ToString();
        }
    }
}
