using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Runtime.Remoting.Messaging;
using System.Runtime.CompilerServices;
using System.Collections;

namespace BeaterScriptEngine
{
    public class ScriptLexer
    {
        BinaryWriter b;
        List<uint> pointers { get; set; }
        CommandsListHandler cmds;
        string path;

        public ScriptLexer(string script, string game)
        {
            // Initialize the script we will write to.
            this.path = script;
            this.b = new BinaryWriter(File.Open(script, FileMode.OpenOrCreate));
            this.cmds = new CommandsListHandler(game);
            this.pointers = new List<uint>();
        }

        public void WriteScript(List<Script> scripts, List<Script> functions, List<List<Movement>> movements)
        {
            // Less messy. But... still messy.
            uint location = (uint)scripts.Count * 0x4 + 2; // Size of header section.
            Dictionary<string, uint> script_map = new Dictionary<string, uint>();

            // Information gathering time.
            // For absolutely no reason, I will place all functions first. Problem?
            foreach (Script s in functions)
            {
                script_map.Add($"Function{functions.IndexOf(s)}", location);
                location += s.GetScriptSize();
            }

            // Okay now we have scripts.
            foreach (Script s in scripts)
            {
                script_map.Add($"Script{scripts.IndexOf(s)}", location);
                pointers.Add(location - 4 * (uint)scripts.IndexOf(s) - 4);
                location += s.GetScriptSize();
            }

            // Now the funky weirdo cousin, movements.
            foreach (List<Movement> m in movements)
            {
                script_map.Add($"Movement{movements.IndexOf(m)}", location);
                location += 0x4;
            }

            using (StreamWriter z = new StreamWriter(Path.Combine(Directory.GetParent(this.path).FullName, Path.GetFileNameWithoutExtension(this.path) + ".map")))
                for (int i = 0; i < script_map.Count(); i++)
                    z.Write($"{script_map.Keys.ElementAt(i)} : {script_map[script_map.Keys.ElementAt(i)]}\n");
            
            // Now we begin writing.
            foreach (uint pointer in pointers)
                b.Write(BitConverter.GetBytes(pointer));

            b.Write((ushort)0xFD13);

            location = (uint)scripts.Count * 0x4 + 2; // Size of header section.

            foreach (Script s in functions)
                foreach (Command c in s)
                {
                    if (c.HasFunction || c.HasMovement)
                        c.Parameters[c.Parameters.Length - 1] = script_map[(string)c.Parameters[c.Parameters.Length - 1]] - location - 4;
                    b.Write(c.ToBytes());
                    location += c.Size();
                }

            foreach (Script s in scripts)
                foreach (Command c in s)
                {
                    if (c.HasFunction || c.HasMovement)
                        c.Parameters[c.Parameters.Length - 1] = script_map[(string)c.Parameters[c.Parameters.Length - 1]] - location - 4;
                    b.Write(c.ToBytes());
                    location += c.Size();
                }

            foreach (List<Movement> movement in movements)
                foreach (Movement m in movement)
                {
                    b.Write(m.ToBytes());
                    location += 0x4;
                }

            // Should be good. Let's hope...
            b.Close();
        }

    }
}
