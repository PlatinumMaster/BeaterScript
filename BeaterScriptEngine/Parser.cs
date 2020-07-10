using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace BeaterScriptEngine
{
    public class ScriptParser
    {
        private BinaryReader b;
        private List<int> pointers { get; }
        private List<List<Command>> scripts { get; }
        private List<List<Command>> functions { get; }
        private List<List<Movement>> movements { get; }

        private List<int> parsed_functions;
        private List<int> parsed_movements;
        CommandsListHandler cmds;

        public ScriptParser(string script, string game)
        {
            // Initialize the script we will read from.
            this.b = new BinaryReader(File.Open(script, FileMode.Open));
            this.cmds = new CommandsListHandler(game);

            this.pointers = new List<int>();
            this.functions = new List<List<Command>>();
            this.movements = new List<List<Movement>>();
            this.parsed_functions = new List<int>();
            this.parsed_movements = new List<int>();

            this.FindScriptAddresses();
            this.scripts =  this.ReadScripts();
            this.b.Close();
        }

        public List<int> Addresses
        {
            get
            {
                return this.pointers;
            }
        }

        public List<List<Command>> Functions
        {
            get
            {
                return this.functions;
            }
        }

        public List<List<Command>> Scripts
        {
            get
            {
                return this.scripts;
            }
        }

        public List<List<Movement>> Movements
        {
            get
            {
                return this.movements;
            }
        }

        public void FindScriptAddresses()
        {
            b.BaseStream.Position = 0;

            ushort next = b.ReadUInt16();
            while (next != 0xFD13)
            {
                b.BaseStream.Position -= 2;
                int addr = b.ReadInt32();
                this.pointers.Add(addr + (int)b.BaseStream.Position);
                next = b.ReadUInt16();
            }
        }

        public List<Movement> ReadMovement(int address)
        {
            List<Movement> movement = new List<Movement>();
            bool isEnd = false;

            b.BaseStream.Position = address;

            while (!isEnd)
            {
                movement.Add(new Movement(b.ReadInt16().ToString(), b.ReadInt16()));

                if (b.ReadInt16() == 0xFE && b.ReadInt16() == 0x00)
                    isEnd = true;

                b.BaseStream.Position -= 0x4;
            }

            movement.Add(new Movement("EndMovement", 0));
            return movement;
        }

        public List<Command> ReadScript(int address)
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
                    if (t == typeof(int))
                        parameters.Add(b.ReadInt32());
                    else if (t == typeof(short))
                        parameters.Add(b.ReadUInt16());
                    else if (t == typeof(sbyte))
                        parameters.Add(b.ReadByte());

                var originalPos = b.BaseStream.Position;
                int addr = (c.HasFunction || c.HasMovement) ? (int)b.BaseStream.Position + (int)parameters[parameters.Count - 1] : 0;

                if (c.HasFunction)
                {
                    Console.WriteLine($"A function was detected at {addr}.");

                    if (!parsed_functions.Contains(addr))
                    {
                        parsed_functions.Add(addr);
                        functions.Add(this.ReadScript(addr));
                    }

                    parameters[parameters.Count - 1] = $"Function{parsed_functions.IndexOf(addr)}";
                }
                else if (c.HasMovement)
                {
                    Console.WriteLine($"A movement was detected at {addr}.");

                    if (!parsed_movements.Contains(addr))
                    {
                        movements.Add(this.ReadMovement(addr));
                        parsed_movements.Add(addr);
                    }

                    parameters[parameters.Count - 1] = $"Movement{parsed_movements.IndexOf(addr)}";
                }

                c.Parameters = parameters.ToArray();

                b.BaseStream.Position = originalPos;
                script.Add(c);

                if (c.Name.Equals("EndScript") || c.Name.Equals("UnconditionalJump"))
                    isEnd = true;
            }


            return script;
        }

        public List<List<Command>> ReadScripts()
        {
            List<List<Command>> scripts = new List<List<Command>>();

            foreach (int addr in Addresses)
                scripts.Add(ReadScript(addr));
            return scripts;
        }

    }
}
