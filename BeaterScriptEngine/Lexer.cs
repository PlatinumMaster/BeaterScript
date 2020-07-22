using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Runtime.Remoting.Messaging;
using System.Runtime.CompilerServices;
using System.Collections;

namespace BeaterScriptEngine
{
    public class ScriptLexer
    {
        BinaryWriter b;
        List<uint> pointers { get; set; }
        CommandsListHandler cmds;

        public ScriptLexer(string script, string game)
        {
            // Initialize the script we will write to.
            this.b = new BinaryWriter(File.Open(script, FileMode.OpenOrCreate));
            this.cmds = new CommandsListHandler(game);
        }

        public void WriteScript(List<Script> scripts, List<Script> functions, List<List<Movement>> movements)
        {
            uint location = (uint)(scripts.Count * 0x4) + 2; // Size of header section.
            Dictionary<string, uint> script_map = new Dictionary<string, uint>();

            // For absolutely no reason, I will place all functions first. Problem?
            foreach (Script s in functions)
            {
                foreach (byte[] cmd_hex in s.GetScriptBytes())
                    b.Write(cmd_hex);
                script_map.Add($"Function{functions.IndexOf(s)}", location);
                location += s.GetScriptSize();
            }

            // Okay now we have scripts.
            foreach (Script s in scripts)
            {
                foreach (byte[] cmd_hex in s.GetScriptBytes())
                    b.Write(cmd_hex);
                script_map.Add($"Script{scripts.IndexOf(s)}", location);
                location += s.GetScriptSize();
            }

            using (StreamWriter z = new StreamWriter("map.txt"))
            {
                for (int i = 0; i < script_map.Count(); i++)
                    z.Write($"{script_map.Keys.ElementAt(i)} : {script_map[script_map.Keys.ElementAt(i)]}\n");
            }

            b.Close();
        }

    }
}
