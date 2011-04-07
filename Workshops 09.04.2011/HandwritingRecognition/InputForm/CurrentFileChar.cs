using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InputForm
{
    public partial class CurrentFileChar : Form
    {
        public CurrentFileChar()
        {
            InitializeComponent();
        }

        public DialogResult dialogResult = DialogResult.Cancel;
        private string resultChar = "";

        public string ResultChar
        {
            get
            {
                if (dialogResult != System.Windows.Forms.DialogResult.OK)
                {
                    throw new DataException("The current train character cannot be read.");
                }
                else
                    return resultChar;
            }
            private set { resultChar = value; }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            resultChar = trainCharacterBox.Text;
            dialogResult = System.Windows.Forms.DialogResult.OK;

            this.Close();
        }
    }
}
