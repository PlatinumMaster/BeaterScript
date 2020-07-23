using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeaterScriptEngine
{
    public class Script
    {
        private uint script_size;
        private List<Command> commands;

        public Script()
        {
            this.commands = new List<Command>();
            this.script_size = 0;
        }

        public void Add(Command c)
        {
            this.commands.Add(c);
            this.script_size += c.Size();
        }

        public uint GetScriptSize()
        {
            return this.script_size;
        }

        public List<Command> GetScript()
        {
            return this.commands;
        }

        public ScriptEnumerator GetEnumerator()
        {  
          return new ScriptEnumerator(this.commands);
        }

    /*
    public byte[][] GetScriptBytes()
    {
        List<byte[]> bytes = new List<byte[]>();
        foreach (Command c in this.commands)
            bytes.Add(c.ToBytes());
        return bytes.ToArray();
    }
    */

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
