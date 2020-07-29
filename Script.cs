using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeaterScript
{
    public class Script
    {
        public List<Command> Commands { get; }

        public Script()
        {
            Commands = new List<Command>();
        }

        public void Add(Command c)
        {
            Commands.Add(c);
        }

        public bool Equals(Script s) => s.Commands.Equals(Commands);
    }
}
