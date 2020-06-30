using System;
using System.Collections.Generic;
using System.IO;

namespace BeaterScriptEngine
{
    public class ScriptParser
    {
        BinaryReader b;
        List<uint> pointers { get; set; }
        CommandsListHandler cmds;

        public ScriptParser(string script, string game)
        {
            // Initialize the script we will read from.
            this.b = new BinaryReader(File.Open(script, FileMode.Open));
            this.pointers = new List<uint>();
            this.cmds = new CommandsListHandler(game);
        }

        public List<uint> Addresses
        {
            get
            {
                return this.pointers;
            }
            set
            {
                this.pointers = value;
            }
        }

        public void FindScriptAddresses()
        {
            b.BaseStream.Position = 0;

            ushort next = b.ReadUInt16();
            while (next != 0xFD13)
            {
                b.BaseStream.Position -= 2;
                var addr = b.ReadUInt32();
                this.pointers.Add(addr + (uint)b.BaseStream.Position);
                next = b.ReadUInt16();
            }
        }

        public List<Command> ReadScript(uint address)
        {
            List<Command> script = new List<Command>();
            bool isEnd = false;
            b.BaseStream.Position = address;

            while (!isEnd)
            {
                var id = b.ReadUInt16();
                Command c;
                try
                {
                    c = cmds.commands[id];
                }
                catch (KeyNotFoundException)
                {
                    // This may not be an unimplemented command. It could very well be some arbitrary binary.
                    Console.WriteLine($"Unimplemented command: {id}");
                    continue;
                }

                List<object> parameters = new List<object>();

                foreach (Type t in c.Types)
                    if (t == typeof(uint))
                        parameters.Add(b.ReadUInt32());
                    else if (t == typeof(ushort))
                        parameters.Add(b.ReadUInt16());
                    else if (t == typeof(byte))
                        parameters.Add(b.ReadByte());

                // TODO: Implement function parsing.
                if (c.HasFunction)
                    Console.WriteLine($"A function was detected at {b.BaseStream.Position + (uint)parameters[parameters.Count - 1]}.");

                // TODO: Implement movement parsing.
                if (c.HasMovement)
                    Console.WriteLine($"A movement was detected at {b.BaseStream.Position + (uint)parameters[parameters.Count - 1]}.");

                c.Parameters = parameters.ToArray();

                script.Add(c);

                if (c.Name == "EndScript" || c.Name == "UnconditionalJump")
                    isEnd = true;
            }

            return script;
        }

    }
}
