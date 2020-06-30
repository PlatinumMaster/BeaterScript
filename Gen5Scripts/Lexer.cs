using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace BeaterScriptEngine
{
    public class ScriptLexer
    {
        BinaryWriter b;
        List<uint> pointers { get; set; }
        CommandsListHandler cmds;

        public ScriptLexer(string script, string game)
        {
            // Initialize the script we will read from.
            this.b = new BinaryWriter(File.Open(script, FileMode.OpenOrCreate));
            this.cmds = new CommandsListHandler(game);
        }

        public void WriteScript(string text)
        {
            foreach (string line in text.Split('\n'))
            {
                string cmd;
                try
                {
                    cmd = line.Substring(0, line.IndexOf("("));
                }
                catch (ArgumentOutOfRangeException)
                {
                    continue;
                }

                b.Write(cmds.command_map[cmd]);

                // This means we actually have parameters.
                int string_delta = line.IndexOf(")") - (line.IndexOf("(") + 1);

                if (string_delta == 0)
                    continue;

                var parameters = line.Substring(line.IndexOf("(") + 1, string_delta);
                var param_types = cmds.commands[cmds.command_map[cmd]];
                int i = 0;
                foreach (var num in parameters.Replace(" ", "").Split(','))
                {
                    if (param_types.Types[i] == typeof(uint))
                        b.Write(uint.Parse(num));
                    else if (param_types.Types[i] == typeof(ushort))
                        b.Write(ushort.Parse(num));
                    else if (param_types.Types[i] == typeof(byte))
                        b.Write(byte.Parse(num));
                    i++;
                }
            }
            b.Close();
        }

    }
}
