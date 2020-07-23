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
        private List<Script> scripts { get; }
        private List<Script> functions { get; }
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
            this.functions = new List<Script>();
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

        public List<Script> Functions
        {
            get
            {
                return this.functions;
            }
        }

        public List<Script> Scripts
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
                pointers.Add(addr + (int)b.BaseStream.Position);
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
                var idx = b.ReadUInt16();
                movement.Add(new Movement(idx.ToString(), idx, b.ReadUInt16()));

                if (b.ReadInt32() == 0xFE)
                    isEnd = true;
                else
                    b.BaseStream.Position -= 0x4;
            }

            movement.Add(new Movement("EndMovement", 0xFE, 0));
            return movement;
        }

        public Script ReadScript(int address)
        {
            Script script = new Script();
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
                    else if (t == typeof(ushort))
                        parameters.Add(b.ReadUInt16());
                    else if (t == typeof(byte))
                        parameters.Add(b.ReadByte());

                int originalPos = (int)b.BaseStream.Position;

                if (c.HasFunction)
                {
                    var addr = originalPos + (int)parameters.Last();
                    if (!parsed_functions.Contains(addr))
                    {
                        parsed_functions.Add(addr);
                        try
                        {
                            functions.Add(this.ReadScript(addr));
                            Console.WriteLine($"A function was detected at {addr}.");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"@ Original position: {originalPos}\n@ Address Value: {addr - originalPos}");
                            //throw e;
                        }
                    }
                    parameters[parameters.Count - 1] = $"Function{parsed_functions.LastIndexOf(addr)}";
                }
                
                if (c.HasMovement)
                {
                    var addr = originalPos + (int)parameters.Last();
                    if (!parsed_movements.Contains(addr))
                    {
                        parsed_movements.Add(addr);
                        movements.Add(this.ReadMovement(addr));
                        Console.WriteLine($"A movement was detected at {addr}.");
                    }
                    parameters[parameters.Count - 1] = $"Movement{parsed_movements.LastIndexOf(addr)}";
                }

                c.Parameters = parameters.ToArray();

                b.BaseStream.Position = originalPos;
                script.Add(c);

                if (c.Name.Equals("EndScript") || c.Name.Equals("UnconditionalJump"))
                    isEnd = true;
            }


            return script;
        }

        public List<Script> ReadScripts()
        {
            List<Script> scripts = new List<Script>();

            foreach (int addr in Addresses)
                scripts.Add(ReadScript(addr));
            return scripts;
        }

    }
}
