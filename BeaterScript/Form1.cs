using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BeaterScriptEngine;

namespace ScriptEditor
{
    public partial class Form1 : Form
    {
        BeaterScriptEngine.ScriptParser parser;
        string script_name;

        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            FolderBrowserDialog browserDialog = new FolderBrowserDialog();
            fileDialog.Filter = "Generation 5 Script | (*.bin)";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                // TODO: Implement rewriting.
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Generation V Script | *.bin";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                parser = new ScriptParser(fileDialog.FileName, "B2W2");
                parser.FindScriptAddresses();

                for (int i = 0; i < parser.Addresses.Count; i++)
                    toolStripComboBox1.Items.Add(i);

                List<BeaterScriptEngine.Command> script = parser.ReadScript(parser.Addresses[0]);
                for (int i = 0; i < script.Count; i++)
                    textBox1.Text += String.Format("{0}{1}", script[i].ToString(), Environment.NewLine);
            }
        }

        private void toolStripComboBox1_SelectionChanged(object sender, EventArgs e)
        {
            textBox1.Text = "";
             
            if (toolStripComboBox1.SelectedIndex > parser.Addresses.Count())
            {
                toolStripComboBox1.SelectedIndex = parser.Addresses.Count();
                return;
            }

            List<BeaterScriptEngine.Command> script = parser.ReadScript(parser.Addresses[toolStripComboBox1.SelectedIndex]);
            for (int i = 0; i < script.Count; i++)
                textBox1.Text += String.Format("{0}{1}", script[i].ToString(), Environment.NewLine);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
