using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeaterScriptEngine
{
    public class Script
    {
        public uint Size { get; set; }
        public List<Command> Commands { get; }

        public Script()
        {
            Commands = new List<Command>();
            Size = 0;
        }

        public void Add(Command c)
        {
            Commands.Add(c);
            Size += c.Size();
        }

        public ScriptEnumerator GetEnumerator() => new ScriptEnumerator(Commands);
 
    public class ScriptEnumerator
        {
            int nIndex;
            private List<Command> commands;
            public ScriptEnumerator(List<Command> s)
            {
                commands = s;
                nIndex = 0;
            }

            public bool MoveNext()
            {
                nIndex++;
                return (nIndex < commands.Count);
            }

            public Command Current => commands[nIndex];
        }
    }
}
