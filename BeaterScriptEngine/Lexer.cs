using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Runtime.Remoting.Messaging;
using System.Runtime.CompilerServices;

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

        public void WriteScript(List<Script> scripts, List<Script> functions, List<List<Movement>> movements, string path)
        {
            // Calculate size of header section. We will use this later.
            int header_size = (scripts.Count * 0x4) + 2;
            Dictionary<string, int> script_map = new Dictionary<string, int>();
            Dictionary<int, string> function_calls = new Dictionary<int, string>();

            List<int> header = new List<int>();

            for (int i = 0; i < scripts.Count; i++)
                header.Add(0x0);

            int currentOffset = header_size;

            foreach (Script s in functions)
            {
                script_map.Add($"Function{functions.IndexOf(s)}", currentOffset);
                byte[][] buf = s.getScriptBytes();
                foreach (byte[] arr in buf)
                    b.Write(arr);
            }

            foreach (Script s in scripts)
            {
                script_map.Add($"Script{scripts.IndexOf(s)}", currentOffset);
                byte[][] buf = s.getScriptBytes();
                foreach (byte[] arr in buf)
                    b.Write(arr);
            }


            b.Close();
        }

    }
}
