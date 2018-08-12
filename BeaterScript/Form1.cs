using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gen5Scripts;

namespace ScriptEditor
{
    public partial class Form1 : Form
    {
        ScriptHandler s = new ScriptHandler();
        string script_name;

        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            FolderBrowserDialog browserDialog = new FolderBrowserDialog();
            fileDialog.Filter += ".bin|Generation 5 Script";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                script_name = browserDialog.SelectedPath + fileDialog.FileName;
                List<string> newScript = textBox1.Text.Split(Environment.NewLine.ToCharArray()).ToList();
                var emptycount = 0;

                for (int i = 0; i < newScript.Count; i++)
                    if (newScript[i] == "")
                        emptycount++;

                for (int i = 0; i <= emptycount; i++)
                    newScript.Remove("");

                for (int i = 0; i < newScript.Count; i++)
                    Console.WriteLine(newScript[i]);

                s.WriteCommands(script_name, newScript);
                s.BringItHome(script_name);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter += ".bin|Generation 5 Script";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                script_name = fileDialog.FileName;
                s.LoadScript(script_name);
                s.FetchPointers(script_name);
                s.FetchCommands(0, script_name);

                for (int i = 0; i < s.pointers.Count; i++)
                    toolStripComboBox1.Items.Add(s.pointers[i].ToString());

                for (int i = 0; i < s.ScriptData.Count; i++)
                    textBox1.Text += s.ScriptData[i] + Environment.NewLine;
            }
        }

        private void toolStripComboBox1_SelectionChanged(object sender, EventArgs e)
        {
            s.FetchCommands(0, script_name);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
