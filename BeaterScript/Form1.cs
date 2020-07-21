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
                var currentScript = new Script();
                var currentFunction = new Script();
                var cmds = new CommandsListHandler("B2W2");

                // Messy :(

                // Save the current script
                foreach (string line in textBox1.Text.Split('\n'))
                {
                    ushort idx;
                    try
                    {
                        idx = cmds.command_map[line.Substring(0, line.IndexOf("("))];
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        continue;
                    }

                    // This means we actually have parameters.
                    if (line.IndexOf(")") - (line.IndexOf("(") + 1) == 0) 
                        continue;

                    var parameters = line.Substring(line.IndexOf("(") + 1, line.IndexOf(")") - (line.IndexOf("(") + 1));
                    int i = 0;

                    List<object> params_hex = new List<object>();

                    foreach (var num in parameters.Replace(" ", "").Split(','))
                    {
                        if (cmds.commands[idx].Types[i] == typeof(uint))
                            params_hex.Add(uint.Parse(num));
                        else if (cmds.commands[idx].Types[i] == typeof(ushort))
                            params_hex.Add(ushort.Parse(num));
                        else if (cmds.commands[idx].Types[i] == typeof(byte))
                            params_hex.Add(byte.Parse(num));
                        i++;
                    }
                    bool isMovement = line.Substring(0, line.IndexOf("(")).Equals("ApplyMovement");
                    bool isFunction = line.Substring(0, line.IndexOf("(")).Equals("ConditionalJump") 
                        || line.Substring(0, line.IndexOf("(")).Equals("UnconditionalJump")
                        || line.Substring(0, line.IndexOf("(")).Equals("CallRoutine");

                    currentScript.Add(new Command(line.Substring(0, line.IndexOf("(")), isFunction, isMovement, params_hex.ToArray(), cmds.commands[idx].Types));
                }

                // Save the current function
                foreach (string line in textBox2.Text.Split('\n'))
                {
                    ushort idx;
                    try
                    {
                        idx = cmds.command_map[line.Substring(0, line.IndexOf("("))];
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        continue;
                    }
                    
                    // Do a check to see if we have parameters. If this check is true, we do not have parameters.
                    if (line.IndexOf(")") - (line.IndexOf("(") + 1) == 0) 
                        continue;

                    var parameters = line.Substring(line.IndexOf("(") + 1, line.IndexOf(")") - (line.IndexOf("(") + 1));
                    int i = 0;

                    List<object> params_hex = new List<object>();

                    foreach (var num in parameters.Replace(" ", "").Split(','))
                    {
                        if (cmds.commands[idx].Types[i] == typeof(uint))
                            params_hex.Add(uint.Parse(num));
                        else if (cmds.commands[idx].Types[i] == typeof(ushort))
                            params_hex.Add(ushort.Parse(num));
                        else if (cmds.commands[idx].Types[i] == typeof(byte))
                            params_hex.Add(byte.Parse(num));
                        i++;
                    }
                    bool isMovement = line.Substring(0, line.IndexOf("(")).Equals("ApplyMovement");
                    bool isFunction = line.Substring(0, line.IndexOf("(")).Equals("ConditionalJump") 
                        || line.Substring(0, line.IndexOf("(")).Equals("UnconditionalJump")
                        || line.Substring(0, line.IndexOf("(")).Equals("CallRoutine");

                    currentFunction.Add(new Command(line.Substring(0, line.IndexOf("(")), isFunction, isMovement, params_hex.ToArray(), cmds.commands[idx].Types));
                }

                parser.Scripts[toolStripComboBox1.SelectedIndex] = currentScript;
                parser.Functions[toolStripComboBox2.SelectedIndex] = currentFunction;

                // Don't touch, I'm sterile.
                var lexer = new ScriptLexer("test.bin", "B2W2");
                lexer.WriteScript(parser.Scripts, parser.Functions, new List<List<Movement>>(), "test.bin");
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

            foreach (Command c in parser.Scripts[toolStripComboBox1.SelectedIndex].getScript())
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

            foreach (Command c in parser.Functions[toolStripComboBox2.SelectedIndex].getScript())
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
