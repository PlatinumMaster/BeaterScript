using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using BeaterScriptEngine;

namespace ScriptEditor
{
    public partial class Form1 : Form
    {
        ScriptParser parser;

        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            FolderBrowserDialog browserDialog = new FolderBrowserDialog();
            fileDialog.Filter = "Generation V Script | *.bin";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                var lexer = new ScriptLexer(fileDialog.FileName, "B2W2");
                lexer.WriteScript(parser.Scripts, parser.Functions, parser.Movements);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Generation V Script | *.bin";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                parser = new ScriptParser(fileDialog.FileName, "B2W2");

                for (int i = 0; i < parser.Addresses.Count; i++)
                    toolStripComboBox1.Items.Add(i);

                for (int i = 0; i < parser.Functions.Count; i++)
                    toolStripComboBox2.Items.Add(i);

                for (int i = 0; i < parser.Movements.Count; i++)
                    toolStripComboBox3.Items.Add(i);

                toolStripComboBox1.SelectedIndex = 
                    toolStripComboBox2.SelectedIndex = 
                    toolStripComboBox3.SelectedIndex = 0;
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

            foreach (Command c in parser.Scripts[toolStripComboBox1.SelectedIndex].GetScript())
                textBox1.Text += String.Format("{0}{1}", c.ToString(), Environment.NewLine);
        }

        private void toolStripComboBox2_SelectionChanged(object sender, EventArgs e)
        {
            textBox2.Text = "";
            if (toolStripComboBox2.SelectedIndex > parser.Functions.Count())
            {
                toolStripComboBox2.SelectedIndex = parser.Functions.Count();
                return;
            }

            foreach (Command c in parser.Functions[toolStripComboBox2.SelectedIndex].GetScript())
                textBox2.Text += String.Format("{0}{1}", c.ToString(), Environment.NewLine);
        }

        private void toolStripComboBox3_SelectionChanged(object sender, EventArgs e)
        {
            textBox3.Text = "";
            if (toolStripComboBox3.SelectedIndex > parser.Movements.Count())
            {
                toolStripComboBox3.SelectedIndex = parser.Movements.Count();
                return;
            }

            foreach (Movement m in parser.Movements[toolStripComboBox3.SelectedIndex])
                textBox3.Text += String.Format("{0}{1}", m.ToString(), Environment.NewLine);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.AutoSize = true;
            textBox2.AutoSize = true;
            textBox3.AutoSize = true;
            tabPage1.Text = "Scripts";
            tabPage2.Text = "Functions";
            tabPage3.Text = "Movements";
        }
    }
}
