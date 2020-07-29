using System.Collections.Generic;

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
