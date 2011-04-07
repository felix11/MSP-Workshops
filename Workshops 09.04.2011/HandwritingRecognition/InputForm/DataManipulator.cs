using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RecognitionLib;
using System.IO;

namespace InputForm
{
    public partial class DataManipulator : Form
    {
        public DataManipulator()
        {
            InitializeComponent();
        }

        internal void loadImages(string[] p)
        {
            InputData = new List<StringInputData>();
            foreach (string image in p)
            {
                StringInputData id = new StringInputData(image, Path.GetFileName(image)[0].ToString());
                InputData.Add(id);
                inputDataBindingSource.Add(id);
            }
        }

        public List<StringInputData> InputData{ get; private set; }

        internal void clearData()
        {
            InputData = new List<StringInputData>();
            inputDataBindingSource.Clear();
        }
    }
}
