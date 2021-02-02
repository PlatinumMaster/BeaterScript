using System.Collections.Generic;

namespace BeaterScript
{
    public class Script
    {
        public List<Command> Commands { get; } = new List<Command>();

        public void Add(Command c)
        {
            Commands.Add(c);
        }

        public bool Equals(Script s) => s.Commands.Equals(Commands);
        public ScriptEnumerator GetEnumerator() => new ScriptEnumerator(Commands);

        public class ScriptEnumerator
        {
            int nIndex;
            private List<Command> commands;
            public ScriptEnumerator(List<Command> s)
            {
                commands = s;
                nIndex = -1;
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
