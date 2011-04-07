using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using RecognitionLib;
using System.IO;

namespace InputForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Form initialisation: graphics
            bmp = new Bitmap(100, 100);
            using (Graphics bmpG = Graphics.FromImage(bmp))
            {
                bmpG.Clear(backgroundColor);
            }
            pictureBox1Graphics = pictureBox1.CreateGraphics();

            // REcognition library initialisation:
            handwritingRecObj = new Handwriting();

            // Input data manipulator
            dataManipulator = new DataManipulator();
        }

        private List<Point> lastPoints = new List<Point>();
        private bool isDrawing;
        private Graphics pictureBox1Graphics;
        private Bitmap bmp;

        private Handwriting handwritingRecObj;
        private DataManipulator dataManipulator;
        private string currentInputFile;
        private string currentInputChar;
        private int currentInputFileCounter;
        private Color backgroundColor = Color.Black;

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                lastPoints.Clear();
                isDrawing = true;
            }
            else
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    clearHandwritingArea();
                }
        }

        private void clearHandwritingArea()
        {
            bmp = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            using (Graphics bmpG = Graphics.FromImage(bmp))
            {
                bmpG.Clear(backgroundColor);
            }

            lastPoints.Clear();
            Graphics g = this.pictureBox1.CreateGraphics();
            g.Clear(backgroundColor);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                Pen pen = new Pen(Color.White, 10);
                
                lastPoints.Add(new Point(e.X, e.Y));
                if(lastPoints.Count > 10)
                    lastPoints.RemoveRange(0, lastPoints.Count-10);

                if (lastPoints.Count > 1)
                {
                    using (Graphics bmpG = Graphics.FromImage(bmp))
                    {
                        bmpG.DrawCurve(pen, lastPoints.ToArray());
                    }
                    pictureBox1Graphics.DrawImage(bmp, new Point(0,0));
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            lastPoints.Clear();
            isDrawing = false;

            bmp.Save("current.bmp", ImageFormat.Bmp);
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                dataManipulator.loadImages(openFileDialog1.FileNames);
                handwritingRecObj.loadTrainImages(dataManipulator.InputData);

                dataManipulator.ShowDialog();
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to clear the train data?", "Clear data", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                handwritingRecObj.clearTrainImages();
                dataManipulator.clearData();
            }
        }

        private void dataModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentFileChar cfc = new CurrentFileChar();
            cfc.ShowDialog();
            
            if (cfc.dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                currentInputChar = cfc.ResultChar;
                currentInputFileCounter = 0;

                currentInputFile = currentInputChar + currentInputFileCounter;
                actionButton.Text = ">>> save >>>";
                outputBox.Text = "File area";
                outputLabel.Text = currentInputFile;
            }
        }

        private void testModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionButton.Text = ">>> recognise >>>";
            outputBox.Text = "Recognition area";
            outputLabel.Text = "--";
        }

        private void actionButton_Click(object sender, EventArgs e)
        {
            // Data mode
            if (actionButton.Text.Contains("save"))
            {
                File.Copy("current.bmp", currentInputFile + ".bmp", true);

                clearHandwritingArea();
                currentInputFileCounter++;
                currentInputFile = currentInputChar + currentInputFileCounter;
                outputLabel.Text = currentInputFile;
            }
            // train mode
            else
            {
                string result = handwritingRecObj.recognise("current.bmp");
                outputLabel.Text = result;
            }
        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataManipulator.ShowDialog();
        }

        private void trainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            handwritingRecObj.train();
            MessageBox.Show("Network trained.");
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            handwritingRecObj.reset();
            MessageBox.Show("Network reset.");
        }
    }
}
