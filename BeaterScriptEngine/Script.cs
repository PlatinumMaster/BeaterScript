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

        public uint getScriptSize()
        {
            return this.script_size;
        }

        public List<Command> getScript()
        {
            return this.commands;
        }

        public byte[][] getScriptBytes()
        {
            List<byte[]> bytes = new List<byte[]>();
            foreach (Command c in commands)
                bytes.Add(c.ToBytes());

            return bytes.ToArray();
        }
    }
}
