﻿namespace GUI
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.buttonStartSimulation = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.numericUpDownStudents = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxOutput = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStudents)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonStartSimulation
            // 
            this.buttonStartSimulation.Location = new System.Drawing.Point(534, 369);
            this.buttonStartSimulation.Name = "buttonStartSimulation";
            this.buttonStartSimulation.Size = new System.Drawing.Size(158, 84);
            this.buttonStartSimulation.TabIndex = 0;
            this.buttonStartSimulation.Text = "Start Simulation";
            this.buttonStartSimulation.UseVisualStyleBackColor = true;
            this.buttonStartSimulation.Click += new System.EventHandler(this.buttonStartSimulation_Click);
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(12, 12);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(454, 306);
            this.chart1.TabIndex = 1;
            this.chart1.Text = "chart1";
            // 
            // numericUpDownStudents
            // 
            this.numericUpDownStudents.Location = new System.Drawing.Point(230, 349);
            this.numericUpDownStudents.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownStudents.Name = "numericUpDownStudents";
            this.numericUpDownStudents.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownStudents.TabIndex = 2;
            this.numericUpDownStudents.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(129, 351);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Numer of Students";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(391, 398);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Stop";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.Location = new System.Drawing.Point(491, 32);
            this.textBoxOutput.Multiline = true;
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.Size = new System.Drawing.Size(201, 286);
            this.textBoxOutput.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 476);
            this.Controls.Add(this.textBoxOutput);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDownStudents);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.buttonStartSimulation);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStudents)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonStartSimulation;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.NumericUpDown numericUpDownStudents;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBoxOutput;
    }
}

