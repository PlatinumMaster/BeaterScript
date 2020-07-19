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

        public void WriteScript(List<List<Command>> scripts, List<List<Command>> functions, List<List<Movement>> movements, string path)
        {
            // Calculate size of header section. We will use this later.
            uint header_size = (uint)(scripts.Count * 0x4) + 2;

            List<int> header = new List<int>();

            for (int i = 0; i < scripts.Count; i++)
                header.Add(0x0);
            b.Close();
        }

    }
}
