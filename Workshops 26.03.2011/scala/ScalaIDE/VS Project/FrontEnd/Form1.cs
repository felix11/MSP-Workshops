using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;
using System.Threading;

namespace FrontEnd
{
    public partial class formScalaIDE : Form
    {
        private ScalaIDESettings settings = new ScalaIDESettings();
        private static StringBuilder sbOutput = new StringBuilder();

        ProcessStartInfo ProcessInfo;
        Process Process;

        public formScalaIDE()
        {
            InitializeComponent();

            loadSettings();
            loadCurrentFile();
        }

        /// <summary>
        /// Executes a command on the command line.
        /// From: Jay Miller, http://www.developer.com/net/csharp/article.php/3707996/NET-Tip-Execute-Commands-From-C.htm
        /// </summary>
        /// <param name="Command">the command to execute on cmd.exe</param>
        /// <param name="Timeout">timeout in msec</param>
        /// <returns>the exit code</returns>
        public int ExecuteCommand(string Command, int Timeout)
        {
            int ExitCode;

            sbOutput = new StringBuilder(textBoxOutput.Text);

            ProcessInfo = new ProcessStartInfo("cmd.exe", "/C " + Command);
            ProcessInfo.CreateNoWindow = true;
            ProcessInfo.UseShellExecute = false;
            ProcessInfo.RedirectStandardError = true;
            ProcessInfo.RedirectStandardOutput = true;
            ProcessInfo.WorkingDirectory = settings.workspaceFolder;
            Process = Process.Start(ProcessInfo);
            Process.BeginOutputReadLine();
            Process.BeginErrorReadLine();
            Process.OutputDataReceived += new DataReceivedEventHandler(Process_OutputDataReceived);
            Process.ErrorDataReceived += new DataReceivedEventHandler(Process_ErrorDataReceived);

            while (!Process.HasExited)
            {
                textBoxOutput.Text = sbOutput.ToString();
                textBoxOutput.Update();
                Thread.Sleep(50);
            }

            textBoxOutput.Text += Environment.NewLine + "program finished.";

            ExitCode = Process.ExitCode;
            Process.Close();

            return ExitCode;
        }


        /// <summary>
        /// Executes a command on the command line.
        /// From: Jay Miller, http://www.developer.com/net/csharp/article.php/3707996/NET-Tip-Execute-Commands-From-C.htm
        /// </summary>
        /// <param name="Command">the command to execute on cmd.exe</param>
        /// <param name="Timeout">timeout in msec</param>
        /// <returns>the exit code</returns>
        public int ExecuteCommandWithShell(string Command, int Timeout)
        {
            int ExitCode;
            ProcessStartInfo ProcessInfo;
            Process Process;

            ProcessInfo = new ProcessStartInfo("cmd.exe", "/C " + Command);
            ProcessInfo.CreateNoWindow = false;
            ProcessInfo.UseShellExecute = true;
            ProcessInfo.WorkingDirectory = settings.workspaceFolder;
            Process = Process.Start(ProcessInfo);

            Process.WaitForExit();

            ExitCode = Process.ExitCode;
            Process.Close();

            return ExitCode;
        }

        void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            sbOutput.Append(e.Data + Environment.NewLine);
        }

        void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            sbOutput.Append(e.Data + Environment.NewLine);
        }

        private void writeSettings()
        {
            Stream a = File.OpenWrite(ScalaIDESettings.path);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(a, settings);
            a.Close();
        }

        private void loadSettings()
        {
            if (!File.Exists(ScalaIDESettings.path)) writeSettings();

            FileStream file = new FileStream(ScalaIDESettings.path, FileMode.Open);

            BinaryFormatter bf = new BinaryFormatter();
            settings = bf.Deserialize(file) as ScalaIDESettings;

            file.Close();

            textBoxScalaFolder.Text = settings.scalaFolder;
            textBoxWorkspaceFolder.Text = settings.workspaceFolder;

            saveFileDialog1.InitialDirectory = settings.workspaceFolder;
            openFileDialog1.InitialDirectory = settings.workspaceFolder;
        }

        private bool verifyFolders()
        {
            if (Directory.Exists(settings.scalaFolder) &&
                Directory.Exists(settings.workspaceFolder) &&
                File.Exists(settings.scala) &&
                File.Exists(settings.scalac))
                return true;
            else
            {
                MessageBox.Show("Problem with scala path!", "Scala path error");
                return false;
            }
        }

        private bool verifyFile()
        {
            if (File.Exists(settings.currentFile))
                return true;
            else
            {
                MessageBox.Show("Current file not existing!", "File error");
                return false;
            }
        }

        private void compileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!verifyFile()) return;
            if (!verifyFolders()) return;

            saveToolStripMenuItem_Click(null, null);

            textBoxOutput.Text = "compiling...";

            string s = settings.scalac + " \"" + settings.currentFile + "\"";
            ExecuteCommand(s, 50000);
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!verifyFile()) return;
            if (!verifyFolders()) return;

            textBoxOutput.Text = "running...";

            string s = settings.scala + " App";
            ExecuteCommand(s, 50000);
        }

        private void buttonSelectScalaFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                settings.scalaFolder = folderBrowserDialog1.SelectedPath;
                settings.scala = settings.scalaFolder + "\\" + "scala.bat";
                settings.scalac = settings.scalaFolder + "\\" + "scalac.bat";

                textBoxScalaFolder.Text = settings.scalaFolder;
            }
        }

        private void buttonSelectWorkspaceFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                settings.workspaceFolder = folderBrowserDialog1.SelectedPath;

                textBoxWorkspaceFolder.Text = settings.workspaceFolder;
            }
        }

        private void formScalaIDE_Load(object sender, EventArgs e)
        {

        }

        private void formScalaIDE_FormClosing(object sender, FormClosingEventArgs e)
        {
            writeSettings();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!verifyFile())
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    settings.currentFile = saveFileDialog1.FileName;
                else
                    return;
            }

            File.Delete(settings.currentFile);
            StreamWriter tw = File.CreateText(settings.currentFile);
            tw.Write(textBoxFile.Text);
            tw.Flush();
            tw.Close();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                settings.currentFile = openFileDialog1.FileName;

                loadCurrentFile();
            }
        }

        private void loadCurrentFile()
        {
            if (!verifyFile()) return;

            FileStream file = new FileStream(settings.currentFile, FileMode.Open);
            StreamReader sr = new StreamReader(file);
            string filetext = sr.ReadToEnd();
            sr.Close();
            file.Close();

            if (filetext.Contains("\n") && !filetext.Contains("\r"))
                filetext = filetext.Replace("\n", Environment.NewLine);

            textBoxFile.Text = filetext;

            settings.workspaceFolder = settings.currentFile.Substring(0, settings.currentFile.LastIndexOf('\\'));
            textBoxWorkspaceFolder.Text = settings.workspaceFolder;
        }

        private void scalaBashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string scalaInteractive = settings.scala;
            ExecuteCommandWithShell(scalaInteractive, 0);
        }

        private void terminateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Process.HasExited)
                Process.Kill();
        }
    }

    [Serializable]
    public class ScalaIDESettings
    {
        public String scalaFolder = "";
        public String scala = "";
        public String scalac = "";
        public String workspaceFolder = "";
        public String currentFile = "";

        [NonSerialized]
        public static string path = "scalaIDE.settings";
    }
}
