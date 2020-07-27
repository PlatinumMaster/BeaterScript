using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace BeaterScriptEngine
{
    public class ScriptParser
    {
        private BinaryReader b;
        public List<int> Addresses { get; }
        public Dictionary<int, Script> Scripts { get; }
        public Dictionary<int, Script> Functions { get; }
        public Dictionary<int, List<Movement>> Movements { get; }
        CommandsListHandler Handler;

        public ScriptParser(string script, string game)
        {
            // Initialize the script we will read from.
            b = new BinaryReader(File.Open(script, FileMode.Open));
            Handler = new CommandsListHandler(game);
            Functions = new Dictionary<int, Script>();
            Movements = new Dictionary<int, List<Movement>>();

            // Process the container here.
            Addresses = GetScriptAddresses();
            Scripts = ReadScripts();

            // We are done reading from it.
            b.Close();
        }

        public List<int> GetScriptAddresses()
        {
            List<int> addr = new List<int>();
            b.BaseStream.Position = 0;

            while (b.ReadUInt16() != 0xFD13)
            {
                b.BaseStream.Position -= 2;
                addr.Add(b.ReadInt32() + (int)b.BaseStream.Position);
            }
            return addr;
        }

        public List<Movement> ReadMovement(int address)
        {
            List<Movement> movement = new List<Movement>();
            b.BaseStream.Position = address;

            while (true)
            {
                var idx = b.ReadUInt16();
                movement.Add(new Movement(idx.ToString(), idx, b.ReadUInt16()));

                if (b.ReadInt32() == 0xFE)
                    break;

                b.BaseStream.Position -= 0x4;
            }

            movement.Add(new Movement("0xFE", 0xFE, 0));
            return movement;
        }

        public Script ReadScript(int address)
        {
            b.BaseStream.Position = Convert.ToInt64(address);

            Script script = new Script();
            while (true)
            {
                var id = b.ReadUInt16();
                Command c;
                try
                {
                    var def = Handler.commands[id];
                    c = new Command(def.Name, def.ID, def.HasFunction, def.HasMovement, def.Types);
                }
                catch (KeyNotFoundException)
                {
                    // This may not be an unimplemented command. It could very well be some arbitrary binary.
                    Console.WriteLine($"WARNING: Unimplemented command: {id}");
                    Console.WriteLine($"Position: {b.BaseStream.Position - 2}");
                    continue;
                }

                foreach (Type t in c.Types)
                    switch (t.Name)
                    {
                        case "Int32":
                            c.Parameters.Add(b.ReadInt32());
                            break;
                        case "UInt16":
                            c.Parameters.Add(b.ReadUInt16());
                            break;
                        case "Byte":
                            c.Parameters.Add(b.ReadByte());
                            break;
                    }

                int originalPos = Convert.ToInt32(b.BaseStream.Position);
                int targetAddress = c.HasFunction || c.HasMovement ? originalPos + Convert.ToInt32(c.Parameters.Last()) : 0;

                if (c.HasFunction)
                {
                    if (!Functions.Keys.ToList().Contains(targetAddress))
                    {
                        Functions.Add(targetAddress, new Script());
                        Functions[targetAddress] = ReadScript(targetAddress);
                        Console.WriteLine($"A function was detected at {targetAddress}.");
                    }
                    c.Parameters[c.Parameters.Count - 1] = $"Function{Functions.Keys.ToList().IndexOf(targetAddress)}";
                }
                else if (c.HasMovement)
                {
                    if (!Movements.Keys.Contains(targetAddress))
                    {
                        Movements.Add(targetAddress, new List<Movement>());
                        Movements[targetAddress] = ReadMovement(targetAddress);
                        Console.WriteLine($"A movement was detected at {targetAddress}.");
                    }
                    c.Parameters[c.Parameters.Count - 1] = $"Movement{Movements.Keys.ToList().IndexOf(targetAddress)}";
                }

                b.BaseStream.Position = originalPos;
                script.Add(c);

                if (c.Name.Equals("EndScript") || c.Name.Equals("UnconditionalJump") || c.Name.Equals("EndRoutine"))
                    break;
            }

            return script;
        }

        public Dictionary<int, Script> ReadScripts()
        {
            Dictionary<int, Script> d = new Dictionary<int, Script>();
            foreach (int Address in Addresses)
                d.Add(Address, ReadScript(Address));

            return d;
        }

    }
}
